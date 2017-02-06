using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

    /*
     * @author: Rui Oliveira 68779 - 2015
     */
namespace You_Videos
{
    static class Player
    {

        private static string activeVideo;

        public static string getActiveVideo()
        {
            return activeVideo;
        }

        public static void setActiveVideo(string video)
        {
            activeVideo = video;
            videoActivation(video);
        }


        public delegate void EventHandler<String>(String e);

        public static event EventHandler<String> videoActivation = delegate { };

        public static event EventHandler<String> videoDesactivation = delegate { };
    }
}
