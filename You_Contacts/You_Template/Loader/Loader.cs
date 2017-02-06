using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using You_Contacts.Teachers_WS;

namespace You_Contacts.Loader
{
    public partial class YouLoader
    {

        public bool Load()
        {
            DownloadTeachers d = new DownloadTeachers();
            d.download();
            return true;
            //return Update(GetUpdates());
        }
    }
}