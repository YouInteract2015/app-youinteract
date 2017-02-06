using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Contacts.Card
{
    public class PhoneNumber
    {
        private Item m_pItem = null;
        private PhoneNumberType_enum m_Type = PhoneNumberType_enum.Cellular;
        private string m_Number = "";

        internal PhoneNumber(Item item, PhoneNumberType_enum type, string number)
        {
            m_pItem = item;
            m_Type = type;
            m_Number = number;
        }


        #region method Changed

        private void Changed()
        {
            m_pItem.ParametersString = PhoneTypeToString(m_Type);
            m_pItem.Value = m_Number;
        }

        #endregion


        #region internal static method Parse

        internal static PhoneNumber Parse(Item item)
        {
            PhoneNumberType_enum type = PhoneNumberType_enum.NotSpecified;
            if (item.ParametersString.ToUpper().IndexOf("HOME") != -1)
            {
                type |= PhoneNumberType_enum.Home;
            }
            if (item.ParametersString.ToUpper().IndexOf("WORK") != -1)
            {
                type |= PhoneNumberType_enum.Work;
            }
            if (item.ParametersString.ToUpper().IndexOf("FAX") != -1)
            {
                type |= PhoneNumberType_enum.Fax;
            }
            if (item.ParametersString.ToUpper().IndexOf("CELL") != -1)
            {
                type |= PhoneNumberType_enum.Cellular;
            }


            return new PhoneNumber(item, type, item.Value);
        }

        #endregion

        #region internal static PhoneTypeToString

        internal static string PhoneTypeToString(PhoneNumberType_enum type)
        {
            string retVal = "";

            if ((type & PhoneNumberType_enum.Fax) != 0)
            {
                retVal += "FAX,";
            }
            if ((type & PhoneNumberType_enum.Home) != 0)
            {
                retVal += "HOME,";
            }
            if ((type & PhoneNumberType_enum.Cellular) != 0)
            {
                retVal += "VOICE,";
            }
            if ((type & PhoneNumberType_enum.Work) != 0)
            {
                retVal += "CELLULAR,";
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

        public PhoneNumberType_enum NumberType
        {
            get { return m_Type; }

            set
            {
                m_Type = value;
                Changed();
            }
        }

        public string Number
        {
            get { return m_Number; }

            set
            {
                m_Number = value;
                Changed();
            }
        }

        #endregion

    }
}
