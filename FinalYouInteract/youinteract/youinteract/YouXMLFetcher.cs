using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YouInteract.YouBasic;

namespace YouInteract.YouPlugin_Developing
{
    /// <summary>
    /// Contains the events to get the xml files from the data base.
    /// </summary>
    /// <returns>The XML page for the application if its created. Null if it isn't</returns>
    public static class YouXMLFetcher
    {
        /// <summary>
        /// Get a XML file with the App Information
        /// </summary>
        public static XDocument getAppXml(YouPlugin plugin)
        {
            try
            {
              /*  if (XDocument.Load(System.AppDomain.CurrentDomain.BaseDirectory + "/XMLAccess/APPXML_" +
                                   plugin.getAppName() +
                                   ".xml") != null)
                {
                    return
                        XDocument.Load(System.AppDomain.CurrentDomain.BaseDirectory + "/XMLAccess/APPXML_" +
                                       plugin.getAppName() + ".xml");
                }*/
                /*Console.WriteLine("Apps names:");
                Console.WriteLine(plugin.getAppName().ToString());*/

                Console.WriteLine("PLUGIN NAME:");
                Console.WriteLine(plugin.getAppName().ToString());

                return XDocument.Load(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YouInteract/XMLAccess/" + plugin.getAppName().ToString() + ".xml"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
            return null;
        }

    }
}