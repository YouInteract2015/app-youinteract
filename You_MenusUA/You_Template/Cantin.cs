using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * 
 * @author: Pedro Abade 59385 
 *          Tomás Rodrigues 68129
 *          
 * Class for saving each canteen information
 */
namespace You_MenusUA
{
    class Cantin
    {
        // canteen id
        private int id;
        // canteen name
        private string name;
        // canteen link for menus information
        private string link;
        // canteen photo
        private string photo;

        /**
         * Initialize canteen
         */
        public Cantin(int id, string name, string photo, string description)
        {
            this.id = id;
            this.name = name;
            this.link = description;
            this.photo = photo;
        }

        /**
         * Returns canteen id
         */
        public int getId()
        {
            return this.id;
        }

        /**
         * Returns canteen name
         */
        public string getName()
        {
            return this.name;
        }

        /**
         * Returns canteen link
         */
        public string getLink()
        {
            return this.link;
        }

        /**
         * Returns canteen photo
         */
        public string getPhoto()
        {
            return this.photo;
        }
    }
}
