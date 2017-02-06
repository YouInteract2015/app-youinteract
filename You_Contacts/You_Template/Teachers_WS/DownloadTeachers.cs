using You_Contacts.DSDWS;
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

namespace You_Contacts.Teachers_WS
{
    class DownloadTeachers
    {
        private DSDProvider DSD = new DSDProvider();
        public static List<Docente> docentes;
        public static List<TeacherItem> teacherList;

        string path = AppDomain.CurrentDomain.BaseDirectory + "/App/You_Contacts/Teachers/";

        
        

        public void download()
        {
            // Clean Folder:
            /*DirectoryInfo directory = new DirectoryInfo(path);
            foreach (FileInfo file in directory.GetFiles())
                file.Delete();

            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
                subDirectory.Delete();*/

            // Get the teacher list from DSD Content Provider

            try
            {
                //docentes = DSD.GetDocentes(DSD.IDDept);
            }
            catch (Exception e) { Console.WriteLine("Can't load DETI contacts from DSD! Using local file!"); }

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
            string path = AppDomain.CurrentDomain.BaseDirectory + "/App/You_Contacts/Teachers/";
            MyDocente d = new MyDocente();
            XmlTextReader reader = new XmlTextReader(AppDomain.CurrentDomain.BaseDirectory + "/App/You_Contacts/Teachers.xml");

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element) 
                {
                    if (reader.Name.Equals("Teacher"))
                    {
                        XElement el = XNode.ReadFrom(reader) as XElement;
                        if (el != null)
                        {
                            IEnumerable<XNode> teacherItems = el.Nodes();
                            for (int i = 0; i < teacherItems.Count(); i++)
                            {
                                if (teacherItems.ElementAt(i).NodeType == XmlNodeType.Element)
                                {
                                    XElement node = (XElement)teacherItems.ElementAt(i);
                                    switch (node.Name.ToString())
                                    {
                                        case "Nome":
                                            d.Nome = node.Value.ToString();
                                            break;

                                        case "Gabinete":
                                            d.Gabinete = node.Value.ToString();
                                            break;

                                        case "Extensao":
                                            d.Extensao = node.Value.ToString();
                                            break;

                                        case "Webpage":
                                            d.Webpage = node.Value.ToString();
                                            break;
                                    }
                                }
                            }
                            TeacherItem ti = new TeacherItem(d.Nome, path + d.Nome.Replace(" ", "") + ".jpg", d.Gabinete, d.Extensao, d.Webpage);
                            teacherList.Add(ti);
                        }
                    }
                }

                           
                        
                   
            }

        }
   }

}


public class MyDocente
{
    public String Nome, Gabinete, Extensao, Webpage;
}

