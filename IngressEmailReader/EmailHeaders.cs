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
    public class EmailItems
    {
        public EmailSettings Settings = null;
        public List<EmailItem> Items { get; set; }
        public static readonly string HeadersPath = Assembly.GetExecutingAssembly().Location + ".items";

        public EmailItems()
        {
            Items = new List<EmailItem>();
        }

        public void Save()
        {
            SaveItems(this);
        }

        public static void SaveItems(EmailItems emailHeaders)
        {
            using (Stream stream = File.OpenWrite(HeadersPath))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(stream, emailHeaders);
            }
        }

        public static EmailItems LoadItems()
        {
            if (!File.Exists(HeadersPath))
            {
                return null;
            }

            try
            {
                using (Stream stream = File.OpenRead(HeadersPath))
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    return serializer.Deserialize(stream) as EmailItems;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}
