using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IngressEmailReader
{
    [Serializable]
    public class EmailItem
    {
        public string Id { get; set; }
        public List<string> Headers { get; set; }

        public string GetSubject()
        {
            if (Headers == null)
            {
                return null;
            }

            int subjectIndex = Headers.IndexOf("Subject") + 1;

            if (subjectIndex <= 0)
            {
                return null;
            }

            return ParseQuotedPrintableString(Headers[subjectIndex] ?? string.Empty);
        }

        public string GetDate(out DateTime date)
        {
            if (Headers == null)
            {
                date = DateTime.MinValue;
                return null;
            }

            int dateIndex = Headers.IndexOf("Date") + 1;

            if (dateIndex <= 0)
            {
                date = DateTime.MinValue;
                return null;
            }

            if (DateTime.TryParse(Headers[dateIndex] ?? string.Empty, out date))
            {
                return date.ToString(CultureInfo.CurrentCulture);
            }

            date = DateTime.MinValue;
            return string.Empty;
        }

        public ListViewItem CreateListViewItem()
        {
            string subject = GetSubject() ?? string.Empty;

            if (subject.IndexOf(':') < 0)
            {
                return new ListViewItem(subject);
            }

            string[] subjectSplitted = subject.Split(':');

            string col1 = subjectSplitted[0].Trim();
            string col2 = string.Join(":", subjectSplitted, 1, subjectSplitted.Length - 1).Trim();

            DateTime date;
            string col3 = GetDate(out date);

            ListViewItem retval = new ListViewItem(col1);
            retval.SubItems.Add(col2);
            retval.SubItems.Add(col3);
            retval.SubItems.Add(string.Empty);

            retval.Tag = date;

            return retval;
        }

        public static string ParseQuotedPrintableString(string text)
        {
            string retval = text;

            if (retval.StartsWith("=?") && retval.EndsWith("?="))
            {
                string[] subjectParts = retval.Substring(2, retval.Length - 4).Split(new[] { "?==?" }, StringSplitOptions.None);
                retval = string.Empty;

                foreach (string subjectPart in subjectParts)
                {
                    Attachment attachment = Attachment.CreateAttachmentFromString(string.Empty, string.Format("=?{0}?=", subjectPart));
                    retval += attachment.Name;
                }
            }

            return retval;
        }
    }
}
