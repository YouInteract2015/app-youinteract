using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Media;

namespace You_Contacts.Card
{
    public class vCard
    {
        private ItemCollection m_pItems = null;
        private DeliveryAddressCollection m_pAddresses = null;
        private PhoneNumberCollection m_pPhoneNumbers = null;
        private EmailAddressCollection m_pEmailAddresses = null;

        public vCard()
        {
            m_pItems = new ItemCollection();
            this.Version = "3.0";
            this.UID = Guid.NewGuid().ToString();
        }


        #region method ToByte

        public byte[] ToByte()
        {
            MemoryStream ms = new MemoryStream();
            ToStream(ms);
            return ms.ToArray();
        }

        #endregion

        #region method ToFile

        public void ToFile(string file)
        {
            using (FileStream fs = File.Create(file))
            {
                ToStream(fs);
            }
        }

        #endregion

        #region method ToStream

        public override string ToString()
        {
            StringBuilder retVal = new StringBuilder();
            retVal.Append("BEGIN:VCARD\r\n");
            foreach (Item item in m_pItems)
            {
                if (!item.Name.ToUpper().Equals("BEGIN") && !item.Name.ToUpper().Equals("END"))
                    retVal.Append(item.ToItemString() + "\r\n");
            }
            retVal.Append("END:VCARD\r\n");
            return retVal.ToString();
        }

        public void ToStream(Stream stream)
        {
            StringBuilder retVal = new StringBuilder();
            retVal.Append("BEGIN:VCARD\r\n");
            foreach (Item item in m_pItems)
            {
                if (!item.Name.ToUpper().Equals("BEGIN") && !item.Name.ToUpper().Equals("END"))
                    retVal.Append(item.ToItemString() + "\r\n");
            }
            retVal.Append("END:VCARD\r\n");

            byte[] data = System.Text.Encoding.UTF8.GetBytes(retVal.ToString());
            stream.Write(data, 0, data.Length);
        }

        #endregion


        #region static method ParseMultiple

        public static List<vCard> ParseMultiple(string file)
        {
            List<vCard> vCards = new List<vCard>();
            List<string> fileStrings = new List<string>();
            string line = "";
            bool hasBeginTag = false;
            using (FileStream fs = File.OpenRead(file))
            {
                TextReader r = new StreamReader(fs, System.Text.Encoding.Default);
                while (line != null)
                {
                    line = r.ReadLine();
                    if (line != null && line.ToUpper() == "BEGIN:VCARD")
                    {
                        hasBeginTag = true;
                    }
                    if (hasBeginTag)
                    {
                        fileStrings.Add(line);
                        if (line != null && line.ToUpper() == "END:VCARD")
                        {
                            vCard singleVcard = new vCard();
                            singleVcard.ParseStrings(fileStrings);
                            vCards.Add(singleVcard);
                            fileStrings.Clear();
                            hasBeginTag = false;
                        }
                    }
                }
            }
            return vCards;
        }

        #endregion

        #region method Parse

        public void Parse(string file)
        {
            List<string> fileStrings = new List<string>();
            string[] fileStringArray = File.ReadAllLines(file, System.Text.Encoding.Default);
            foreach (string fileString in fileStringArray)
            {
                fileStrings.Add(fileString);
            }
            ParseStrings(fileStrings);
        }

        public void Parse(TextReader reader)
        {
            List<string> fileStrings = new List<string>();
            string line = "";
            while (line != null)
            {
                line = reader.ReadLine();
                fileStrings.Add(line);
            }

            ParseStrings(fileStrings);
        }

        public void Parse(FileStream stream)
        {
            List<string> fileStrings = new List<string>();
            string line = "";
            TextReader r = new StreamReader(stream, System.Text.Encoding.Default);
            while (line != null)
            {
                line = r.ReadLine();
                fileStrings.Add(line);
            }
            ParseStrings(fileStrings);
        }

        public void ParseStrings(List<string> fileStrings)
        {
            m_pItems.Clear();
            m_pPhoneNumbers = null;
            m_pEmailAddresses = null;

            int lineCount = 0;
            string line = fileStrings[lineCount];

            while (line != null && line.ToUpper() != "BEGIN:VCARD")
            {
                line = fileStrings[lineCount++];
            }
            line = fileStrings[lineCount++];
            while (line != null && line.ToUpper() != "END:VCARD")
            {
                StringBuilder item = new StringBuilder();
                item.Append(line);
                line = fileStrings[lineCount++];
                while (line != null && (line.StartsWith("\t") || line.StartsWith(" ")))
                {
                    item.Append(line.Substring(1));
                    line = fileStrings[lineCount++];
                }

                string[] name_value = item.ToString().Split(new char[] { ':' }, 2);

                string[] name_params = name_value[0].Split(new char[] { ';' }, 2);
                string name = name_params[0];
                string parameters = "";
                if (name_params.Length == 2)
                {
                    parameters = name_params[1];
                }
                string value = "";
                if (name_value.Length == 2)
                {
                    value = name_value[1];
                }
                m_pItems.Add(name, parameters, value);
            }
        }

        #endregion


        #region Properties Implementation

        public ItemCollection Items
        {
            get { return m_pItems; }
        }

        public string Version
        {
            get
            {
                Item item = m_pItems.GetFirst("VERSION");
                if (item != null)
                {
                    return item.DecodedValue;
                }
                else
                {
                    return null;
                }
            }

            set { m_pItems.SetDecodedValue("VERSION", value); }
        }

