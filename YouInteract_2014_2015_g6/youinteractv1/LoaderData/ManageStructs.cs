using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using YouInteractV1.Themes;

using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using System.Net;
using System.Net.NetworkInformation;
using System.Text;

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
                if ((string)el.Element("Theme_file") == "") returnData.Background = "background.jpg";
                else returnData.Background = (string)el.Element("Theme_file");

                return returnData;
            }
            return null;
        }

        public static List<string> GetActiveApps()
        {

            var visibleApps = new List<string>();

            var reader = new XmlTextReader(Loader.CurrentXmlFile);

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

        public static List<schedulers> GetActiveSchedulers()
        {
            List<schedulers> visibleSchedulers = new List<schedulers>();

            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YouInteract/XMLAccess/configsBD.xml"));
            }
            catch (XmlException e)
            {
                Console.WriteLine("Error Reading Scheduler configs");
            }
            XmlNodeList schedulersNode = xml.SelectNodes("//Scheduler");
            foreach (XmlNode scheduler in schedulersNode)
            {
                schedulers aux = new schedulers();

                aux.id = int.Parse((scheduler.SelectSingleNode("id") as XmlElement).InnerText);
                aux.startAt = (scheduler.SelectSingleNode("startAt") as XmlElement).InnerText;
                aux.endAt = (scheduler.SelectSingleNode("endAt") as XmlElement).InnerText;
                aux.monday = int.Parse((scheduler.SelectSingleNode("monday") as XmlElement).InnerText);
                aux.tuesday = int.Parse((scheduler.SelectSingleNode("tuesday") as XmlElement).InnerText);
                aux.wednesday = int.Parse((scheduler.SelectSingleNode("wednesday") as XmlElement).InnerText);
                aux.thursday = int.Parse((scheduler.SelectSingleNode("thursday") as XmlElement).InnerText);
                aux.friday = int.Parse((scheduler.SelectSingleNode("friday") as XmlElement).InnerText);
                aux.saturday = int.Parse((scheduler.SelectSingleNode("saturday") as XmlElement).InnerText);
                aux.sunday = int.Parse((scheduler.SelectSingleNode("sunday") as XmlElement).InnerText);
                aux.startTime = (scheduler.SelectSingleNode("startTime") as XmlElement).InnerText;
                aux.endTime = (scheduler.SelectSingleNode("endTime") as XmlElement).InnerText;
                aux.path = (scheduler.SelectSingleNode("path") as XmlElement).InnerText;
                aux.type = (scheduler.SelectSingleNode("type") as XmlElement).InnerText;
                aux.active = (scheduler.SelectSingleNode("Active") as XmlElement).InnerText;

                visibleSchedulers.Add(aux);
            }

            return visibleSchedulers;
        }
    }
}
