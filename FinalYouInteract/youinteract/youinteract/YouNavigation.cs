using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouInteract.YouBasic;
using System.Windows.Controls;


namespace YouInteract.YouPlugin_Developing
{
    /// <summary>
    /// Class for developers to Navigate between frames of theire application
    /// </summary>
    public static class YouNavigation
    {
        /// <summary>
        /// Requests the Navigation Class for a Frame Change
        /// </summary>
        /// <param name="sourcePage">A reference of the window calling the Navigation Class (usually <b><i>this</i></b>)</param>
        /// <param name="destinationName">The name of the destination Page</param>
        public static void requestFrameChange(YouPlugin sourcePage, String destinationName)
        {
            if (sourcePage.getKinectRequirements().getKinectRegionReq())
                if (sourcePage.getRegion() != null)
                    KinectApi.unbindRegion(sourcePage.getRegion());

            NavigationRequest(sourcePage.getAppName() + "*" + destinationName);

        }
        /// <summary>
        /// Request the Navigation Class to navigate to the Main Menu
        /// </summary>
        /// <param name="sourcePage">A reference of the window calling the Navigation Class (usually <b><i>this</i></b></n>)</param>
        public static void navigateToMainMenu(YouPlugin sourcePage)
        {
            if (sourcePage.getKinectRequirements().getKinectRegionReq())
                if (sourcePage.getRegion() != null)
                    KinectApi.unbindRegion(sourcePage.getRegion());
            NavigateToMainMenu("YouMainMenu");
        }
        /// <summary>
        /// The Event raised for navigating to the mainMenu. Do not use this method
        /// instead use the method navigateToMainMenu
        /// </summary>
        public static event EventHandler NavigateToMainMenu = delegate { };

        /// <summary>
        /// The event raised to inform the YouPluginManager of a Navigation Request
        /// with the application name and the destination page's name
        /// </summary>
        internal static event EventHandler NavigationRequest = delegate { };

        /// <summary>
        /// The event handler
        /// </summary>
        /// <param name="e">A string</param>
        public delegate void EventHandler(String e);
    }
}
