using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IngressEmailReader.Properties;
using Joshi.Utils.Imap;

namespace IngressEmailReader
{
    public partial class MainForm : Form
    {
        public const string VERSION = "0.7.0.0";

        private Task task = null;
        private Task continuationTask = null;
        private readonly object taskLock = new object();

        private EmailSettings settings = null;
        private EmailItems emailItems = null;
        private EmailSorter lvwEmailSorter;
        private SummarySorter lvwSummarySorter;

        public MainForm()
        {
            InitializeComponent();

            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwEmailSorter = new EmailSorter();
            lvIngressEmails.ListViewItemSorter = lvwEmailSorter;

            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwSummarySorter = new SummarySorter();
            lvSummary.ListViewItemSorter = lvwSummarySorter;

            Text += string.Format(" - Version {0}", VERSION);
        }

        private void ShowErrorMessage(string format, params object[] args)
        {
            this.InvokeIfRequired(() =>
            {
                MessageBox.Show(string.Format(format, args), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }

        private void ShowInfoMessage(string format, params object[] args)
        {
            this.InvokeIfRequired(() =>
            {
                MessageBox.Show(string.Format(format, args), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }

        private void pbSettings_Click(object sender, EventArgs e)
        {
            SettingsForm dlg = new SettingsForm();

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            settings = dlg.Settings;
        }

        private void LoadTask(object sender, EventArgs e)
        {
            Imap imap = new Imap();

            try
            {
                this.InvokeIfRequired(() => pbMain.Value = 0);

                if (settings == null)
                {
                    settings = EmailSettings.LoadSettings();

                    if (settings == null)
                    {
                        this.InvokeIfRequired(() => pbSettings_Click(sender, e));
                        return;
                    }
                }

                this.InvokeIfRequired(ShowAll);

                if (emailItems == null)
                {
                    emailItems = EmailItems.LoadItems() ?? new EmailItems();
                }

                if (!settings.Equals(emailItems.Settings))
                {
                    emailItems.Items.Clear();
                    emailItems.Settings = ObjectCopier.Clone(settings);
                    this.InvokeIfRequired(() => lvIngressEmails.Items.Clear());
                }

                imap.Login(settings.HostAddress,
                           (settings.HostPort > 0
                                ? settings.HostPort
                                : (settings.SslEnabled ? (ushort)993 : (ushort)143)),
                           settings.Username,
                           settings.Password, settings.SslEnabled);

                imap.SelectFolder(settings.Folder);
                imap.ExamineFolder(settings.Folder);

                ArrayList emailsFromIngressSupport = new ArrayList();

                /*
Ingress Portal Photo Accepted:
Ingress Portal Submitted:
Ingress Portal Duplicate:
Ingress Portal Rejected:
Invalid Ingress Portal Report:
Ingress Portal Photo Submitted:
Ingress Portal Data Edit Reviewed:
Ingress Portal Data Edit Accepted:
Ingress Portal Live:
Ingress Portal Edits Submitted:
Ingress Portal Photo Rejected:
                */

                string[] searchParameters = new string[Properties.Settings.Default.SearchParameters.Count];
                Properties.Settings.Default.SearchParameters.CopyTo(searchParameters, 0);

                imap.SearchMessage(searchParameters, false, emailsFromIngressSupport);

                ArrayList emailHeaders = new ArrayList();
                List<string> emailIds = new List<string>(emailsFromIngressSupport.Count);

                foreach (string emailFromIngressSupport in emailsFromIngressSupport)
                {
                    if (string.IsNullOrEmpty(emailFromIngressSupport) ||
                        emailItems.Items.FirstOrDefault(item => item.Id == emailFromIngressSupport) != null)
                    {
                        continue;
                    }

                    emailIds.Add(emailFromIngressSupport);
                }

                this.InvokeIfRequired(() => pbMain.Maximum = emailIds.Count);

                foreach (string emailFromIngressSupport in emailIds)
                {
                    imap.FetchPartHeader(emailFromIngressSupport, "0", emailHeaders);

                    EmailItem emailItem = new EmailItem();
                    emailItem.Id = emailFromIngressSupport;
                    emailItem.Headers = emailHeaders.Cast<string>().ToList();
                    emailItems.Items.Add(emailItem);

                    this.InvokeIfRequired(() =>
                    {
                        lvIngressEmails.Items.Add(emailItem.CreateListViewItem());
                        pbMain.PerformStep();
                    });
                }

                if (emailIds.Count > 0)
                {
                    emailItems.Save();
                    this.InvokeIfRequired(ShowSummary);
                    ShowInfoMessage("New emails found! (Count: {0})", emailIds.Count);
                }
                else
                {
                    ShowInfoMessage("No new emails found.");
                }

                this.InvokeIfRequired(() =>
                {
                    AutoResizeColumns(lvIngressEmails);
                    pbMain.Value = 0;
                });
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
            finally
            {
                imap.LogOut();
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (task != null)
            {
                ShowErrorMessage("Previous task still running!");
                return;
            }

            pMain.Enabled = false;

            lock (taskLock)
            {
                task = Task.Factory.StartNew(() => LoadTask(sender, e));
                continuationTask = task.ContinueWith(continuation => this.InvokeIfRequired(() =>
                {
                    pMain.Enabled = true;

                    lock (taskLock)
                    {
                        task = null;
                    }
                }));
            }
        }

        private void AutoResizeColumns(ListView listView)
        {
            foreach (ColumnHeader column in listView.Columns)
            {
                column.Width = -2;
            }
        }

        private void ShowAllItems()
        {
            try
            {
                lvIngressEmails.Items.Clear();

                if (emailItems == null)
                {
                    emailItems = EmailItems.LoadItems() ?? new EmailItems();
                }

                foreach (EmailItem emailItem in emailItems.Items)
                {
                    lvIngressEmails.Items.Add(emailItem.CreateListViewItem());
                }

                if (lvIngressEmails.Items.Count > 0)
                {
                    AutoResizeColumns(lvIngressEmails);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void ShowSummary()
        {
            try
            {
                lvSummary.Items.Clear();

                var types = (from ListViewItem item in lvIngressEmails.Items select item.SubItems[0].Text).Distinct();

                foreach (string type in types)
                {
                    ListViewItem summaryItem = new ListViewItem(string.Format("Total count: {0}", type));
                    summaryItem.SubItems.Add(
                        (from ListViewItem item in lvIngressEmails.Items where item.SubItems[0].Text == type select item).Count().ToString());
                    lvSummary.Items.Add(summaryItem);
                }

                AutoResizeColumns(lvSummary);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void ShowAll()
        {
            ShowAllItems();
            ShowSummary();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ShowAll();
        }

        private void lvIngressEmails_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwEmailSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwEmailSorter.Order == SortOrder.Ascending)
                {
                    lvwEmailSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwEmailSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwEmailSorter.SortColumn = e.Column;
                lvwEmailSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lvIngressEmails.Sort();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lvIngressEmails.Items.Clear();
            emailItems.Items.Clear();
            emailItems.Save();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            lock (taskLock)
            {
                if (task != null)
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            ShowAll();
        }

        private void lvSummary_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwSummarySorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwSummarySorter.Order == SortOrder.Ascending)
                {
                    lvwSummarySorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwSummarySorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwSummarySorter.SortColumn = e.Column;
                lvwSummarySorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lvSummary.Sort();
        }

        private void btnTypeHierarchy_Click(object sender, EventArgs e)
        {
            const string AVGRESPONSETIME = "Average response time";

            try
            {
                ShowAllItems();
                List<TimeSpan> responseTimes = new List<TimeSpan>();

                foreach (string typeHierarchyValue in Settings.Default.TypeHierarchy)
                {
                    string[] typeHierarchy = typeHierarchyValue.Split('~');

                    if (typeHierarchy.Length != 3)
                    {
                        throw new SettingsPropertyWrongTypeException("Type hierarchy configuration value is invalid! Please configure application settings and restart the application.");
                    }

                    var rootItems = (from ListViewItem rootItem in lvIngressEmails.Items
                                     where
                                         string.Compare(rootItem.SubItems[0].Text, typeHierarchy[0],
                                                        StringComparison.InvariantCultureIgnoreCase) == 0
                                     select rootItem).ToList();

                    foreach (ListViewItem rootItem in rootItems)
                    {
                        if (rootItem.ImageIndex >= 0)
                        {
                            continue;
                        }

                        var subItem = (from ListViewItem item in lvIngressEmails.Items
                                       where string.Compare(item.SubItems[0].Text, typeHierarchy[1],
                                                            StringComparison.InvariantCultureIgnoreCase) == 0 &&
                                                            rootItem.SubItems[1].Text == item.SubItems[1].Text &&
                                             ((DateTime) rootItem.Tag) < ((DateTime) item.Tag)
                                       orderby ((DateTime) item.Tag) ascending 
                                       select item).FirstOrDefault();

                        if (subItem != null)
                        {
                            TimeSpan responseTime = ((DateTime) subItem.Tag) - ((DateTime) rootItem.Tag);

                            responseTimes.Add(responseTime);

                            rootItem.SubItems[3].Text = string.Format("{0} days", responseTime.Days);
                            rootItem.SubItems[3].Tag = responseTime.Ticks;

                            switch (typeHierarchy[2].ToUpper())
                            {
                                case "SUCCESS":
                                    rootItem.ImageIndex = 0;
                                    break;
                                case "FAIL":
                                    rootItem.ImageIndex = 1;
                                    break;
                                default:
                                    rootItem.ImageIndex = 2;
                                    break;
                            }

                            lvIngressEmails.Items.Remove(subItem);
                        }
                    }
                }

                double avgResponseTime = responseTimes.Sum(item => item.TotalDays) / (double)responseTimes.Count;
                string avgResponseTimeText = string.Format("{0:0.##} days", avgResponseTime);

                ListViewItem avgResponseTimeItem =
                    (from ListViewItem item in lvSummary.Items where item.Text == AVGRESPONSETIME select item)
                        .FirstOrDefault();

                if (avgResponseTimeItem != null)
                {
                    avgResponseTimeItem.SubItems[1].Text = avgResponseTimeText;
                }
                else
                {
                    ListViewItem newItem = new ListViewItem(AVGRESPONSETIME);
                    newItem.SubItems.Add(avgResponseTimeText);
                    lvSummary.Items.Add(newItem);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }
    }
}
