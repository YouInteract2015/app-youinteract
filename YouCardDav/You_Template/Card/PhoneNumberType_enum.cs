using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Contacts.Card
{
    [Flags]
    public enum PhoneNumberType_enum
    {
        NotSpecified = 0,
            
        Home = 1,

        Work = 2,

        Cellular = 4,

        Fax = 8,
    }
}
