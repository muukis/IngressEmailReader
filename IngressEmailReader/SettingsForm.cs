using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Joshi.Utils.Imap;

namespace IngressEmailReader
{
    public partial class SettingsForm : Form
    {
        public EmailSettings Settings { get; set; }

        public SettingsForm()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void ShowErrorMessage(string format, params object[] args)
        {
            MessageBox.Show(string.Format(format, args), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowInfoMessage(string format, params object[] args)
        {
            MessageBox.Show(string.Format(format, args), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool CopyFormToSettings()
        {
            Settings = new EmailSettings();

            if (tbHostAddress.Text.Contains(":"))
            {
                string[] hostAddressArray = tbHostAddress.Text.Trim().Split(':');

                if (hostAddressArray.Length != 2)
                {
                    ShowErrorMessage("Invalid host address value!");
                    return false;
                }

                string hostAddress = hostAddressArray[0].Trim();

                if (hostAddress.Length == 0)
                {
                    ShowErrorMessage("Invalid host address value!");
                    return false;
                }

                string hostPortValue = hostAddressArray[1].Trim();
                ushort hostPort;

                if (hostPortValue.Length == 0 || !ushort.TryParse(hostPortValue, out hostPort))
                {
                    ShowErrorMessage("Invalid host address value!");
                    return false;
                }

                Settings.HostAddress = hostAddress;
                Settings.HostPort = hostPort;
            }
            else
            {
                Settings.HostAddress = tbHostAddress.Text.Trim();
                Settings.HostPort = 0;

                if (Settings.HostAddress.Length == 0)
                {
                    ShowErrorMessage("Invalid host address value!");
                    return false;
                }
            }

            Settings.Folder = tbFolder.Text.Trim();
            Settings.Username = tbUsername.Text.Trim();
            Settings.Password = tbPassword.Text.Trim();
            Settings.SslEnabled = cbSslEnabled.Checked;

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CopyFormToSettings())
            {
                return;
            }

            Settings.Save();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                Settings = EmailSettings.LoadSettings();
            }
            catch (Exception)
            {
                ShowErrorMessage("Loading settings failed!");
                return;
            }

            if (Settings == null)
            {
                return;
            }

            if (Settings.HostPort > 0)
            {
                tbHostAddress.Text = string.Format("{0}:{1}", Settings.HostAddress, Settings.HostPort);
            }
            else
            {
                tbHostAddress.Text = Settings.HostAddress;
            }

            tbFolder.Text = Settings.Folder;
            tbUsername.Text = Settings.Username;
            tbPassword.Text = Settings.Password;
            cbSslEnabled.Checked = Settings.SslEnabled;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (!CopyFormToSettings())
            {
                return;
            }

            Imap imap = new Imap();

            try
            {
                imap.Login(Settings.HostAddress,
                           (Settings.HostPort > 0
                                ? Settings.HostPort
                                : (Settings.SslEnabled ? (ushort) 993 : (ushort) 143)),
                           Settings.Username,
                           Settings.Password, Settings.SslEnabled);

                imap.SelectFolder(Settings.Folder);
                imap.ExamineFolder(Settings.Folder);

                ShowInfoMessage("Your connection settings seems to be working fine.");
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
    }
}
