using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using YouInteractV1.Themes;


namespace YouInteractV1.LoaderData
{
    public static class ManageStructs
    {
        public static Theme GetActiveTheme()
        {
            var doc = XDocument.Load(Loader.CurrentXmlFile);
            var returnData = new Theme();
            if (doc.Root == null) return null;
            var xElement = doc.Root.Element("Themes");
            if (xElement == null) return null;
            foreach (var el in xElement.Elements().Where(el => (bool)el.Element("Active")))
            {
                returnData.Name = (string)el.Element("Theme_name");
                returnData.Font = (string)el.Element("Theme_font");
                returnData.Id = (int)el.Element("Theme_id");
                return returnData;
            }
            return null;
        }

        public static List<string> GetActiveApps()
        {

            var visibleApps = new List<string>();

            var reader = new XmlTextReader(AppDomain.CurrentDomain.BaseDirectory+"/XMLAccess/configsBD.xml");

            var flagApps = false;
            var flagActive = false;
            var flagAdd = false;
            var vemTextoApps = false;
         
            var tmp = "";

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "Apps")
                        {
                            flagApps = true;
                        }
                        else if (reader.Name == "Active")
                        {
                            flagActive = true;
                        }

                        else if (reader.Name == "Apps_name")
                        {
                            vemTextoApps = true;
                        }
                        break;
                    case XmlNodeType.Text:
                        var texto = reader.Value.ToString();
                        //MessageBox.Show(texto);
                        if (flagActive && texto == "True")//ACRESCENTEI FLAGACTIVE
                        {

                            flagAdd = true;

                        }
                        else if (flagActive && texto == "False")
                        {
                            flagAdd = false;
                            tmp = "";
                        }
                        if (vemTextoApps)
                        {
                            tmp = texto;

                        }
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name == "Apps_name")
                        {
                            vemTextoApps = false;
                        }
                        if (reader.Name == "Apps")
                        {
                            flagApps = false;
                        }
                        if (reader.Name == "Active")
                        {
                            if (flagAdd == true)
                            {
                                if (tmp != "")
                                {
                                    visibleApps.Add(tmp);
                                }
                            }
                            flagActive = false;
                            flagAdd = false;
                            tmp = "";
                        }
                        break;
                }
            }
            return visibleApps;
        }
    }
}
