using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Videos
{
    /*
     * @author: Vasco Santos, 64191 - 2014
     * @author: Rui Oliveira 68779 - 2015
     */

    class Video
    {
        private int id;
        private string name;
        private string description;
        private string path_video;
        private string path_preview; 

        public Video(int id, string name, string description, string path_video, string path_preview)
        {
            Console.WriteLine("CONSTRUTOR: " + id);
            this.id = id;
            this.name = name;
            this.path_video=path_video;
            this.path_preview = path_preview;
            this.description = description;
        }

        public int getId()
        {
            return id;
        }

        public void setID(int id)
        {
            this.id = id;
        }

        public string getName()
        {
            return name;
        }

        public string getDescription()
        {
            return description;
        }

        public string getpathvideo()
        {
            return path_video; 
        }

        public string getpathpreview()
        {
            return path_preview; 
        }
    }
}
