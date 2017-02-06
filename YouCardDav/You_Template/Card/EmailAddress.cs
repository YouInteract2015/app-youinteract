using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Contacts.Card
{
    public class EmailAddress
    {
        private Item m_pItem = null;
        private EmailAddressType_enum m_Type = EmailAddressType_enum.Internet;
        private string m_EmailAddress = "";


        internal EmailAddress(Item item, EmailAddressType_enum type, string emailAddress)
        {
            m_pItem = item;
            m_Type = type;
            m_EmailAddress = emailAddress;
        }


        #region method Changed


        private void Changed()
        {
            m_pItem.ParametersString = EmailTypeToString(m_Type);
            m_pItem.SetDecodedValue(m_EmailAddress);
        }

        #endregion


        #region internal static method Parse


        internal static EmailAddress Parse(Item item)
        {
            EmailAddressType_enum type = EmailAddressType_enum.NotSpecified;
            if (item.ParametersString.ToUpper().IndexOf("PREF") != -1)
            {
                type |= EmailAddressType_enum.Preferred;
            }
            if (item.ParametersString.ToUpper().IndexOf("INTERNET") != -1)
            {
                type |= EmailAddressType_enum.Internet;
            }

            return new EmailAddress(item, type, item.DecodedValue);
        }

        #endregion

        #region internal static EmailTypeToString


        internal static string EmailTypeToString(EmailAddressType_enum type)
        {
            string retVal = "";
            if ((type & EmailAddressType_enum.Internet) != 0)
            {
                retVal += "INTERNET,";
            }
            if ((type & EmailAddressType_enum.Preferred) != 0)
            {
                retVal += "PREF,";
            }
            if (retVal.EndsWith(","))
            {
                retVal = retVal.Substring(0, retVal.Length - 1);
            }

            return retVal;
        }

        #endregion


        #region Properties Implementation


        public Item Item
        {
            get { return m_pItem; }
        }


        public EmailAddressType_enum EmailType
        {
            get { return m_Type; }

            set
            {
                m_Type = value;
                Changed();
            }
        }


        public string Email
        {
            get { return m_EmailAddress; }

            set
            {
                m_EmailAddress = value;
                Changed();
            }
        }

        #endregion

    }
}
