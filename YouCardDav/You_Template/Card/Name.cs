using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Contacts.Card
{
    public class Name
    {
        private string m_LastName = "";
        private string m_FirstName = "";

        public Name(string lastName, string firstName)
        {
            m_LastName = lastName;
            m_FirstName = firstName;
        }

        internal Name()
        {
        }


        #region method ToValueString

        public string ToValueString()
        {
            return m_LastName + ";" + m_FirstName;
        }

        #endregion


        #region internal static method Parse

        internal static Name Parse(Item item)
        {
            string[] items = item.DecodedValue.Split(';');
            Name name = new Name();
            if (items.Length >= 1)
            {
                name.m_LastName = items[0];
            }
            if (items.Length >= 2)
            {
                name.m_FirstName = items[1];
            }

            return name;
        }

        #endregion


        #region Properties Implementation

        public string LastName
        {
            get { return m_LastName; }
        }

        public string FirstName
        {
            get { return m_FirstName; }
        }


        #endregion

    }
}
