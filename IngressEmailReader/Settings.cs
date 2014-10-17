using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using IngressEmailReader.Properties;

namespace IngressEmailReader
{
    [Serializable]
    public class EmailSettings
    {
        public string HostAddress;
        public ushort HostPort;
        public string Username;

        [NonSerialized]
        public string Folder;
        [NonSerialized]
        public string Password;
        [NonSerialized]
        public bool SslEnabled;

        public bool Equals(EmailSettings settings)
        {
            return settings != null &&
                   HostAddress == settings.HostAddress &&
                   HostPort == settings.HostPort &&
                   Username == settings.Username;
        }

        public void Save()
        {
            SaveSettings(this);
        }

        public void Load()
        {
            EmailSettings settings = LoadSettings();

            if (settings == null)
            {
                return;
            }

            HostAddress = settings.HostAddress;
            HostPort = settings.HostPort;
            Folder = settings.Folder;
            Username = settings.Username;
            Password = settings.Password;
            SslEnabled = settings.SslEnabled;
        }

        public static void SaveSettings(EmailSettings settings)
        {
            Settings.Default.HostAddress = settings.HostAddress;
            Settings.Default.HostPort = settings.HostPort;
            Settings.Default.Folder = settings.Folder;
            Settings.Default.Username = settings.Username;
            Settings.Default.Password = settings.Password;
            Settings.Default.SslEnabled = settings.SslEnabled;
            Settings.Default.Save();
        }

        public static EmailSettings LoadSettings()
        {
            EmailSettings settings = new EmailSettings();
            settings.HostAddress = Settings.Default.HostAddress;
            settings.HostPort = Settings.Default.HostPort;
            settings.Folder = Settings.Default.Folder;
            settings.Username = Settings.Default.Username;
            settings.Password = Settings.Default.Password;
            settings.SslEnabled = Settings.Default.SslEnabled;

            return settings;
        }
    }
}
