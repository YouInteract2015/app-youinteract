using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouInteractV1.LoaderData
{
    public class Connection
    {
        public static string Connectionstring = "Data Source=cluster-sql1.mgmt.ua.pt; Initial Catalog = deti- youinteract; Integrated Security = SSPI; Persist security info = False; Trusted_Connection=Yes";
        public static SqlConnection Conn = new SqlConnection(Connectionstring);
    }
}
