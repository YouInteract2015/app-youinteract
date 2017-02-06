using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Contacts.Card
{
    public class DeliveryAddressCollection : IEnumerable
    {
        private vCard m_pOwner = null;
        private List<DeliveryAddress> m_pCollection = null;


        internal DeliveryAddressCollection(vCard owner)
        {
            m_pOwner = owner;
            m_pCollection = new List<DeliveryAddress>();

            foreach (Item item in owner.Items.Get("ADR"))
            {
                m_pCollection.Add(DeliveryAddress.Parse(item));
            }
        }


        #region method Add


        public void Add(DeliveryAddressType_enum type, string postOfficeAddress, string street, string city, string state, string postalCode, string country)
        {
            string value = "" +
                postOfficeAddress + ";" +
                street + ";" +
                city + ";" +
                state + ";" +
                postalCode + ";" +
                country;

            Item item = m_pOwner.Items.Add("ADR", DeliveryAddress.AddressTypeToString(type), "");
            item.SetDecodedValue(value);
            m_pCollection.Add(new DeliveryAddress(item, type, postOfficeAddress, street, city, state, postalCode, country));
        }

        #endregion

        #region method Remove


        public void Remove(DeliveryAddress item)
        {
            m_pOwner.Items.Remove(item.Item);
            m_pCollection.Remove(item);
        }

        #endregion

        #region method Clear


        public void Clear()
        {
            foreach (DeliveryAddress email in m_pCollection)
            {
                m_pOwner.Items.Remove(email.Item);
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


        public DeliveryAddress this[int index]
        {
            get { return m_pCollection[index]; }
        }

        #endregion

    }
}
