using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Contacts.Card
{
    public class PhoneNumberCollection : IEnumerable
    {
        private vCard m_pOwner = null;
        private List<PhoneNumber> m_pCollection = null;

        internal PhoneNumberCollection(vCard owner)
        {
            m_pOwner = owner;
            m_pCollection = new List<PhoneNumber>();

            foreach (Item item in owner.Items.Get("TEL"))
            {
                m_pCollection.Add(PhoneNumber.Parse(item));
            }
        }


        #region method Add

        public void Add(PhoneNumberType_enum type, string number)
        {
            Item item = m_pOwner.Items.Add("TEL", PhoneNumber.PhoneTypeToString(type), number);
            m_pCollection.Add(new PhoneNumber(item, type, number));
        }

        #endregion

        #region method Remove

        public void Remove(PhoneNumber item)
        {
            m_pOwner.Items.Remove(item.Item);
            m_pCollection.Remove(item);
        }

        #endregion

        #region method Clear

        public void Clear()
        {
            foreach (PhoneNumber number in m_pCollection)
            {
                m_pOwner.Items.Remove(number.Item);
            }
            m_pCollection.Clear();
        }

        #endregion


        #region interface IEnumerator

        public IEnumerator GetEnumerator()
        {
            return m_pCollection.GetEnumerator();
        }

        #endregion

        #region Properties Implementation

        public int Count
        {
            get { return m_pCollection.Count; }
        }

        #endregion

    }
}