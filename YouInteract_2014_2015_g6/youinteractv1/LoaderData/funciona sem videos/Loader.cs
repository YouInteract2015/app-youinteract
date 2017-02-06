using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace YouInteractV1.LoaderData
{
    public static class Loader
    {
        /// <summary>
        ///     Loads configurations from the database writing them in a local <code>configsBD.xml</code> file.
        /// </summary>
        /// <returns>
        ///     <value>true</value>
        ///     if it was successful
        /// </returns>
        private static string _currentXmlFile = Directory.GetCurrentDirectory() + "/XMLAccess/configsBD.xml";

        /// <summary>
        ///     Loads configurations from the database writing them in a local <code>configsBD.xml</code> file.
        /// </summary>
        /// <returns>
        ///     <value>true</value>
        ///     if it was successful
        /// </returns>
        public static string CurrentXmlFile
        {
            get { return _currentXmlFile; }
            set { _currentXmlFile = value; }
        }

        public static bool Load()
        {
            if(!Directory.Exists(Directory.GetCurrentDirectory() + "/XMLAccess"))
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/XMLAccess");
            var res = Update(GetUpdates());
            var res2 = CreateAppXML();

            return res && res2;
        }

        private static bool CreateAppXML()
        {
            Console.WriteLine("(LOADER) Starting to create apps' XML files...");
            XDocument document;

            try
            {
                document = XDocument.Load(_currentXmlFile);
            }
            catch (Exception)
            {
                Console.WriteLine("(LOADER) ERROR! INTERNAL ERROR OCCURRED!");
                return false;
            }

            if (document.Root == null)
            {
                Console.WriteLine("(LOADER) ERROR! INTERNAL ERROR OCCURRED!");
                return false;
            }

            var tables = document.Root.Elements();
            foreach (var table in tables)
            {
                Console.WriteLine("(LOADER) Checking if table is from an app...");
                if (table.Name.ToString().Equals("Themes") || table.Name.ToString().Equals("Important_dates") ||
                    table.Name.ToString().Equals("Users") || table.Name.ToString().Equals("Apps"))
                {
                    Console.WriteLine("(LOADER) Table " + table.Name + " is not an app");
                    continue;
                }
                Console.WriteLine("(LOADER) Table " + table.Name + " is an app");
                var settings = new XmlWriterSettings { Indent = true, IndentChars = "\t", NewLineOnAttributes = true };
                Console.WriteLine("(LOADER) Creating the table xml: " + "/XMLAccess/APPXML_" + table.Name + ".xml...");
                using (XmlWriter writer = XmlWriter.Create(Directory.GetCurrentDirectory() + "/XMLAccess/APPXML_" + table.Name + ".xml", settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement(Capitalize(table.Name.ToString()));

                    var entriesL = table.Elements();
                    var entries = entriesL as XElement[] ?? entriesL.ToArray();
                    if (!entries.Any())
                        writer.WriteComment("Empty");
                    else
                    {
                        foreach (var entry in entries)
                        {
                            writer.WriteStartElement("Entry");

                            var fieldsL = entry.Elements();
                            var fields = fieldsL as XElement[] ?? fieldsL.ToArray();
                            foreach (var field in fields)
                            {
                                writer.WriteElementString(Capitalize(field.Name.ToString()),
                                    (string) field);
                            }
                            writer.WriteEndElement();
                        }
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();
                }
                Console.WriteLine("(LOADER) Done!");
            }
            Console.WriteLine("(LOADER) Done!");
            return true;
        }

        private static ArrayList GetUpdates()
        {
            Console.WriteLine("(LOADER) Starting to get the updates...");
            var altered = new ArrayList();
            try
            {
                Connection.Conn.Open();
                SqlCommand command = Connection.Conn.CreateCommand();
                command.CommandType = CommandType.Text;
                // vai buscar a tabela dos alterados
                command.CommandText = "SELECT * FROM [deti- youinteract].dbo.altered_tables;";
                SqlDataReader reader = command.ExecuteReader();
                //se não tem updates retorna Null
                if (!reader.HasRows)
                {
                    reader.Close();
                    Connection.Conn.Close();
                    return null;
                }
                //se tem vai buscar o primeiro atributo de cada para  o array list
                while (reader.Read())
                {
                    if (reader.GetValue(0).ToString().Equals("users"))
                        continue;
                    altered.Add(reader.GetValue(0).ToString());
                    Console.WriteLine("(LOADER) Table " + reader.GetValue(0) + " exists");
                }

                reader.Close();
                Connection.Conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("(LOADER) ERROR! Tried to connect or read from altered_tables: " + e.Message);
                Connection.Conn.Close();
                return null;
            }
            //altered tem o que esta na tabela dos updates
            //devolve á função update os updates que tem de fazer
            Console.WriteLine("(LOADER) Done!");
            return altered;
        }

        private static bool Update(IList updates)
        {
            Console.WriteLine("(LOADER) Started to access the updates...");
            //Se a getUpdates não tem valores retorna falso
            if (updates == null || updates.Count == 0)
            {
                try
                {
                    XDocument.Load(_currentXmlFile);
                    Console.WriteLine("(LOADER) ERROR! Loaded previously existing xml file! Using " + _currentXmlFile + " as the base XML!");
                }
                catch (Exception)
                {
                    _currentXmlFile = Directory.GetCurrentDirectory() + "/XMLAccess/configsBDDefault.xml";
                    Console.WriteLine("(LOADER) ERROR! Main xml file could not be loaded! Using " + _currentXmlFile + " as the base XML!");
                }
                return false;
            }

            //Numero de updates do array
            int count = updates.Count - 1;

            //Settings para escrever no Xml
            var settings = new XmlWriterSettings { Indent = true, IndentChars = "\t", NewLineOnAttributes = true };

            var returnValue = true;
            Console.WriteLine("(LOADER) Creating " + _currentXmlFile + " file...");
            //Cria-se um XML com as settings anteriormente criadas
            using (XmlWriter writer = XmlWriter.Create(_currentXmlFile, settings))
            {
                //inicia-se a escrita
                writer.WriteStartDocument();
                writer.WriteStartElement("KinectApp");
                //while que percorre todos os updates
                Console.WriteLine("(LOADER) Done!");
                Console.WriteLine("(LOADER) Getting data from " + updates.Count + " tables...");
                while (count >= 0)
                {
                    try
                    {
                        Connection.Conn.Open();
                        SqlCommand command = Connection.Conn.CreateCommand();
                        command.CommandType = CommandType.Text;
                        //updates[count] tem por exemplo "pong" e então vamos la buscar
                        command.CommandText = "SELECT * FROM [deti- youinteract].dbo." + updates[count];
                        if (updates[count].Equals("apps") || updates[count].Equals("videos"))
                        {
                            command.CommandText += " ORDER BY ordernumber";
                        }
                        SqlDataReader reader = command.ExecuteReader();

                        //Escrever o que irá sofrer updatas no inicio do xml
                        writer.WriteStartElement(Capitalize(updates[count].ToString()));

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
                        _currentXmlFile = Directory.GetCurrentDirectory() + "/XMLAccess/configsBDDefault.xml";
                        Console.WriteLine("(LOADER) ERROR! Could not load table " + updates[count] + " because: " + e.Message);
                        Console.WriteLine("(LOADER) ERROR! Using " + _currentXmlFile + " as the base XML!");
                        Connection.Conn.Close();
                        returnValue = false;
                    }
                    //já se processou um update
                    count--;
                }
                Console.WriteLine("(LOADER) Done!");
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }

            return returnValue;
        }

        private static string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
