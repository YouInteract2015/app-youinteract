//using You_Contacts.DSDWS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace You_Contacts
{
    class DownloadContatcs
    {
        //private DSDProvider DSD = new DSDProvider();
        public static List<Docente> docentes;
        public static List<TeacherItem> teacherList;

        string path = Directory.GetCurrentDirectory() + "\\Contacts\\";

        
        

        public void download()
        {
            // Clean Folder:
            /*DirectoryInfo directory = new DirectoryInfo(path);
            foreach (FileInfo file in directory.GetFiles())
                file.Delete();

            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
                subDirectory.Delete();*/

            // Get the teacher list from DSD Content Provider
            docentes = DSD.GetDocentes(DSD.IDDept);

            teacherList = new List<TeacherItem>();

            if (docentes != null)
            {
                //backupToXML();
                foreach (Docente d in docentes)
                {
                    // Download Image:
                    /*try
                    {
                        downloadImageToSystem(String.Format("{0}{1}", DSDProvider.DSDFilePath, d.Foto), d.Pessoa.Nome);
                    }
                    catch (Exception)
                    {
                        downloadImageToSystem("http://images.tribe.net/tribe/upload/photo/420/b3e/420b3e40-0942-4cd8-8692-dedd987d308f", d.Pessoa.Nome);
                        //MessageBox.Show("Error downloading photo: " + String.Format("{0}{1}", DSDProvider.DSDFilePath, d.Foto));
                    }*/

                    TeacherItem ti = new TeacherItem(d.Pessoa.Nome, path + d.Pessoa.Nome.Replace(" ", "") + ".jpg", d.Gabinete, d.Extensao, d.Webpage);
                    /*
                    Console.WriteLine("Nome: " + d.Pessoa.Nome +
                                      "\nFoto: " + path + d.Pessoa.Nome +
                                      "\nGabinete: " + d.Gabinete +
                                      "\nTelefone: " + d.Extensao +
                                      "\nSite: " + d.Webpage);*/
                    teacherList.Add(ti);
                }
            }
            else
            {
                //Add an item with a warning for bad connection
                /*TeacherItem ti = new TeacherItem("Erro ao carregar docentes!", DSDProvider.DSDFilePath, "", "", "");
                teacherList.Add(ti);*/

                readFromXML();
            }
        }


        
        private void readFromXML()
        {
            string path = Directory.GetCurrentDirectory() + "\\Contacts\\";
            MyContatcs d = new MyContatcs();
            XmlTextReader reader = new XmlTextReader("Contacts.xml");

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element) 
                {
                    if (reader.Name.Equals("Contact"))
                    {
                        XElement el = XNode.ReadFrom(reader) as XElement;
                        if (el != null)
                        {
                            IEnumerable<XNode> contactItems = el.Nodes();
                            Console.WriteLine(contactItems.Count());
                            for (int i = 0; i < contactItems.Count(); i++)
                            {
                                if (contactItems.ElementAt(i).NodeType == XmlNodeType.Element)
                                {
                                    XElement node = (XElement)contactItems.ElementAt(i);
                                    switch (node.Name.ToString())
                                    {
                                        case "FirstName":
                                            Console.WriteLine(d.FirstName);
                                            d.FirstName = node.Value.ToString();
                                            break;

                                        case "LastName":
                                            Console.WriteLine(d.LastName);
                                            d.LastName = node.Value.ToString();
                                            break;


                                        case "Street":
                                            Console.WriteLine(d.Street);
                                            d.Street = node.Value.ToString();
                                            break;


                                        case "PostOfficeAddress":
                                            Console.WriteLine(d.PostOfficeAddress);
                                            d.PostOfficeAddress = node.Value.ToString();
                                            break;


                                        case "City":
                                            Console.WriteLine(d.City);
                                            d.City = node.Value.ToString();
                                            break;


                                        case "State":
                                            Console.WriteLine(d.State);
                                            d.State = node.Value.ToString();
                                            break;

                                        case "PostalCode":
                                            Console.WriteLine(d.PostalCode);
                                            d.PostalCode = node.Value.ToString();
                                            break;


                                        case "Country":
                                            Console.WriteLine(d.Country);
                                            d.Country = node.Value.ToString();
                                            break;


                                        case "Email":
                                            Console.WriteLine(d.email);
                                            d.email = node.Value.ToString();
                                            break;


                                        case "Number":
                                            Console.WriteLine(d.number);
                                            d.Number = node.Value.ToString();
                                            break;


                                    }
                                }
                            }

                            ContactItem ti = new ContactItem(d.FirstName, d.LastName, d.Street, d.PostOfficeAddress, d.City, d.State, d.PostalCode, d.PostalCode, d.Country, d.email, d.Number);
                            contactList.Add(ti);
                        }
                    }
                }

                           
                        
                   
            }

        }

        private void backupToXML()
        {
            //Settings para escrever no Xml
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.NewLineOnAttributes = true;

            //Cria-se um XML com as settings anteriormente criadas
            using (XmlWriter writer = XmlWriter.Create("Contacts.xml", settings))
            {
                

                    try
                    {
                        //docentes = DSD.GetDocentes(DSD.IDDept);
                        contactList = new List<ContactItem>();

                        //inicia-se a escrita
                        writer.WriteStartDocument();
                        writer.WriteStartElement("ContactsAPP");


                        foreach (Docente d in docentes)
                        {
                            ContatctItem ti = new ContactItem(d.FirstName, d.LastName, d.Street, d.PostOfficeAddress, d.City, d.State, d.PostalCode, d.PostalCode, d.Country, d.email, d.Number);
                          
                            teacherList.Add(ti);
                            writer.WriteStartElement("Contact");
                            
                            writer.WriteElementString("FirstName", d.FirstName);
                            writer.WriteElementString("LastName", d.LastName);
                            writer.WriteElementString("Street", d.Street);
                            writer.WriteElementString("PostOfficeAddress", d.PostOfficeAddress);
                            writer.WriteElementString("City", d.City);
                            writer.WriteElementString("State", d.State);
                            writer.WriteElementString("PostalCode", d.PostalCode);
                            writer.WriteElementString("Country", d.Country);
                            writer.WriteElementString("Email", d.Email);
                            writer.WriteElementString("Number", d.Number);
                            writer.WriteEndElement();
                 
                        }
                            writer.WriteEndElement();
                            writer.WriteEndDocument();
                    }
                    catch
                    {
                        
                    }

                //writer.WriteEndElement();
                //writer.WriteEndDocument();
                //writer.Close();
           
                }
                
            }
   }

}


public class MyContact
{
    public String FirstName, LastName, Street, PostOfficeAddress, City, State, PostalCode, PostalCode, Country, Email, Number;
                          
}

