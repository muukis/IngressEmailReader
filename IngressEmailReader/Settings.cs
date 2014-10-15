using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace IngressEmailReader
{
    [Serializable]
    public class Settings
    {
        public static readonly string SettingsPath = Assembly.GetExecutingAssembly().Location + ".settings";

        public string HostAddress { get; set; }
        public ushort? HostPort { get; set; }
        public string Folder { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool SslEnabled { get; set; }

        public bool Equals(Settings settings)
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
            Settings settings = LoadSettings();

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

        public static void SaveSettings(Settings settings)
        {
            using (Stream stream = File.OpenWrite(SettingsPath))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(stream, settings);
            }
        }

        public static Settings LoadSettings()
        {
            if (!File.Exists(SettingsPath))
            {
                return null;
            }

            try
            {
                using (Stream stream = File.OpenRead(SettingsPath))
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    return serializer.Deserialize(stream) as Settings;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}
