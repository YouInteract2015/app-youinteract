using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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
        private static string _currentXmlFilePath = AppDomain.CurrentDomain.BaseDirectory + "/XMLAccess/configsBD.xml";

        private static string _currentAppXmlFilePrefix = "APPXML_";

        public static string CurrentXmlFile
        {
            get { return _currentXmlFilePath; }
            set { _currentXmlFilePath = value; }
        }

        public static string CurrentAppXmlFilePrefix
        {
            get { return _currentAppXmlFilePrefix; }
            set { _currentAppXmlFilePrefix = value; }
        }

        /// <summary>
        ///     Loads configurations from the database writing them in a local <code>configsBD.xml</code> file.
        /// </summary>
        /// <returns>
        ///     <value>true</value>
        ///     if it was successful
        /// </returns>
        public static bool IsUsingDefaults()
        {
            return !_currentAppXmlFilePrefix.Equals("APPXML_");
        }

        public static bool Load()
        {
            Console.WriteLine("CHEGUEI AQUI");

            Console.WriteLine("(LOADER) Checking if XMLAccess folder exists...");
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/XMLAccess"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/XMLAccess");

            Console.WriteLine("(LOADER) Creating/Writing Default files...");

            #region Defaults

            string lines =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<KinectApp>\r\n\t<Themes>\r\n\t\t<Entry>\r\n\t\t\t<Theme_id>1</Theme_id>\r\n\t\t\t<Theme_name>Theme1</Theme_name>\r\n\t\t\t<Active>True</Active>\r\n\t\t\t<Theme_font>N/A</Theme_font>\r\n\t\t</Entry>\r\n\t\t<Entry>\r\n\t\t\t<Theme_id>2</Theme_id>\r\n\t\t\t<Theme_name>ThemeTest</Theme_name>\r\n\t\t\t<Active>False</Active>\r\n\t\t\t<Theme_font>Calibri</Theme_font>\r\n\t\t</Entry>\r\n\t</Themes>\r\n\t<Important_dates>\r\n\t\t<Entry>\r\n\t\t\t<Id_usr>001</Id_usr>\r\n\t\t\t<Idate>12/03/2014 00:00:00</Idate>\r\n\t\t\t<About>dfghusfgwgergqergi</About>\r\n\t\t</Entry>\r\n\t\t<Entry>\r\n\t\t\t<Id_usr>001</Id_usr>\r\n\t\t\t<Idate>12/03/2014 00:00:00</Idate>\r\n\t\t\t<About>dfgwgergqergi</About>\r\n\t\t</Entry>\r\n\t</Important_dates>\r\n\t<Apps>\r\n\t\t<Entry>\r\n\t\t\t<Apps_name>You_Help</Apps_name>\r\n\t\t\t<App_id>5</App_id>\r\n\t\t\t<App_path>teste/path</App_path>\r\n\t\t\t<Times_played>0</Times_played>\r\n\t\t\t<Highscore>0</Highscore>\r\n\t\t\t<Active>False</Active>\r\n\t\t\t<Ordernumber>1</Ordernumber>\r\n\t\t\t<Dll>You_Help</Dll>\r\n\t\t</Entry>\r\n\t\t<Entry>\r\n\t\t\t<Apps_name>You_Timetables</Apps_name>\r\n\t\t\t<App_id>3</App_id>\r\n\t\t\t<App_path>xxx</App_path>\r\n\t\t\t<Times_played>0</Times_played>\r\n\t\t\t<Highscore>0</Highscore>\r\n\t\t\t<Active>False</Active>\r\n\t\t\t<Ordernumber>2</Ordernumber>\r\n\t\t\t<Dll>You_Timetables</Dll>\r\n\t\t</Entry>\r\n\t\t<Entry>\r\n\t\t\t<Apps_name>You_Videos</Apps_name>\r\n\t\t\t<App_id>2</App_id>\r\n\t\t\t<App_path>gpopogrrgp</App_path>\r\n\t\t\t<Times_played>0</Times_played>\r\n\t\t\t<Highscore />\r\n\t\t\t<Active>True</Active>\r\n\t\t\t<Ordernumber>3</Ordernumber>\r\n\t\t\t<Dll>You_Videos</Dll>\r\n\t\t</Entry>\r\n\t\t<Entry>\r\n\t\t\t<Apps_name>You_Pong</Apps_name>\r\n\t\t\t<App_id>1</App_id>\r\n\t\t\t<App_path />\r\n\t\t\t<Times_played>0</Times_played>\r\n\t\t\t<Highscore />\r\n\t\t\t<Active>False</Active>\r\n\t\t\t<Ordernumber>4</Ordernumber>\r\n\t\t\t<Dll>You_Pong</Dll>\r\n\t\t</Entry>\r\n\t\t<Entry>\r\n\t\t\t<Apps_name>You_Contacts</Apps_name>\r\n\t\t\t<App_id>4</App_id>\r\n\t\t\t<App_path>xxx</App_path>\r\n\t\t\t<Times_played>0</Times_played>\r\n\t\t\t<Highscore>0</Highscore>\r\n\t\t\t<Active>False</Active>\r\n\t\t\t<Ordernumber>5</Ordernumber>\r\n\t\t\t<Dll>You_Contacts</Dll>\r\n\t\t</Entry>\r\n\t</Apps>\r\n</KinectApp>";
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/XMLAccess/configsBDDefault.xml", lines);
            lines =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<You_videos>\r\n\t<Entry>\r\n\t\t<Id_video>14</Id_video>\r\n\t\t<Name_video>CAMBADA  @ RoboCup 2007- Impossible Goal.mp4</Name_video>\r\n\t\t<Path_video>teste/path</Path_video>\r\n\t\t<Active_video>False</Active_video>\r\n\t\t<Description_video>CAMBADA scores an amazing goal against Eigen team, from Japan, in RoboCup2007, Atlanta, USA.</Description_video>\r\n\t</Entry>\r\n\t<Entry>\r\n\t\t<Id_video>15</Id_video>\r\n\t\t<Name_video>CAMBADA @RoboCup2010- amazing pass with a goal.mp4</Name_video>\r\n\t\t<Path_video>teste/path</Path_video>\r\n\t\t<Active_video>True</Active_video>\r\n\t\t<Description_video>The first pass followed by a goal during free play of the RoboCup Middle Size League, scored by the CAMBADA team. It was in the 3rd vs 4th game against MRL at RoboCup 2010 in Singapore.</Description_video>\r\n\t</Entry>\r\n\t<Entry>\r\n\t\t<Id_video>16</Id_video>\r\n\t\t<Name_video>MicroRato13.mp4</Name_video>\r\n\t\t<Path_video>teste/path</Path_video>\r\n\t\t<Active_video>True</Active_video>\r\n\t\t<Description_video>V\u00EDdeo Promocional da 18\u00AA Edi\u00E7\u00E3o do Concurso MicroRato.</Description_video>\r\n\t</Entry>\r\n\t<Entry>\r\n\t\t<Id_video>17</Id_video>\r\n\t\t<Name_video>YouInteract.avi</Name_video>\r\n\t\t<Path_video>teste/path</Path_video>\r\n\t\t<Active_video>True</Active_video>\r\n\t\t<Description_video>Video elaborado no \u00E2mbito do Projecto DETI Interact da UC de Projecto em Engenharia Inform\u00E1tica (Deliverable 1) do curso de Engenharia de Computadores e Telem\u00E1tica da Universidade de Aveiro.</Description_video>\r\n\t</Entry>\r\n\t<Entry>\r\n\t\t<Id_video>19</Id_video>\r\n\t\t<Name_video>CAMBADA MSL Qualification Video - RoboCup 2014.avi</Name_video>\r\n\t\t<Path_video>test/path</Path_video>\r\n\t\t<Active_video>False</Active_video>\r\n\t\t<Description_video>This is the qualification video from team CAMBADA, from University of Aveiro, Portugal for RoboCup 2014 Middle Size League.</Description_video>\r\n\t</Entry>\r\n\t<Entry>\r\n\t\t<Id_video>18</Id_video>\r\n\t\t<Name_video>enei2014.avi</Name_video>\r\n\t\t<Path_video>path</Path_video>\r\n\t\t<Active_video>True</Active_video>\r\n\t\t<Description_video>Apresenta\u00E7\u00E3o do ENEI 2014 aos alunos da Universidade de Aveiro</Description_video>\r\n\t\t<Ordernumber>5</Ordernumber>\r\n\t</Entry>\r\n</You_videos>";
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/XMLAccess/APPXML_Default_You_videos.xml", lines);
            lines = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<You_contacts>\r\n\t<!--Empty-->\r\n</You_contacts>";
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/XMLAccess/APPXML_Default_You_contacts.xml", lines);
            lines =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<You_pong>\r\n\t<Entry>\r\n\t\t<Num_players>2</Num_players>\r\n\t\t<Player1_color>red</Player1_color>\r\n\t\t<Player2_color>blue</Player2_color>\r\n\t</Entry>\r\n</You_pong>";
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/XMLAccess/APPXML_Default_You_pong.xml", lines);

            #endregion

            Console.WriteLine("(LOADER) Getting the updates...");
            // Commented for debug!!! Final Version please remove comment (cpatricio - 27/02/2015)
            //List<List<string>> x = GetUpdates();
            List<List<string>> x = null;
            Console.WriteLine("(LOADER) Updating...");
            return x != null ? Update(x[0], x[1]) : Update(null, null);
        }

        private static List<List<string>> GetUpdates()
        {
            var altered = new List<string>();
            var apps = new List<string>();
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
                    if (reader.GetValue(0).ToString().Equals("themes") ||
                        reader.GetValue(0).ToString().Equals("important_dates") ||
                        reader.GetValue(0).ToString().Equals("apps"))
                        altered.Add(reader.GetValue(0).ToString());
                    else
                    {
                        apps.Add(reader.GetValue(0).ToString());
                    }
                    Console.WriteLine("\t(LOADER) Table " + reader.GetValue(0) + " will be written.");
                }

                reader.Close();
                Connection.Conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("\t(LOADER) ERROR! Tried to connect or read from altered_tables: " + e.Message);
                Connection.Conn.Close();
                return null;
            }
            //altered tem o que esta na tabela dos updates
            //devolve á função update os updates que tem de fazer
            return new List<List<string>> { altered, apps };
        }

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
            var settings = new XmlWriterSettings { Indent = true, IndentChars = "\t", NewLineOnAttributes = true };

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

        public static string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}