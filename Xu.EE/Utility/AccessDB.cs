using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Xu.EE
{
    public static class AccessDb
    {
        public static OleDbConnection Connect(string fileName)
            => new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Persist Security Info = False; ");

        public static bool TableExists(this OleDbConnection conn, string tableName)
            => conn.GetSchema("Tables", new string[4] { null, null, tableName, "TABLE" }).Rows.Count > 0;

        public static void DeleteTable(this OleDbConnection conn, string tableName)
        {
            using (OleDbCommand cmd = new OleDbCommand("DROP TABLE [" + tableName + "]", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public static void CreateTable(this OleDbConnection conn, DataTable dt)
        {
            if (!conn.TableExists(dt.TableName))
            {
                string sqlCreateCmd = "CREATE TABLE [" + dt.TableName + "](";
                foreach (DataColumn dc in dt.Columns)
                {
                    sqlCreateCmd += "[" + dc.ColumnName + "] VARCHAR(255), ";
                }

                if (dt.PrimaryKey.Count() > 0)
                {
                    sqlCreateCmd += "PRIMARY KEY (";
                    foreach (DataColumn dc in dt.PrimaryKey)
                    {
                        sqlCreateCmd += "[" + dc.ColumnName + "],";
                    }
                    sqlCreateCmd = sqlCreateCmd.Trim().Trim(',') + ")";
                }

                sqlCreateCmd = sqlCreateCmd.Trim().Trim(',') + ");";
                Console.WriteLine("Creating Table: " + sqlCreateCmd);
                OleDbCommand addtablecmd = new OleDbCommand(sqlCreateCmd, conn);
                addtablecmd.ExecuteNonQuery();
            }
        }

        public static void PrepareTable(this OleDbConnection conn, DataTable dt)
        {
            if (TableExists(conn, dt.TableName))
                DeleteTable(conn, dt.TableName);
            CreateTable(conn, dt);
        }

        public static void InsertTable(this OleDbConnection conn, DataTable dt)
        {
            string columnLine = "(";
            string valueLine = "(";
            Dictionary<string, string> columnToValue = new Dictionary<string, string>();

            foreach (DataColumn dc in dt.Columns)
            {
                string colName = "[" + dc.ColumnName + "]";
                string valueName = "@" + dc.ColumnName.ToLower().Replace(" ", string.Empty);

                columnToValue.Add(dc.ColumnName, valueName);
                columnLine += colName + ", ";
                valueLine += valueName + ", ";
            }

            columnLine = columnLine.Trim().Trim(',') + ")";
            valueLine = valueLine.Trim().Trim(',') + ")";

            using (OleDbCommand cmd = new OleDbCommand("INSERT INTO [" + dt.TableName + "]" + columnLine + " VALUES " + valueLine + ";", conn))
            {
                Console.WriteLine(cmd.CommandText);

                foreach (string volName in columnToValue.Values)
                    cmd.Parameters.Add(volName, OleDbType.VarChar, 255);

                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        cmd.Parameters[columnToValue[dc.ColumnName]].Value = dr[dc.ColumnName].ToString();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static DataTable GetTable(string tableName)
        {
            DataTable dt = new DataTable(tableName);
            DataColumn keyColumn = new DataColumn("Component Name", typeof(string));
            dt.Columns.Add(keyColumn);
            dt.Columns.Add("Item ID", typeof(string));
            dt.Columns.Add("Comment", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("Package Name", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Mfg Name", typeof(string));
            dt.Columns.Add("Mfg PN", typeof(string));
            dt.Columns.Add("Library Ref", typeof(string));
            dt.Columns.Add("Library Path", typeof(string));
            dt.Columns.Add("Footprint Ref", typeof(string));
            dt.Columns.Add("Footprint Path", typeof(string));
            dt.Columns.Add("Sim Description", typeof(string)); // Vendor Data
            dt.Columns.Add("Sim Kind", typeof(string)); // General
            dt.Columns.Add("Sim SubKind", typeof(string)); // Spice Subcircuit
            dt.Columns.Add("Sim Spice Prefix", typeof(string)); // X
            dt.Columns.Add("Sim Netlist", typeof(string)); // @DESIGNATOR %1 %2 @MODEL
            dt.Columns.Add("Sim Port Map", typeof(string)); // (1:Port1),(2:Port2)
            dt.Columns.Add("Sim File", typeof(string)); // Basic\Simulation\Murata\GRM.ckt *** first three letters.
            dt.Columns.Add("Sim Model Name", typeof(string)); // --- VendorPartNumber ---
            dt.Columns.Add("Sim Parameters", typeof(string));

            //dt.Columns.Add("Update Values", typeof(string));
            //dt.Columns.Add("Add To Design", typeof(string));
            //dt.Columns.Add("Visible On Add", typeof(string));
            //dt.Columns.Add("Remove From Design", typeof(string));

            dt.PrimaryKey = new DataColumn[] { keyColumn };
            dt.AcceptChanges();
            return dt;
        }

        public static void WriteToFile(IDictionary<string, DataTable> ds, string fileName)
        {
            foreach (DataTable dts in ds.Values)
            {
                dts.AcceptChanges();
                using (OleDbConnection conn = Connect(fileName))
                {
                    try
                    {
                        conn.Open();
                        conn.PrepareTable(dts);
                        conn.InsertTable(dts);

                        Console.WriteLine("\ndoing good.\n\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failure: " + ex);
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }

        public static DataTable FromCsv(string path, bool isFirstRowHeader = true)
        {
            string hasHeader = isFirstRowHeader ? "Yes" : "No";
            string pathName = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

            using (OleDbConnection conn = new OleDbConnection(
                      @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathName +
                      ";Extended Properties=\"Text;HDR=" + hasHeader + "\""))
            using (OleDbCommand command = new OleDbCommand(@"SELECT * FROM [" + fileName + "]", conn))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dt = new DataTable
                {
                    Locale = CultureInfo.CurrentCulture
                };

                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
