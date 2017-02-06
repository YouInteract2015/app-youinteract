using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Contacts.Card
{
    public class EmailAddressCollection : IEnumerable
    {
        private vCard m_pOwner = null;
        private List<EmailAddress> m_pCollection = null;

        internal EmailAddressCollection(vCard owner)
        {
            m_pOwner = owner;
            m_pCollection = new List<EmailAddress>();

            foreach (Item item in owner.Items.Get("EMAIL"))
            {
                m_pCollection.Add(EmailAddress.Parse(item));
            }
        }


        #region method Add

        public EmailAddress Add(EmailAddressType_enum type, string email)
        {
            Item item = m_pOwner.Items.Add("EMAIL", EmailAddress.EmailTypeToString(type), "");
            item.SetDecodedValue(email);
            EmailAddress emailAddress = new EmailAddress(item, type, email);
            m_pCollection.Add(emailAddress);

            return emailAddress;
        }

        #endregion

        #region method Remove

        public void Remove(EmailAddress item)
        {
            m_pOwner.Items.Remove(item.Item);
            m_pCollection.Remove(item);
        }

        #endregion

        #region method Clear

        public void Clear()
        {
            foreach (EmailAddress email in m_pCollection)
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

        public EmailAddress this[int index]
        {
            get { return m_pCollection[index]; }
        }

        #endregion

    }
}
