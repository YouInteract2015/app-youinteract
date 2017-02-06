using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Contacts
{
    public class HTTPException : Exception
    {
        public HTTPException()
            : base()
        {

        }

        public HTTPException(string message)
            : base(message)
        {
        }
    }
}