using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Contacts.Card
{
    public class ItemCollection : IEnumerable
    {
        private List<Item> m_pItems = null;

        internal ItemCollection()
        {
            m_pItems = new List<Item>();
        }


        #region method Add

        public Item Add(string name, string parametes, string value)
        {
            Item item = new Item(name, parametes, value);
            m_pItems.Add(item);

            return item;
        }

        #endregion

        #region method Remove

        public void Remove(string name)
        {
            for (int i = 0; i < m_pItems.Count; i++)
            {
                if (m_pItems[i].Name.ToLower() == name.ToLower())
                {
                    m_pItems.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Remove(Item item)
        {
            m_pItems.Remove(item);
        }

        #endregion

        #region method Clear

        public void Clear()
        {
            m_pItems.Clear();
        }

        #endregion


        #region method GetFirst

        public Item GetFirst(string name)
        {
            foreach (Item item in m_pItems)
            {
                if (item.Name.ToLower() == name.ToLower())
                {
                    return item;
                }
            }

            return null;
        }

        #endregion

        #region method Get

        public Item[] Get(string name)
        {
            List<Item> retVal = new List<Item>();
            foreach (Item item in m_pItems)
            {
                if (item.Name.ToLower() == name.ToLower())
                {
                    retVal.Add(item);
                }
            }

            return retVal.ToArray();
        }

        #endregion

        #region method SetDecodedStringValue

        public void SetDecodedValue(string name, string value)
        {
            if (value == null)
            {
                Remove(name);
                return;
            }

            Item item = GetFirst(name);
            if (item != null)
            {
                item.SetDecodedValue(value);
            }
            else
            {
                item = new Item(name, "", "");
                m_pItems.Add(item);
                item.SetDecodedValue(value);
            }
        }

        #endregion

        #region method SetValue

        public void SetValue(string name, string value)
        {
            SetValue(name, "", value);
        }

        public void SetValue(string name, string parametes, string value)
        {
            if (value == null)
            {
                Remove(name);
                return;
            }

            Item item = GetFirst(name);
            if (item != null)
            {
                item.Value = value;
            }
            else
            {
                m_pItems.Add(new Item(name, parametes, value));
            }
        }

        #endregion


        #region interface IEnumerator

        public IEnumerator GetEnumerator()
        {
            return m_pItems.GetEnumerator();
        }

        #endregion

        #region Properties Implementation

        public int Count
        {
            get { return m_pItems.Count; }
        }

        #endregion

    }
}
