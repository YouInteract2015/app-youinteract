using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace You_NewsUA
{
    /**
     * Class for saving all loaded News
     * 
     * @author: Tomás Rodrigues 68129
     *          Pedro Abade 59385
     */
    static class AllNews
    {
        // List of News
        private static Noticia[] allNews;
        // List of News Categories
        private static XmlNodeList lista_categorias;

        /**
         * Instantiate lists
         */
        public static void instantate(int nNews)
        {
            allNews = null ;
            allNews = new Noticia[nNews];
            lista_categorias = null;
        }

        /**
         * Set News
         */
        public static void setNews(Noticia[] news)
        {
            int i;
            for (i = 0; i < allNews.Length; i++)
            {
                allNews[i] = news[i];
            }
        }

        /**
         * Get all News
         */
        public static Noticia[] getNews()
        {
            return allNews;
        }

        /**
         * Set News Categories
         */
        public static void setCategories(XmlNodeList lista_cat, int cat)
        {
            lista_categorias = lista_cat;

            newActivation(cat);
        }

        /**
         * Get all Categories
         */
        public static XmlNodeList getCategories()
        {
            return lista_categorias;
        }

        public delegate void EventHandler<String>(int e);
        // Evend Handler when selecting a New Categorie
        public static event EventHandler<int> newActivation = delegate { };
    }
}
