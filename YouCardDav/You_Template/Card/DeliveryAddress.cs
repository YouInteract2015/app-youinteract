using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Contacts.Card
{
    public class DeliveryAddress
    {
        private Item m_pItem = null;
        private DeliveryAddressType_enum m_Type =  DeliveryAddressType_enum.Work;
        private string m_Street = ""; 
        private string m_PostOfficeAddress = "";
        private string m_City = "";
        private string m_State = "";
        private string m_PostalCode = "";
        private string m_Country = "";

       
        internal DeliveryAddress(Item item, DeliveryAddressType_enum addressType, string postOfficeAddress, string street, string city, string state, string postalCode, string country)
        {
            m_pItem = item;
            m_Type = addressType;
            m_PostOfficeAddress = postOfficeAddress;
            m_Street = street;
            m_City = city;
            m_PostalCode = postalCode;
            m_Country = country;
        }


        #region method Changed

        private void Changed()
        {
            string value = "" +
                m_PostOfficeAddress + ";" +
                m_Street + ";" +
                m_City + ";" +
                m_PostalCode + ";" +
                m_Country;

            m_pItem.ParametersString = AddressTypeToString(m_Type);
            m_pItem.SetDecodedValue(value);
        }

        #endregion


        #region internal static method Parse


        internal static DeliveryAddress Parse(Item item)
        {
            DeliveryAddressType_enum type = DeliveryAddressType_enum.NotSpecified;
            if (item.ParametersString.ToUpper().IndexOf("PREF") != -1)
            {
                type |= DeliveryAddressType_enum.Preferred;
            }
            if (item.ParametersString.ToUpper().IndexOf("HOME") != -1)
            {
                type |= DeliveryAddressType_enum.Home;
            }
            if (item.ParametersString.ToUpper().IndexOf("WORK") != -1)
            {
                type |= DeliveryAddressType_enum.Work;
            }

            string[] items = item.DecodedValue.Split(';');
            return new DeliveryAddress(
                item,
                type,
                items.Length >= 1 ? items[0] : "",
                items.Length >= 2 ? items[1] : "",
                items.Length >= 3 ? items[2] : "",
                items.Length >= 4 ? items[3] : "",
                items.Length >= 5 ? items[4] : "",
                items.Length >= 6 ? items[5] : ""
            );
        }

        #endregion

        #region internal static AddressTypeToString


        internal static string AddressTypeToString(DeliveryAddressType_enum type)
        {
            string retVal = "";

            if ((type & DeliveryAddressType_enum.Home) != 0)
            {
                retVal += "HOME,";
            }

            if ((type & DeliveryAddressType_enum.Preferred) != 0)
            {
                retVal += "Preferred,";
            }
            if ((type & DeliveryAddressType_enum.Work) != 0)
            {
                retVal += "Work,";
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


        public DeliveryAddressType_enum AddressType
        {
            get { return m_Type; }

            set
            {
                m_Type = value;
                Changed();
            }
        }


        public string PostOfficeAddress
        {
            get { return m_PostOfficeAddress; }

            set
            {
                m_PostOfficeAddress = value;
                Changed();
            }
        }



        public string Street
        {
            get { return m_Street; }

            set
            {
                m_Street = value;
                Changed();
            }
        }


        public string City
        {
            get { return m_City; }

            set
            {
                m_City = value;
                Changed();
            }
        }


        public string State
        {
            get { return m_State; }

            set
            {
                m_State = value;
                Changed();
            }
        }


        public string PostalCode
        {
            get { return m_PostalCode; }

            set
            {
                m_PostalCode = value;
                Changed();
            }
        }


        public string Country
        {
            get { return m_Country; }

            set
            {
                m_Country = value;
                Changed();
            }
        }

        #endregion

    }
}
