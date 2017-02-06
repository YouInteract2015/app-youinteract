using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_NewsUA
{
    /**
     * Class for saving New information
     * 
     * @author: Tomás Rodrigues 68129
     *          Pedro Abade 59385
     */

    class Noticia
    {
        // New id
        private string id;
        // New title
        private string title;
        // New link
        private string link;
        // New description
        private string descr;
        // New date
        private string date;

        /**
         * Instantiate a New
         */
        public Noticia(string id, string title, string link, string descr, string data)
        {
            this.id = id;
            this.title = title;
            this.link = link;
            this.descr = descr;

            // Convert received month to portuguese month name
            try
            {
                string[] date_split = data.Split(' ');

                switch (date_split[2])
                {
                    case "Jan":
                        {
                            date_split[2] = "Janeiro";
                            break;
                        }
                    case "Feb":
                        {
                            date_split[2] = "Fevereiro";
                            break;
                        }
                    case "Mar":
                        {
                            date_split[2] = "Março";
                            break;
                        }
                    case "Apr":
                        {
                            date_split[2] = "Abril";
                            break;
                        }
                    case "May":
                        {
                            date_split[2] = "Maio";
                            break;
                        }
                    case "Jun":
                        {
                            date_split[2] = "Junho";
                            break;
                        }
                    case "Jul":
                        {
                            date_split[2] = "Julho";
                            break;
                        }
                    case "Aug":
                        {
                            date_split[2] = "Agosto";
                            break;
                        }
                    case "Sep":
                        {
                            date_split[2] = "Setembro";
                            break;
                        }
                    case "Oct":
                        {
                            date_split[2] = "Outubro";
                            break;
                        }
                    case "Nov":
                        {
                            date_split[2] = "Novembro";
                            break;
                        }
                    case "Dec":
                        {
                            date_split[2] = "Dezembro";
                            break;
                        }
                }

                date = date_split[1] + " " + date_split[2] + " " + date_split[3];
            }
            catch (Exception e) { Console.WriteLine("Can't retrieve New date!"); date = null; }
        }
        
        /**
         * Get New id
         */
        public string getId()
        {
            return id;
        }

        /**
         * Get New title
         */
        public string getTitle()
        {
            return title;
        }

        /**
         * Get New link
         */
        public string getLink()
        {
            return link;
        }

        /**
         * Get New description
         */
        public string getDescription()
        {
            return descr;
        }

        /**
         * Get New date
         */
        public string getDate()
        {
            return date;
        }
    }
}
