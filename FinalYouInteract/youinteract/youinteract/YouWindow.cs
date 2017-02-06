using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Controls;
using YouInteract.YouInteractAPI;

namespace YouInteract.YouPlugin_Developing
{
    /*
     * @author: Vasco Santos, 64191
     * 
     * - Height e Width das páginas
     * - Funções comuns em menus de selecção - MainMenu, Videos e Contactos
     */

    /// <summary>
    /// Auxiliary Class for reading the screens height and width
    /// Contains function for setting up simples menus
    /// </summary>
    public static class YouWindow
    {
        /// <summary>
        /// The height value of the display
        /// </summary>
        private static double Height;
        /// <summary>
        /// The width value of the display
        /// </summary>
        private static double Width;

        /// <summary>
        /// To set the screen measures
        /// </summary>
        /// <param name="height">Screen Height</param>
        /// <param name="width">Screen Width</param>
        public static void setWindow(double height, double width)
        {
            Height = height;
            Width = width;
        }
        /// <summary>
        /// Function for getting the screens height
        /// </summary>
        /// <returns>Returns the screen's height</returns>
        public static double getHeight()
        {
            return Height;
        }
        /// <summary>
        /// Function for getting the screens width
        /// </summary>
        /// <returns>Returns the screen's width </returns>
        public static double getWidth()
        {
            return Width;
        }

        /// <summary>
        /// Inserts the YouInteract Logo into an image
        /// </summary>
        /// <param name="logo"> A image file that will receive the BitMapImage of the logo</param>
        /// <param name="w"> Width of the image</param>
        /// <param name="h"> Height of the image</param>
        public static void initLogo(Image logo, double w, double h)
        {
            BitmapImage bitmapLogo = new BitmapImage();
            bitmapLogo.BeginInit();
            bitmapLogo.UriSource = new Uri("/YouInteractV1;component/Images/Logo/tvlogo.png", UriKind.Relative);
            bitmapLogo.EndInit();
            logo.Stretch = Stretch.Fill;
            logo.Source = bitmapLogo;
            logo.Width = Width * w;
            logo.Height = Height * h;
            Canvas.SetLeft(logo, Width * 0.5 - logo.Width * 0.5);
            Canvas.SetBottom(logo, 0);
        }

        /// <summary>
        /// Set a source of an image to a bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="source"></param>
        /// <param name="type"></param>
        public static void bitmapSource(BitmapImage bitmap, string source, UriKind type)
        {
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(source, type);
            bitmap.EndInit();
        }

        /// <summary>
        /// Set the Scroller Arrows Position and Size in the Canvas
        /// </summary>
        /// <param name="ScrollLeft">Kinect Hover Button from the left side</param>
        /// <param name="ScrollRight">Kinect Hover Button from the right side</param>
        /// <param name="h">Arrow Height</param>
        /// <param name="w">Arrow Width</param>
        /// <param name="lTop">Left Arrow Canvas Top Position</param>
        /// <param name="lLeft">Left Arrow Canvas Left Position</param>
        /// <param name="rTop">Right Arrow Canvas Top Position</param>
        /// <param name="rLeft">Right Arrow Canvas Left Position</param>
        
        public static void setScrollArrows(KinectHoverButton ScrollLeft, KinectHoverButton ScrollRight, double h, double w, double lTop, double lLeft, double rTop, double rLeft)
        {
            ScrollLeft.Height = Height * h;
            ScrollLeft.Width = Width * w;
            ScrollLeft.Background = Brushes.Transparent;
            ScrollRight.Height = Height * h;
            ScrollRight.Width = Width * w;
            ScrollRight.Background = Brushes.Transparent;

            Canvas.SetTop(ScrollLeft, Height * lTop);
            Canvas.SetLeft(ScrollLeft, Width * lLeft);
            Canvas.SetTop(ScrollRight, Height * rTop);
            Canvas.SetLeft(ScrollRight, Width * rLeft);
        }
        

        /// <summary>
        /// Set the User Viewer Position and Size in the Canvas
        /// </summary>
        /// <param name="Viewer">Kinect User Viewer Control</param>
        /// <param name="w">User Viewer Width</param>
        /// <param name="h">User Viewer Height</param>
        /// <param name="setBottom">User Viewer Set Bottom Canvas</param>
        /// <param name="setRight">User Viewer Set Right Canvas</param>
        public static void setUserViewer(KinectUserViewer Viewer, double w, double h, double setBottom, double setRight)
        {
            Viewer.Width = Width * w;
            Viewer.Height = Height * h;
            Canvas.SetBottom(Viewer, setBottom);
            Canvas.SetRight(Viewer, setRight);
        }
    }
}
