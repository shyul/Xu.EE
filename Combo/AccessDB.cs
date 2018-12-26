using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace Combo
{
    public class AccessDB
    {
        private string ConnStr(string path) => "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False";
        // Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\OneDrive - Facebook\Altium\Library\Basic\Basic.accdb;Persist Security Info=False

        public DataTable Read()
        {
            string path = @"C:\Users\OneDrive - Facebook\Altium\Library\Basic\Basic.accdb";
            string cmdstr = @"select * from [info]";

            DataTable dt = new DataTable();

            using (OleDbConnection Conn = new OleDbConnection(ConnStr(path)))
            {
                Conn.Open();
                using (OleDbCommand Cmd = new OleDbCommand(cmdstr, Conn))
                {
                    using (OleDbDataReader Rd = Cmd.ExecuteReader())
                    {
                        dt.Load(Rd);
                    }
                }
                Conn.Close();
            }

            return dt;
        }

        // https://stackoverflow.com/questions/18961938/populate-data-table-from-data-reader
        // https://stackoverflow.com/questions/4781323/how-do-you-create-an-ms-access-table-in-c-sharp-programmatically
        // https://stackoverflow.com/questions/20998803/c-sharp-datatable-update-access-database
    }




}
