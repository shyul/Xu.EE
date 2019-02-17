/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2018 Xu Li - shyu.lee@gmail.com
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xu;

namespace Xu.EE
{
    public static class ComponentList
    {
        public static Dictionary<string, DataTable> Accdb_Basic = new Dictionary<string, DataTable>();

        // Description as key and VendorName+VendorPartNumber as Hash
        public static Dictionary<string, HashSet<Capacitor>> Capacitors = new Dictionary<string, HashSet<Capacitor>>();


        public static Dictionary<string, HashSet<Resistor>> Resistors = new Dictionary<string, HashSet<Resistor>>();


        public static Dictionary<string, HashSet<Inductor>> Inductors = new Dictionary<string, HashSet<Inductor>>();


        public static void InsertCmp<T>(this IDictionary<string, HashSet<T>> list, T c) where T : Component
        {
            c.Id = (ulong)list.Count + 1;

            if (list.ContainsKey(c.Description))
            {
                HashSet<T> set = list[c.Description];
                c.Variation = ((uint)(set.Count + 1), c.Variation.Name);
                set.CheckAdd(c);
            }
            else
            {
                HashSet<T> set = new HashSet<T>();
                set.CheckAdd(c);
                list.Add(c.Description, set);
            }
        }

        public static void WriteToTable<T>(this IDictionary<string, HashSet<T>> list) where T : Component
        {
            foreach(KeyValuePair<string, HashSet<T>> desc in list)
            {
                HashSet<T> subSet = desc.Value;
                Component c = subSet.Where(c0 => c0.Variation.Id == 1).ElementAt(0);

                if (!Accdb_Basic.Keys.Contains(c.TableName)) Accdb_Basic.Add(c.TableName, AccessDb.GetTable(c.TableName));

                DataTable dt = Accdb_Basic[c.TableName];
                if (!dt.Rows.Contains(c.Name))
                    dt.Rows.Add(c.DataRow);
            }
        }

        public static void WriteToFile(string fileName)
        {
            WriteToTable(Capacitors);
            WriteToTable(Resistors);
            WriteToTable(Inductors);
            AccessDb.WriteToFile(Accdb_Basic, fileName);
        }
    }
}
