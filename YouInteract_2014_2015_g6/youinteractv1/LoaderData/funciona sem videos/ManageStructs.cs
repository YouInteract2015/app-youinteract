using System;
using System.Collections.Generic;
using System.Xml;


namespace YouInteractV1.LoaderData
{
    public static class ManageStructs
    {
        private static int countApps = 0;

        /*
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
        }*/

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


        /*public static String[] sendBackString(String request)
        {

            // visibleApps = new string[] { "main", "timeTables", "contacts", "pong", "calendario", "videos" };
            string[] visibleApps = new string[20];
            XmlTextReader reader = new XmlTextReader(YouLoader.CurrentXmlFile);
            int flag = 0;
            Boolean flagActive = false;
            Boolean flagAdd = false;
            string tmp = "";
            int vemTextoApps = 0;
            string[] videosConfig = new string[100];
            int guarda = 0;
            int lerOuNao = 0;
            int vervisivel = 0;
            string solo = "";
            int cont = 0;
            
            if (request == "videos")
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name == "Videos")
                            {
                                flag = 1;
                            }
                            else if (reader.Name == "Id_video")
                            {
                                vemTextoApps = 1;
                            }
                            else if (reader.Name == "Name_video")
                            {
                                vemTextoApps = 1;
                            }
                            else if (reader.Name == "Path_video")
                            {
                                vemTextoApps = 1;
                            }
                            else if (reader.Name == "Active_video")
                            {
                                vervisivel = 1;
                                vemTextoApps = 1;
                            }
                            else if (reader.Name == "Description_video")
                            {
                                vemTextoApps = 1;
                            }
                            break;
                        case XmlNodeType.Text:
                            if (flag == 1 && vemTextoApps == 1)
                            {
                                //     visibleApps[cont] = reader.Value.ToString();
                                //     cont++;
                                if (vervisivel == 1)
                                {
                                    if (reader.Value.ToString() == "True")
                                    {
                                        lerOuNao = 1;
                                    }
                                    else if (reader.Value.ToString() == "False")
                                    {
                                        lerOuNao = 0;
                                    }
                                }
                                if (guarda == 0)
                                {
                                    solo = solo + reader.Value.ToString() + "#";
                                }
                                else if (guarda == 1)
                                {
                                    if (lerOuNao == 1)
                                    {
                                        videosConfig[cont] = solo;
                                        cont++;
                                    }
                                    solo = "";
                                    guarda = 0;
                                    solo = solo + reader.Value.ToString() + "#";
                                }
                            }
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            //    if (reader.Name == "Videos")
                            //  {
                            //    flag = 0;
                            //}
                            if (reader.Name == "Description_video")
                            {
                                guarda = 1;
                            }
                            if (reader.Name == " Active_video")
                            {
                                vervisivel = 0;
                            }
                            break;
                        //<Entry>
                        //<Apps_name>pong</Apps_name>
                        //<App_id>1</App_id>
                        //<App_path />
                        //<Times_played>0</Times_played>
                        //<Highscore />
                        //<Active>True</Active>
                        //</Entry>
                    }
                }
                string[] copia = new string[cont];
                int i;
                for (i = 0; i < cont; i++)
                {
                    copia[i] = videosConfig[i];
                }
                return copia;
            }
            else
            {
                return videosConfig;
            }
        }*/

        public static int getCountApps()
        {
            return countApps;
        }
    }
}