        public Name Name
        {
            get
            {
                Item item = m_pItems.GetFirst("N");
                if (item != null)
                {
                    return Name.Parse(item);
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (value != null)
                {
                    m_pItems.SetDecodedValue("N", value.ToValueString());
                }
                else
                {
                    m_pItems.SetDecodedValue("N", null);
                }
            }
        }

        public string FormattedName
        {
            get
            {
                Item item = m_pItems.GetFirst("FN");
                if (item != null)
                {
                    return item.DecodedValue;
                }
                else
                {
                    return null;
                }
            }

            set { m_pItems.SetDecodedValue("FN", value); }
        }

        public string NickName
        {
            get
            {
                Item item = m_pItems.GetFirst("NICKNAME");
                if (item != null)
                {
                    return item.DecodedValue;
                }
                else
                {
                    return null;
                }
            }

            set { m_pItems.SetDecodedValue("NICKNAME", value); }
        }

        /*public Image Photo
        {
            get
            {
                Item item = m_pItems.GetFirst("PHOTO");
                if (item != null)
                {
                    return Image.FromStream(new MemoryStream(System.Text.Encoding.Default.GetBytes(item.DecodedValue)));
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (value != null)
                {
                    MemoryStream ms = new MemoryStream();
                    value.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                    m_pItems.SetValue("PHOTO", "ENCODING=b;TYPE=JPEG", Convert.ToBase64String(ms.ToArray()));
                }
                else
                {
                    m_pItems.SetValue("PHOTO", null);
                }
            }
        }*/

        public DateTime BirthDate
        {
            get
            {
                Item item = m_pItems.GetFirst("BDAY");
                if (item != null)
                {
                    string date = item.DecodedValue.Replace("-", "");
                    string[] dateFormats = new string[]{
                        "yyyyMMdd",
                        "yyyyMMddz"
                    };
                    return DateTime.ParseExact(date, dateFormats, System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None);
                }
                else
                {
                    return DateTime.MinValue;
                }
            }

            set
            {
                if (value != DateTime.MinValue)
                {
                    m_pItems.SetValue("BDAY", value.ToString("yyyyMMdd"));
                }
                else
                {
                    m_pItems.SetValue("BDAY", null);
                }
            }
        }

        public DeliveryAddressCollection Addresses
        {
            get
            {
                if (m_pAddresses == null)
                {
                    m_pAddresses = new DeliveryAddressCollection(this);
                }

                return m_pAddresses;
            }
        }

        public PhoneNumberCollection PhoneNumbers
        {
            get
            {
                if (m_pPhoneNumbers == null)
                {
                    m_pPhoneNumbers = new PhoneNumberCollection(this);
                }

                return m_pPhoneNumbers;
            }
        }

        public EmailAddressCollection EmailAddresses
        {
            get
            {
                if (m_pEmailAddresses == null)
                {
                    m_pEmailAddresses = new EmailAddressCollection(this);
                }

                return m_pEmailAddresses;
            }
        }

        public string Title
        {
            get
            {
                Item item = m_pItems.GetFirst("TITLE");
                if (item != null)
                {
                    return item.DecodedValue;
                }
                else
                {
                    return null;
                }
            }

            set { m_pItems.SetDecodedValue("TITLE", value); }
        }

        public string Role
        {
            get
            {
                Item item = m_pItems.GetFirst("ROLE");
                if (item != null)
                {
                    return item.DecodedValue;
                }
                else
                {
                    return null;
                }
            }

            set { m_pItems.SetDecodedValue("ROLE", value); }
        }

        public string Organization
        {
            get
            {
                Item item = m_pItems.GetFirst("ORG");
                if (item != null)
                {
                    return item.DecodedValue;
                }
                else
                {
                    return null;
                }
            }

            set { m_pItems.SetDecodedValue("ORG", value); }
        }

        public string NoteText
        {
            get
            {
                Item item = m_pItems.GetFirst("NOTE");
                if (item != null)
                {
                    return item.DecodedValue;
                }
                else
                {
                    return null;
                }
            }

            set { m_pItems.SetDecodedValue("NOTE", value); }
        }

        public string UID
        {
            get
            {
                Item item = m_pItems.GetFirst("UID");
                if (item != null)
                {
                    return item.DecodedValue;
                }
                else
                {
                    return null;
                }
            }

            set { m_pItems.SetDecodedValue("UID", value); }
        }

        public string HomeURL
        {
            get
            {
                Item[] items = m_pItems.Get("URL");
                foreach (Item item in items)
                {
                    if (item.ParametersString == "" || item.ParametersString.ToUpper().IndexOf("HOME") > -1)
                    {
                        return item.DecodedValue;
                    }
                }

                return null;
            }

            set
            {
                Item[] items = m_pItems.Get("URL");
                foreach (Item item in items)
                {
                    if (item.ParametersString.ToUpper().IndexOf("HOME") > -1)
                    {
                        if (value != null)
                        {
                            item.Value = value;
                        }
                        else
                        {
                            m_pItems.Remove(item);
                        }
                        return;
                    }
                }

                if (value != null)
                {
                    m_pItems.Add("URL", "HOME", value);
                }
            }
        }

        public string WorkURL
        {
            get
            {
                Item[] items = m_pItems.Get("URL");
                foreach (Item item in items)
                {
                    if (item.ParametersString.ToUpper().IndexOf("WORK") > -1)
                    {
                        return item.DecodedValue;
                    }
                }

                return null;
            }

            set
            {
                Item[] items = m_pItems.Get("URL");
                foreach (Item item in items)
                {
                    if (item.ParametersString.ToUpper().IndexOf("WORK") > -1)
                    {
                        if (value != null)
                        {
                            item.Value = value;
                        }
                        else
                        {
                            m_pItems.Remove(item);
                        }
                        return;
                    }
                }

                if (value != null)
                {
                    m_pItems.Add("URL", "WORK", value);
                }
            }
        }

        #endregion

    }
}
