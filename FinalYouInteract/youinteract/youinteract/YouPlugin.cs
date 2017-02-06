using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xaml;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

namespace YouInteract.YouBasic
{
    /// <summary>
    /// The Interface that must be implemented by all Pages
    /// </summary>
    public interface YouPlugin
    {
        /// <summary>
        /// A function that returns the Page's Name
        /// </summary>
        /// <returns>The page's Name (usually <b><i>this.Name</i></b>)</returns>
        String getName();
        /// <summary>
        /// A function that returns the name of the App on which the Page is inserted
        /// </summary>
        /// <returns>The application's name</returns>
        String getAppName();
        /// <summary>
        /// A function that returns an instance of the page
        /// </summary>
        /// <returns>The page's reference (usually <b><i>this</i></b>)</returns>
        Page getPage();
        /// <summary>
        /// A function to indentify a page's Kinect Requirements
        /// </summary>
        /// <returns>The kinect Requirements for the page (usually new KinectRequirements(bool,bool,bool)</returns>
        KinectRequirements getKinectRequirements();
        /// <summary>
        /// A function that returns an instance of the Kinect Region
        /// </summary>
        /// <returns>The Kinect region of the page (if used)(usually null or this."kinectRegionName"))</returns>
        KinectRegion getRegion();

        /// <summary>
        /// A function that returns wheter the page is the starting page of a plugin
        /// </summary>
        /// <returns>Wheter the page is the starting page of the app or not (usually true or false, only one true per app)</returns>
        bool getIsFirstPage();
    }

    
}
