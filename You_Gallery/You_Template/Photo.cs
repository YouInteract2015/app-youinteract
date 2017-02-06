using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Gallery
{
    /*
     * @author: Rui Oliveira 68779 
     */

    class Photo
    {
        private int id;
        private string name;
        private string description;
        private string path;

        public Photo(int id, string name, string path, string description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.path = path; 
        }


        public string getPath()
        {
            return path; 
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
    }
}
