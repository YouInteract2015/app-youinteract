using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using You_Contacts.Utils;
using You_Contacts.Utils.MIME;

namespace You_Contacts.Card
{
    public class Item
    {
        private string m_Name = "";
        private string m_Parameters = "";
        private string m_Value = "";


        internal Item(string name, string parameters, string value)
        {
            m_Name = name;
            m_Parameters = parameters;
            m_Value = value;
        }


        #region method SetDecodedValue


        public void SetDecodedValue(string value)
        {

            if (NeedEncode(value))
            {
           
                string newParmString = "";
                string[] parameters = m_Parameters.ToLower().Split(';');
                foreach (string parameter in parameters)
                {
                    string[] name_value = parameter.Split('=');
                    if (name_value[0] == "encoding" || name_value[0] == "charset")
                    {
                    }
                    else if (parameter.Length > 0)
                    {
                        newParmString += parameter + ";";
                    }
                }

                newParmString += "ENCODING=b;CHARSET=utf-8";

                this.ParametersString = newParmString;
                this.Value = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value));
            }
            else
            {
                this.Value = value;
            }
        }

        #endregion


        #region internal method ToItemString

        internal string ToItemString()
        {
            if (m_Parameters.Length > 0)
            {
                return m_Name + ";" + m_Parameters + ":" + FoldData(m_Value);
            }
            else
            {
                return m_Name + ":" + FoldData(m_Value);
            }
        }

        #endregion


        #region method NeedEncode

        private bool NeedEncode(string value)
        {

            if (!Net.IsAscii(value))
            {
                return true;
            }

            foreach (char c in value)
            {
                if (!(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion



        #region static method FoldData

        private string FoldData(string data)
        {

            if (data.Length > 76)
            {
                int startPosition = 0;
                int lastPossibleFoldPos = -1;
                StringBuilder retVal = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    char c = data[i];
 
                    if (c == ' ' || c == '\t')
                    {
                        lastPossibleFoldPos = i;
                    }


                    if (i == (data.Length - 1))
                    {
                        retVal.Append(data.Substring(startPosition));
                    }

                    else if ((i - startPosition) >= 76)
                    {
                        if (lastPossibleFoldPos == -1)
                        {
                            lastPossibleFoldPos = i;
                        }

                        retVal.Append(data.Substring(startPosition, lastPossibleFoldPos - startPosition) + "\r\n\t");

                        i = lastPossibleFoldPos;
                        lastPossibleFoldPos = -1;
                        startPosition = i;
                    }
                }

                return retVal.ToString();
            }
            else
            {
                return data;
            }
        }

        #endregion


        #region Properties Implementation


        public string Name
        {
            get { return m_Name; }
        }


        public string ParametersString
        {
            get { return m_Parameters; }

            set { m_Parameters = value; }
        }


        public string Value
        {
            get { return m_Value; }

            set { m_Value = value; }
        }


        public string DecodedValue
        {


            get
            {
                string data = m_Value;
                string encoding = null;
                string charset = null;
                string[] parameters = m_Parameters.ToLower().Split(';');
                foreach (string parameter in parameters)
                {
                    string[] name_value = parameter.Split('=');
                    if (name_value[0] == "encoding" && name_value.Length > 1)
                    {
                        encoding = name_value[1];
                    }
                    else if (name_value[0] == "charset" && name_value.Length > 1)
                    {
                        charset = name_value[1];
                    }
                }


                if (encoding != null)
                {
                    if (encoding == "quoted-printable")
                    {
                        data = System.Text.Encoding.Default.GetString(Mimes.QuotedPrintableDecode(System.Text.Encoding.Default.GetBytes(data)));
                    }
                    else if (encoding == "b")
                    {
                        data = System.Text.Encoding.Default.GetString(Net.FromBase64(System.Text.Encoding.Default.GetBytes(data)));
                    }
                    else
                    {
                        throw new Exception("Unknown data encoding '" + encoding + "' !");
                    }
                }

                if (charset != null)
                {
                    data = System.Text.Encoding.GetEncoding(charset).GetString(System.Text.Encoding.Default.GetBytes(data));
                }

                return data;
            }
        }

        #endregion

    }
}
