using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Joshi.Utils.Imap;

namespace IngressEmailReader
{
    public partial class MainForm : Form
    {
        private Settings settings = null;
        private EmailItems emailItems = null;
        private EmailSorter lvwColumnSorter;

        public MainForm()
        {
            InitializeComponent();

            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwColumnSorter = new EmailSorter();
            lvIngressEmails.ListViewItemSorter = lvwColumnSorter;
        }

        private void ShowErrorMessage(string format, params object[] args)
        {
            MessageBox.Show(string.Format(format, args), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowInfoMessage(string format, params object[] args)
        {
            MessageBox.Show(string.Format(format, args), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pbSettings_Click(object sender, EventArgs e)
        {
            SettingsForm dlg = new SettingsForm();

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            Imap imap = new Imap();

            try
            {
                if (settings == null)
                {
                    settings = Settings.LoadSettings();

                    if (settings == null)
                    {
                        pbSettings_Click(sender, e);
                        return;
                    }
                }

                if (emailItems == null)
                {
                    emailItems = EmailItems.LoadItems() ?? new EmailItems();
                }

                if (!settings.Equals(emailItems.Settings))
                {
                    emailItems.Items.Clear();
                    emailItems.Settings = ObjectCopier.Clone(settings);
                    lvIngressEmails.Items.Clear();
                }

                imap.Login(settings.HostAddress,
                           (settings.HostPort.HasValue
                                ? settings.HostPort.Value
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
                int newItems = 0;

                foreach (string emailFromIngressSupport in emailsFromIngressSupport)
                {
                    if (string.IsNullOrEmpty(emailFromIngressSupport) ||
                        emailItems.Items.FirstOrDefault(item => item.Id == emailFromIngressSupport) != null)
                    {
                        continue;
                    }

                    imap.FetchPartHeader(emailFromIngressSupport, "0", emailHeaders);

                    EmailItem emailItem = new EmailItem();
                    emailItem.Id = emailFromIngressSupport;
                    emailItem.Headers = emailHeaders.Cast<string>().ToList();
                    emailItems.Items.Add(emailItem);

                    lvIngressEmails.Items.Add(emailItem.CreateListViewItem());

                    newItems++;
                }

                if (newItems > 0)
                {
                    emailItems.Save();
                    AutoResizeColumns();
                    ShowInfoMessage("New emails found! (Count: {0})", newItems);
                }
                else
                {
                    ShowInfoMessage("No new emails found.");
                }
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

        private void AutoResizeColumns()
        {
            lvIngressEmails.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void MainForm_Load(object sender, EventArgs e)
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

                AutoResizeColumns();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void lvIngressEmails_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lvIngressEmails.Sort();
        }
    }
}
