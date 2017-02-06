using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;
using YouInteract.YouBasic;
using System.Xaml;
using System.Windows.Controls;
using YouInteract.YouPlugin_Developing;


namespace YouInteract.YouPlugin_System
{
    /// <summary>
    /// Used for Managing and Navigating through the Pages added to YouInteract
    /// </summary>
    public class YouPluginManager
    {

        private YouPlugin[] Plugins;
        /// <summary>
        /// The Constructor of YouPluginManager. It will load Plugins based on which apps are active.
        /// </summary>
        /// <param name="activeApps">A String Array containing the name of the active Applications</param>
        public YouPluginManager(String[] activeApps)
        {
            String path = System.AppDomain.CurrentDomain.BaseDirectory;
            string[] pluginFiles = Directory.GetFiles(path, "You_*.dll");

            try
            {
                // Function that loads all plugins that are YouInteract related.
                var listaPlugins = (
                    // From each file in the files.
                    from file in pluginFiles
                    // Load the assembly.
                    let asm = Assembly.LoadFile(file)
                    // For every type in the assembly that is visible outside of
                    // the assembly.
                    from type in asm.GetExportedTypes()

                    // Where the type implements the interface.
                    where typeof(YouPlugin).IsAssignableFrom(type)
                    // Create the instance.
                    select (YouPlugin)Activator.CreateInstance(type)

                    // Materialize to an array.
                    ).ToArray();
                // Function that cross-references the active apps and the plugins lists to find the ones that are active
                Plugins = (
                    // From each Plugin in the Plugin List.
                    from p in listaPlugins
                    // For every Plugin that it's name is contained in the Active Apps Array
                    where activeApps.Contains(p.getAppName())
                    // Select thos Plugins to be stored
                    select p

                    ).ToArray();

            }
            catch (MissingMethodException)
            {
                Console.WriteLine("Crashou");
            }

            YouNavigation.NavigationRequest += YouNavigation_NavigationRequest;

        }

        private void YouNavigation_NavigationRequest(string e)
        {
            YouPlugin destination = (
                    from p in Plugins
                    where p.getAppName() == e.Split('*')[0] && p.getName() == e.Split('*')[1]

                    select p

                ).First();
            if (destination != null)
            {
                KinectReqReady(new KinectRequirementsArgs(destination.getKinectRequirements()));
                FrameReady(new NavigateArgs(destination.getPage()));
                if (destination.getKinectRequirements().getKinectRegionReq())
                    KinectApi.bindRegion(destination.getRegion());
            }
            else
            {
                Console.WriteLine("Page could not be loaded, no page with name: " + destination.ToString());
            }

        }

        /// <summary>
        /// Our Navigation Platform, not public for security reasons
        /// </summary>
        /// <param name="appName">The destination app name</param>
        /// <param name="pageName">The destination page name</param>
        public void freeNavigate(String appName)
        {
            // The frame destination
            YouPlugin destination = (
                // from plugin in the list of active plugins
                from p in Plugins
                // search for a page which the appname and name correspond to the function arguments
                where p.getAppName() == appName && p.getIsFirstPage() == true
                // select that page
                select p

                ).First();
            // If the page exist, navigate to it
            if (destination != null)
            {
                FrameReady(new NavigateArgs(destination.getPage()));
                if (destination.getKinectRequirements().getKinectRegionReq())
                    KinectApi.bindRegion(destination.getRegion());
            }

        }


        /// <summary>
        /// An auxiliary class that contains the destination Page so we can change the frame in the MainWindow
        /// </summary>
        public class NavigateArgs
        {

            public Page Destination { get; private set; }
            public NavigateArgs(Page e)
            {
                Destination = e;

            }
        }

        /// <summary>
        /// Event Handler for when a frame is ready to be navigated to
        /// </summary>
        /// <param name="e">The destination page, as an argument</param>
        public delegate void NavigationEventHandler(NavigateArgs e);
        /// <summary>
        /// The event launched when a frame is ready to be navigated to
        /// </summary>
        public event NavigationEventHandler FrameReady = delegate { };

        public class KinectRequirementsArgs
        {
            public KinectRequirements req { get; private set; }
            public KinectRequirementsArgs(KinectRequirements e)
            {
                req = e;
            }

        }

        public static event KinectReqEventHandler KinectReqReady = delegate { };

        public delegate void KinectReqEventHandler(KinectRequirementsArgs e);
    }
}
