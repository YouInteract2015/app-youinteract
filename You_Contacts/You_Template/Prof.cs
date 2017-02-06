using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace You_Contacts
{
    /*
     * @author: Jose Mendes, 50463
     */

    class Prof
    {
        private int id;
        private string name;
        private string office;
        private string phone;
        private BitmapImage bitmap;

        public Prof(int id, string name, string office, string phone, BitmapImage bitmap)
        {
            this.id = id;
            this.office = office;
            this.name = name;
            this.phone = phone;
            this.bitmap = bitmap;
        }

        public string getOffice()
        {
            return office;
        }

        public string getName()
        {
            return name;
        }

        public string getPhone()
        {
            return phone;
        }
        public int getId()
        {
            return id;
        }

        public BitmapImage getImage()
        {
            return bitmap;
        }
    }
}