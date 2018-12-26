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

        private OleDbConnection Conn = new OleDbConnection();
        private OleDbCommand Cmd = new OleDbCommand();
        private OleDbDataReader Rd = new OleDbDataReader ();

        public void Read()
        {
            string path = @"C:\Users\OneDrive - Facebook\Altium\Library\Basic\Basic.accdb";
            string cmdstr = @"select * from [info]";
            using (OleDbConnection Conn = new OleDbConnection(ConnStr(path)))
            {
                Conn.Open();
                using (OleDbCommand Cmd = new OleDbCommand(cmdstr, Conn))
                {

                }
            }

        }


    }
}
