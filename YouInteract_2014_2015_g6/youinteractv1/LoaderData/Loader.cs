using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace YouInteractV1.LoaderData
{
    public static class Loader
    {
        /* Wi-Fi MAC address form this device */
        private static string mac_address;

        /* Path to XMLfiles */
        private static string _currentXmlFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YouInteract/XMLAccess/configsBD.xml");

        /**
         * Return Xmlfiles path
         */
        public static string CurrentXmlFile
        {
            get { return _currentXmlFilePath; }
            set { _currentXmlFilePath = value; }
        }
        
        /**
         * Load Xmlfile configuration from Portal
         * Load active apps xmlfiles configurations from Portal
         */
        public static bool Load()
        {
            /* Checks if local XmlFolder is created */
            checkXmlFolder();

            /* Get general XML */
            XmlDocument portalXml = portalConnection();
            if (portalXml != null)
            {
                Console.WriteLine("(LOADER) XML Received...");
                portalXml.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YouInteract/XMLAccess/configsBD.xml"));

                /* Get apps configuration xml's */
                if (getAppsConfigs(portalXml))
                    Console.WriteLine("(LOADER) Apps configs successfull retrieved...");
                else
                    Console.WriteLine("(LOADER) Can't retrieve all apps configs, using old definitions for those who were not received...");
            }
            else
            {
                Console.WriteLine("(LOADER) Using old XML file with last definitions...");
            }

            /* Terminou Load */
            return true;
        }

        /**
         * Checks if local XmlFolder is created, if not create it
         */
        private static void checkXmlFolder()
        {
            /* Checks if XMLAccess folder exists */
            Console.WriteLine("(LOADER) Checking if XMLAccess folder exists...");
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YouInteract/XMLAccess")))
            {
                Console.WriteLine("(LOADER) Creating directory and default config xml file...");
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YouInteract/XMLAccess"));

                #region Defaults

                string lines =
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<KinectApp>\r\n\t<Themes>\r\n\t\t<Entry>\r\n\t\t\t<Theme_id>1</Theme_id>\r\n\t\t\t<Theme_name>Default</Theme_name>\r\n\t\t\t<Active>True</Active>\r\n\t\t\t<Theme_font>N/A</Theme_font>\r\n\t\t</Entry>\r\n\t</Themes>\r\n\t<Important_dates>\r\n\t</Important_dates>\r\n\t<Apps>\r\n\t\t<Entry>\r\n\t\t\t<Apps_name>You_Tutorial</Apps_name>\r\n\t\t\t<App_id>5</App_id>\r\n\t\t\t<App_path>teste/path</App_path>\r\n\t\t\t<Times_played>0</Times_played>\r\n\t\t\t<Highscore>0</Highscore>\r\n\t\t\t<Active>False</Active>\r\n\t\t\t<Ordernumber>1</Ordernumber>\r\n\t\t\t<Dll>You_Help</Dll>\r\n\t\t</Entry>\r\n\t</Apps>\r\n</KinectApp>";
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YouInteract/XMLAccess/configsBD.xml"), lines);

                #endregion
            }
        }

        /**
         * Connect to Portal to get config .xml
         */
        private static XmlDocument portalConnection()
        {
            // Get Mac address from Wi-Fi adapter
            try
            {
                Console.WriteLine("(LOADER) Sending device Mac Address to Portal...");
                NetworkInterface[] netInt = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in netInt)
                {
                    if (adapter.NetworkInterfaceType.ToString().ToLower().Contains("wi-fi") || adapter.NetworkInterfaceType.ToString().ToLower().Contains("wifi") || adapter.NetworkInterfaceType.ToString().ToLower().Contains("wireless"))// only return MAC Address from Wi-Fi adapter
                    {
                        mac_address = adapter.GetPhysicalAddress().ToString();
                        Console.WriteLine("(LOADER) Wireless MAC Address: " + mac_address);
                    }
                }
            }
            catch (NetworkInformationException) { Console.WriteLine("(LOADER) Can't access Wi-Fi adapter MAC Address..."); return null; };

            // Connect to server and send mac address
            string[] lines = System.IO.File.ReadAllLines(@""+AppDomain.CurrentDomain.BaseDirectory+"/portalLink.txt");

            string url = lines[0] + "/device/kinect/get";
            XmlDocument xml = new XmlDocument();

            try
            {
                Console.WriteLine("(LOADER) Connect to Portal to transfer XML file...");
                var wb = new WebClient();

                var data = new NameValueCollection();
                data["mac"] = mac_address;
                xml.LoadXml(System.Text.Encoding.UTF8.GetString(wb.UploadValues(url, "POST", data)));
            }
            catch (System.Net.WebException) { Console.WriteLine("(LOADER) Can't connect to YouInteract Portal..."); return null; };
            
            return xml;
        }

        /*
         * Checks number os apps present in XML file
         * For each app try to receive his config xml file
         */
        private static bool getAppsConfigs(XmlDocument portalXml)
        {
            bool all_apps = true;

            XmlNodeList apps = portalXml.SelectNodes("KinectApp/Apps/Entry[App_id!=0]");

            // fore each app try to retrieve his config file and save it
            foreach (XmlElement app in apps)
            {
                string name = app.ChildNodes[0].FirstChild.Value;
                string id = app.ChildNodes[1].FirstChild.Value;

                string[] lines = System.IO.File.ReadAllLines(@"" + AppDomain.CurrentDomain.BaseDirectory + "/portalLink.txt");
                string url = lines[0] + "/device/kinect/app/get";

                var wb_ip = new WebClient();

                var data_ip = new NameValueCollection();

                data_ip["mac"] = mac_address;
                data_ip["app"] = id;

                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(System.Text.Encoding.UTF8.GetString(wb_ip.UploadValues(url, "POST", data_ip)));
                    xml.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YouInteract/XMLAccess/"+name+".xml"));
                }
                catch (System.Net.WebException) { Console.WriteLine("(LOADER) Can't retrieve " + name + " config xml file..."); all_apps = false; };
            }

            return all_apps;
        }











        /*
            
            // Se nao existir o diretorio da merda ao compilar, por isso isto e inutil xD
            



            Console.WriteLine("(LOADER) Getting the updates...");
            // Commented for debug!!! Final Version please remove comment (cpatricio - 27/02/2015)
            //List<List<string>> x = GetUpdates();
            List<List<string>> x = null;
            Console.WriteLine("(LOADER) Updating...");
            return x != null ? Update(x[0], x[1]) : Update(null, null);

            
        }

        
        
         Fazer update conforme o .xml 
        private static bool Update(IList<string> updates, IList<string> apps)
        {
            //Se a getUpdates não tem valores retorna falso
            if (updates == null || updates.Count == 0)
            {
                try
                {
                    if (MainWindow.debug == true) Console.WriteLine("Config XML to Load: " + _currentXmlFilePath);
                    XDocument.Load(_currentXmlFilePath);
                    Console.WriteLine("\t(LOADER) ERROR! Loaded previously existing XML files!");
                }
                catch (Exception)
                {
                    _currentXmlFilePath = AppDomain.CurrentDomain.BaseDirectory + "/XMLAccess/configsBDDefault.xml";
                    _currentAppXmlFilePrefix = "APPXML_Default_";
                    Console.WriteLine(
                        "\t(LOADER) ERROR! Preiously existing XML files could not be loaded! Using Defaults.");
                }
                return false;
            }

            //Numero de updates do array
            int count = updates.Count - 1;

            //Settings para escrever no Xml
            var settings = new XmlWriterSettings {Indent = true, IndentChars = "\t", NewLineOnAttributes = true};

            bool returnValue = true;
            string[] temp = _currentXmlFilePath.Split('/');
            Console.WriteLine("\t(LOADER) Creating file: " + temp.Last() + "...");
            //Cria-se um XML com as settings anteriormente criadas
            XmlWriter writer = XmlWriter.Create(_currentXmlFilePath, settings);

            //inicia-se a escrita
            writer.WriteStartDocument();
            writer.WriteStartElement("KinectApp");

            //while que percorre todos os updates
            Console.WriteLine("\t(LOADER) Will write " + updates.Count + " system tables plus " + apps.Count +
                              " application tables.");
            Console.WriteLine("\t(LOADER) Getting data from " + (updates.Count + apps.Count) + " tables...");
            while (count >= 0)
            {
                try
                {
                    Connection.Conn.Open();
                    SqlCommand command = Connection.Conn.CreateCommand();
                    command.CommandType = CommandType.Text;
                    //updates[count] tem por exemplo "pong" e então vamos la buscar
                    command.CommandText = "SELECT * FROM [deti- youinteract].dbo." + updates[count];
                    if (updates[count].Equals("apps"))
                    {
                        command.CommandText += " ORDER BY ordernumber";
                    }
                    SqlDataReader reader = command.ExecuteReader();

                    //Escrever o que irá sofrer updatas no inicio do xml
                    writer.WriteStartElement(Capitalize(updates[count]));

                    //se não ler nada é porque está nos updates mas não tem nada
                    if (!reader.HasRows)
                    {
                        writer.WriteComment("Empty");
                    }
                    //enquando tiver linhas
                    while (reader.Read())
                    {
                        //escrever no xml
                        writer.WriteStartElement("Entry");
                        //percorre colunas
                        for (int colCount = 0; colCount < reader.FieldCount; colCount++)
                        {
                            if (reader.GetValue(colCount) != null && !reader.IsDBNull(colCount))
                            {
                                //ecrever o elemeto no xml
                                writer.WriteElementString(Capitalize(reader.GetName(colCount)),
                                    reader.GetValue(colCount).ToString().Trim()); //eliminar espaços
                            }
                        }
                        //elemento de fechar xml
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    //fechar ligações
                    reader.Close();
                    Connection.Conn.Close();
                }
                catch (Exception e)
                {
                    _currentXmlFilePath = AppDomain.CurrentDomain.BaseDirectory + "/XMLAccess/configsBDDefault.xml";
                    _currentAppXmlFilePrefix = "APPXML_Default_";
                    Console.WriteLine("(LOADER) ERROR! Could not load table " + updates[count] + " because: " +
                                      e.Message);
                    Console.WriteLine("(LOADER) ERROR! Using Defaults.");
                    Connection.Conn.Close();
                    returnValue = false;
                }
                //já se processou um update
                count--;
            }
            count = apps.Count - 1;
            while (count >= 0)
            {
                try
                {
                    Connection.Conn.Open();
                    SqlCommand command = Connection.Conn.CreateCommand();
                    command.CommandType = CommandType.Text;
                    //updates[count] tem por exemplo "pong" e então vamos la buscar
                    command.CommandText = "SELECT * FROM [deti- youinteract].dbo." + apps[count];
                    if (apps[count].Equals("You_videos"))
                    {
                        command.CommandText += " ORDER BY ordernumber";
                    }
                    SqlDataReader reader = command.ExecuteReader();

                    Console.WriteLine("\t(LOADER) Creating file: " + "APPXML_" + Capitalize(apps[count]) + ".xml...");
                    XmlWriter writer2 =
                        XmlWriter.Create(
                            AppDomain.CurrentDomain.BaseDirectory + "/XMLAccess/APPXML_" + Capitalize(apps[count]) +
                            ".xml", settings);
                    //Escrever o que irá sofrer updatas no inicio do xml
                    writer2.WriteStartDocument();
                    writer2.WriteStartElement(Capitalize(apps[count]));

                    //se não ler nada é porque está nos updates mas não tem nada
                    if (!reader.HasRows)
                    {
                        writer2.WriteComment("Empty");
                    }
                    //enquando tiver linhas
                    while (reader.Read())
                    {
                        //escrever no xml
                        writer2.WriteStartElement("Entry");
                        //percorre colunas
                        for (int colCount = 0; colCount < reader.FieldCount; colCount++)
                        {
                            if (reader.GetValue(colCount) != null && !reader.IsDBNull(colCount))
                            {
                                //ecrever o elemeto no xml
                                writer2.WriteElementString(Capitalize(reader.GetName(colCount)),
                                    reader.GetValue(colCount).ToString().Trim()); //eliminar espaços
                            }
                        }
                        //elemento de fechar xml
                        writer2.WriteEndElement();
                    }

                    writer2.WriteEndElement();
                    writer2.WriteEndDocument();
                    writer2.Close();
                    //fechar ligações
                    reader.Close();
                    Connection.Conn.Close();
                }
                catch (Exception e)
                {
                    _currentXmlFilePath = AppDomain.CurrentDomain.BaseDirectory + "/XMLAccess/configsBDDefault.xml";
                    _currentAppXmlFilePrefix = "APPXML_Default_";
                    Console.WriteLine("(LOADER) ERROR! Could not load table " + updates[count] + " because: " +
                                      e.Message);
                    Console.WriteLine("(LOADER) ERROR! Using Defaults.");
                    Connection.Conn.Close();
                    returnValue = false;
                }
                //já se processou um update
                count--;
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            return returnValue;
        }
            */
        /* Por em maiuscula, nem sei se vou usar 
        public static string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return char.ToUpper(s[0]) + s.Substring(1);
        }
            */
        /*
         * Verifiy if we have connection to the Portal.
         * If we have we send Mac address and return .xml.
         * If we dont have return null.
         */
       
    }
}