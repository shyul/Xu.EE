/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2018 Xu Li - shyu.lee@gmail.com
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace Xu.EE
{
    public static class Murata
    {
        public static void ImportAll(string pathName)
        {
            BuildPartNumberWithSim(pathName + @"Library\Basic\Simulation\Murata\");

            //Dictionary<string, DataTable> ds = InitiateEmptyTables();

            string filePath = pathName + @"Data\Components\capacitor_murata\";
            string[] files = new string[] { "GRM", "GJM", "GQM", "GCM", "GCQ" };

            foreach (string file in files)
            {
                DataTable csv = AccessDb.FromCsv(filePath + file + ".csv");

                foreach (DataRow dw in csv.Rows)
                {
                    string mfrpn = dw["Part number (#:Packing code)"].ToString(); // Console.WriteLine(dw["Part number (#:Packing code)"].ToString());
                    (Capacitor c, bool isValid) = DecodeCapacitor(mfrpn);

                    if (isValid)
                    {
                        ComponentList.Capacitors.InsertCmp(c);
                    }
                }
            }

            Console.WriteLine("\n");
            foreach (string s in AllListPackSizes.Keys)
            {
                Console.WriteLine(s + ": " + AllListPackSizes[s]);
            }
            Console.WriteLine("\n");

            filePath = pathName + @"Data\Components\inductor_murata\";

            FileInfo[] Files = Xu.Serialization.GetFileList(filePath, "*.csv");

            foreach (FileInfo file in Files)
            {
                Console.WriteLine(file.FullName); //file.Name;
                DataTable csv = AccessDb.FromCsv(file.FullName);

                //if(csv.Columns.Contains("Part number # indicates the package specification code#")) Console.Write(" ok. \n");
                //else Console.Write(" error. \n");
                //Console.Write(" Count: " + csv.Columns[2].ColumnName + "  \n");
                /*
                foreach(DataColumn dc in csv.Columns)
                {
                    Console.WriteLine(dc.ColumnName);
                }*/

                
                foreach(DataRow dr in csv.Rows)
                {
                    string mfrpn = ((string)dr["Part number # indicates the package specification code#"]).Trim('#');
                    string seriesType = ((string)dr["Series"]).Trim('#');
                    string srf = (string)dr["SRF (min#)"];
                    string rdc = (string)dr["Rdc (max#)"];
                    //string Isat = (string)dr["Rated Current (Isat)"];
                    string Itemp = (string)dr["Rated Current (Itemp)"];
                    //string tolerance = (string)dr["Inductance Tolerance"];
                    // inductance = (string)dr["Inductance"];

                    (Inductor ind, bool isValid) = DecodeInductor(mfrpn);

                    if (isValid)
                    {




                        ComponentList.Inductors.InsertCmp(ind);
                    }







                }

            
            }
        }

        public static HashSet<string> StandardPackages = new HashSet<string>()
        { "008004", "01005", "015008", "0201", "0402", "0603", "0704", "0805", "1111", "1206", "1210", "1808", "1812", "2220" };

        public static DataTable InitiateEmptyTable(string packageName)
        {
            DataTable dt = AccessDb.GetTable("Capacitor - " + packageName);
            dt.Rows.Add(new object[] { "NC_" + packageName, "N/A", "NC", "0pF", packageName, "CAP,SM," + packageName + ",NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", packageName +"_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            return dt;
        }

        public static Dictionary<string, DataTable> InitiateEmptyTables()
        {
            Dictionary<string, DataTable> ds = new Dictionary<string, DataTable>();
            foreach (string s in StandardPackages)
            {
                DataTable t = InitiateEmptyTable(s);
                ds.CheckAdd(t.TableName, t);
            }
            return ds;
        }

        public static Dictionary<string, string> AllListPackSizes = new Dictionary<string, string>();

        public static Dictionary<string, string> AllListPartNumberWithSim = new Dictionary<string, string>(); // PartNumber : FileName

        public static void BuildPartNumberWithSim(string dirPath)
        {
            FileInfo[] Files = Xu.Serialization.GetFileList(dirPath, "*.ckt");
            foreach (FileInfo file in Files)
            {
                //Console.WriteLine(file.FullName); //file.Name;
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.StartsWith(".SUBCKT"))
                        {
                            string[] vals = line.Split(' ');
                            //Console.WriteLine(vals[1]);
                            AllListPartNumberWithSim.CheckAdd(vals[1], file.Name);
                        }
                    }
                }
            }
        }

        public static (Inductor ind, bool isValid) DecodeInductor(string partNumber)
        {
            partNumber = partNumber.Trim('#');

            string partcode = partNumber.Substring(0, 2); // *
            string typecode = partNumber.Substring(2, 1); // *
            string packageCode = partNumber.Substring(3, 2);
            string appCode = partNumber.Substring(5, 1); // *
            string catCode = partNumber.Substring(6, 1); // *
            string valueCode = partNumber.Substring(7, 3);
            string toleranceCode = partNumber.Substring(10, 1);

            Inductor ind = new Inductor()
            {
                VendorName = "Murata",
                VendorPartNumber = partNumber,
                Value = FromValueCode3(valueCode, 'N'),
                PackageName = FromPackageCode(packageCode),
            };

            bool isValid = true;

            switch (toleranceCode)
            {
                case ("W"): ind.Tolerance = (100 * 0.05 / ind.Value); ind.ToleranceDescription = "±0.05nH"; break;
                case ("B"): ind.Tolerance = (100 * 0.1 / ind.Value); ind.ToleranceDescription = "±0.1nH"; break;
                case ("C"): ind.Tolerance = (100 * 0.2 / ind.Value); ind.ToleranceDescription = "±0.2nH"; break;
                case ("S"): ind.Tolerance = (100 * 0.2 / ind.Value); ind.ToleranceDescription = "±0.3nH"; break;
                case ("D"): ind.Tolerance = (100 * 0.5 / ind.Value); ind.ToleranceDescription = "±0.5nH"; break;

                case ("F"): ind.Tolerance = 1; ind.ToleranceDescription = "±1%"; break;
                case ("G"): ind.Tolerance = 2; ind.ToleranceDescription = "±2%"; break;
                case ("H"): ind.Tolerance = 2; ind.ToleranceDescription = "±3%"; break;
                case ("R"): ind.Tolerance = 2.5; ind.ToleranceDescription = "±2.5%"; break;
                case ("J"): ind.Tolerance = 5; ind.ToleranceDescription = "±5%"; break;
                case ("K"): ind.Tolerance = 10; ind.ToleranceDescription = "±10%"; break;
                case ("M"): ind.Tolerance = 20; ind.ToleranceDescription = "±20%"; break;
                default: throw new ArgumentException("Invalid Murata Part Number");
            }

            ind.FootprintName = ind.PackageName + "_" + partNumber.Substring(0, 5);

            if (AllListPartNumberWithSim.ContainsKey(partNumber))
            {
                ind.SimDescription = "Vendor Simulation Data";
                ind.SimKind = "General";
                ind.SimSubKind = "Spice Subcircuit";
                ind.SimSpicePrefix = "X";
                ind.SimNetlist = "@DESIGNATOR %1 %2 @MODEL";
                ind.SimPortMap = "(1:1),(2:2)";
                //ind.SimFile = @"Basic\Simulation\Murata\" + typecode + ".ckt";
                ind.SimFile = @"Basic\Simulation\Murata\" + AllListPartNumberWithSim[partNumber];
                ind.SimModel = partNumber;
                ind.SymbolName = "INDUCTOR_SIM";
            }

            switch (typecode)
            {
                case ("G"): ind.InductorType = InductorType.MULTILAYER; break;
                case ("P"): ind.InductorType = InductorType.FILM; break;
                case ("W"): ind.InductorType = InductorType.WIRE_WOUND; break;
                case ("H"): ind.InductorType = InductorType.FERRITE_WIRE_WOUND; break;
                default: return (ind, false);
            }



            return (ind, isValid);
        }

        public static (Capacitor c, bool isValid) DecodeCapacitor(string partNumber)
        {
            partNumber = partNumber.Trim('#');

            string typecode = partNumber.Substring(0, 3); // *

            string packageCode = partNumber.Substring(3, 2);
            string heightCode = partNumber.Substring(5, 1); // *
            string tempCode = partNumber.Substring(6, 2);
            string voltageCode = partNumber.Substring(8, 2);
            string valueCode = partNumber.Substring(10, 3);
            string toleranceCode = partNumber.Substring(13, 1);

            bool isValid = true;

            Capacitor c = new Capacitor
            {
                VendorName = "Murata",
                VendorPartNumber = partNumber,
                Value = FromValueCode3(valueCode),
                PackageName = FromPackageCode(packageCode),
            };

            AllListPackSizes.CheckAdd(c.PackageName + "_" + heightCode, partNumber);

            switch (typecode)
            {
                case ("GRM"):
                    break;
                case ("GR3"):
                    c.Tags.Add("Hi-Ripple");
                    break;
                case ("GJM"):
                    c.Tags.Add("Hi-Q");
                    break;
                case ("GQM"):
                    c.Tags.Add("Hi-Q");
                    c.Tags.Add("Hi-Power");
                    break;
                case ("GCM"):
                    c.Tags.Add("AEC-Q200");
                    break;
                case ("GCQ"):
                    c.Tags.Add("AEC-Q200");
                    c.Tags.Add("Hi-Q");
                    break;
                default: throw new ArgumentException("Invalid Murata Part Number");
            }

            c.TableName = "Capacitor - " + c.PackageName;
            if (c.Tags.Count > 0) c.TableName += " - " + Xu.Collection.ToString<string>(c.Tags, "/");

            switch (tempCode)
            {
                case ("1X"): c.MaterialTempCode = "SL"; c.TemperatureRange = new Range<double>(-55, 125); isValid = false; break;
                case ("1C"): c.MaterialTempCode = "CG"; c.TemperatureRange = new Range<double>(-55, 125); isValid = false; break;
                case ("2C"): c.MaterialTempCode = "CH"; c.TemperatureRange = new Range<double>(-55, 125); isValid = false; break;
                case ("3C"): c.MaterialTempCode = "CJ"; c.TemperatureRange = new Range<double>(-55, 125); isValid = false; break;
                case ("3U"): c.MaterialTempCode = "UJ"; c.TemperatureRange = new Range<double>(-25, 85); isValid = false; break;
                case ("4C"): c.MaterialTempCode = "CK"; c.TemperatureRange = new Range<double>(-55, 125); isValid = false; break;
                case ("5C"): c.MaterialTempCode = "C0G"; c.TemperatureRange = new Range<double>(-55, 125); break;
                case ("5G"): c.MaterialTempCode = "X8G"; c.TemperatureRange = new Range<double>(-55, 150); break;
                case ("7U"): c.MaterialTempCode = "U2J"; c.TemperatureRange = new Range<double>(-55, 125); break;
                case ("B1"): c.MaterialTempCode = "B1"; c.TemperatureRange = new Range<double>(-25, 85); isValid = false; break;
                case ("B3"): c.MaterialTempCode = "B3"; c.TemperatureRange = new Range<double>(-25, 85); isValid = false; break;
                case ("C6"): c.MaterialTempCode = "X5S"; c.TemperatureRange = new Range<double>(-55, 85); break;
                case ("C7"): c.MaterialTempCode = "X7S"; c.TemperatureRange = new Range<double>(-55, 125); break;
                case ("C8"): c.MaterialTempCode = "X6S"; c.TemperatureRange = new Range<double>(-55, 105); break;
                case ("D7"): c.MaterialTempCode = "X7T"; c.TemperatureRange = new Range<double>(-55, 125); break;
                case ("D8"): c.MaterialTempCode = "X6T"; c.TemperatureRange = new Range<double>(-55, 105); break;
                case ("E7"): c.MaterialTempCode = "X7U"; c.TemperatureRange = new Range<double>(-55, 125); break;
                case ("R1"): c.MaterialTempCode = "R"; c.TemperatureRange = new Range<double>(-55, 125); isValid = false; break;
                case ("R6"): c.MaterialTempCode = "X5R"; c.TemperatureRange = new Range<double>(-55, 85); break;
                case ("R7"): c.MaterialTempCode = "X7R"; c.TemperatureRange = new Range<double>(-55, 125); break;
                case ("W0"): c.MaterialTempCode = "X7T"; c.TemperatureRange = new Range<double>(-55, 125); break;
                case ("Z7"): c.MaterialTempCode = "X7R"; c.TemperatureRange = new Range<double>(-55, 125); break;
                case ("9E"): c.MaterialTempCode = "ZLM"; c.TemperatureRange = new Range<double>(-55, 125); isValid = false; break;
                case ("L8"): c.MaterialTempCode = "X8L"; c.TemperatureRange = new Range<double>(-55, 125); isValid = false; break;
                case ("M8"): c.MaterialTempCode = "X8M"; c.TemperatureRange = new Range<double>(-55, 125); isValid = false; break;
                case ("R9"): c.MaterialTempCode = "X8R"; c.TemperatureRange = new Range<double>(-55, 125); break;
                default: throw new ArgumentException("Invalid Murata Part Number");
            }

            switch (c.PackageName)
            {
                case ("008004"):
                    c.FootprintName = "008004";
                    break;
                case ("01005"):
                    c.FootprintName = "01005";
                    break;
                case ("015008"):
                    c.FootprintName = "015008";
                    break;
                case ("0201"):
                    {
                        switch (heightCode)
                        {
                            case ("2"): c.FootprintName = "0201_2"; break;
                            case ("5"): c.FootprintName = "0201_5"; break;
                            default: c.FootprintName = "0201"; break;
                        }
                    }
                    break;
                case ("0402"):
                    {
                        switch (heightCode)
                        {
                            case ("2"): c.FootprintName = "0402_2"; break;
                            case ("3"): c.FootprintName = "0402_3"; break;
                            case ("6"): c.FootprintName = "0402_6"; break;
                            default: c.FootprintName = "0402"; break;
                        }
                    }
                    break;
                case ("0603"):
                    {
                        switch (heightCode)
                        {
                            case ("6"): c.FootprintName = "0603_6"; break;
                            case ("7"): c.FootprintName = "0603_7"; break;
                            case ("8"): c.FootprintName = "0603_8"; break;
                            default: c.FootprintName = "0603"; break;
                        }
                    }
                    break;
                case ("0704"):
                    {
                        switch (heightCode)
                        {
                            default: c.FootprintName = "0704"; break;
                        }
                    }
                    break;
                case ("0805"):
                    {
                        switch (heightCode)
                        {
                            case ("9"): c.FootprintName = "0805_9"; break;
                            case ("A"): c.FootprintName = "0805_A"; break;
                            case ("B"): c.FootprintName = "0805_B"; break;
                            default: c.FootprintName = "0805"; break;
                        }
                    }
                    break;
                case ("1111"):
                    {
                        switch (heightCode)
                        {
                            default: c.FootprintName = "1111"; break;
                        }
                    }
                    break;
                case ("1206"):
                    {
                        switch (heightCode)
                        {
                            case ("A"): c.FootprintName = "1206_A"; break;
                            case ("B"): c.FootprintName = "1206_B"; break;
                            case ("C"): c.FootprintName = "1206_C"; break;
                            case ("M"): c.FootprintName = "1206_M"; break;
                            default: c.FootprintName = "1206"; break;
                        }
                    }
                    break;
                case ("1210"):
                    {
                        switch (heightCode)
                        {
                            case ("B"): c.FootprintName = "1210_B"; break;
                            case ("D"): c.FootprintName = "1210_D"; break;
                            case ("E"): c.FootprintName = "1210_E"; break;
                            case ("Q"): c.FootprintName = "1210_Q"; break;
                            default: c.FootprintName = "1210"; break;
                        }
                    }
                    break;
                case ("1808"):
                    {
                        switch (heightCode)
                        {
                            default: c.FootprintName = "1808"; break;
                        }
                    }
                    break;
                case ("1812"):
                    {
                        switch (heightCode)
                        {
                            case ("D"): c.FootprintName = "1812_D"; break;
                            default: c.FootprintName = "1812"; break;
                        }
                    }
                    break;
                case ("2220"):
                    {
                        switch (heightCode)
                        {
                            case ("D"): c.FootprintName = "2220_D"; break;
                            default: c.FootprintName = "2220"; break;
                        }
                    }
                    break;
                default:
                    return (c, false);
            }

            if (c.MaterialTempCode == "C0G") c.FootprintName += "_GRAY";

            switch (voltageCode)
            {
                case ("0E"): c.Voltage = 2.5; break;
                case ("0G"): c.Voltage = 4; break;
                case ("0J"): c.Voltage = 6.3; break;
                case ("1A"): c.Voltage = 10; break;
                case ("1C"): c.Voltage = 16; break;
                case ("EE"): c.Voltage = 16; break;
                case ("1E"): c.Voltage = 25; break;
                case ("EF"): c.Voltage = 25; break;
                case ("YA"): c.Voltage = 35; break;
                case ("EG"): c.Voltage = 35; break;
                case ("1H"): c.Voltage = 50; break;
                case ("EH"): c.Voltage = 50; break;
                case ("1J"): c.Voltage = 63; break;
                case ("1K"): c.Voltage = 80; break;
                case ("2A"): c.Voltage = 100; break;
                case ("EL"): c.Voltage = 100; break;
                case ("2D"): c.Voltage = 200; break;
                case ("2E"): c.Voltage = 250; break;
                case ("2W"): c.Voltage = 450; break;
                case ("2H"): c.Voltage = 500; break;
                case ("2J"): c.Voltage = 630; break;
                case ("3A"): c.Voltage = 1000; break;
                case ("3D"): c.Voltage = 2000; break;
                case ("3F"): c.Voltage = 3150; break;
                default: throw new ArgumentException("Invalid Murata Part Number");
            }

            switch (toleranceCode)
            {
                case ("W"): c.Tolerance = (100 * 0.05 / c.Value); c.ToleranceDescription = "±0.05pF"; break;
                case ("B"): c.Tolerance = (100 * 0.1 / c.Value); c.ToleranceDescription = "±0.1pF"; break;
                case ("C"): c.Tolerance = (100 * 0.25 / c.Value); c.ToleranceDescription = "±0.25pF"; break;
                case ("D"):
                    {
                        if (c.Value < 10)
                        {
                            c.Tolerance = (100 * 0.5 / c.Value);
                            c.ToleranceDescription = "±0.5pF";
                        }
                        else
                        {
                            c.Tolerance = 0.5;
                            c.ToleranceDescription = "±0.5%";
                        }
                    }
                    break;
                case ("F"): c.Tolerance = 1; c.ToleranceDescription = "±1%"; break;
                case ("G"): c.Tolerance = 2; c.ToleranceDescription = "±2%"; break;
                case ("R"): c.Tolerance = 2.5; c.ToleranceDescription = "±2.5%"; break;
                case ("J"): c.Tolerance = 5; c.ToleranceDescription = "±5%"; break;
                case ("K"): c.Tolerance = 10; c.ToleranceDescription = "±10%"; break;
                case ("M"): c.Tolerance = 20; c.ToleranceDescription = "±20%"; break;
                default: throw new ArgumentException("Invalid Murata Part Number");
            }

            if (AllListPartNumberWithSim.ContainsKey(partNumber))
            {
                c.SimDescription = "Vendor Simulation Data";
                c.SimKind = "General";
                c.SimSubKind = "Spice Subcircuit";
                c.SimSpicePrefix = "X";
                c.SimNetlist = "@DESIGNATOR %1 %2 @MODEL";
                c.SimPortMap = "(1:1),(2:2)";
                //c.SimFile = @"Basic\Simulation\Murata\" + typecode + ".ckt";
                c.SimFile = @"Basic\Simulation\Murata\" + AllListPartNumberWithSim[partNumber];
                c.SimModel = partNumber;
                c.SymbolName = "CAPACITOR_SIM";
            }

            return (c, isValid);
        }

        public static void DecodeInductor()
        {

        }

        public static string FromPackageCode(string packageCode)
        {
            switch (packageCode)
            {
                case ("01"): return "008004";
                case ("02"): return "01005";
                case ("MD"): return "015008";
                case ("0D"): return "015015";
                case ("03"): return "0201";
                case ("04"): return "03015";
                case ("05"): return "0202";
                case ("08"): return "0303";
                case ("1U"): return "02404";
                case ("15"): return "0402";
                case ("18"): return "0603";
                case ("JN"): return "0704";
                case ("21"): return "0805";
                case ("2B"): return "0806"; // ??
                case ("2M"): return "0806"; // ??
                case ("2U"): return "1008"; // ??
                case ("2H"): return "1008"; // ??
                case ("22"): return "1111";
                case ("31"): return "1206";
                case ("32"): return "1210";
                case ("3N"): return "1212";
                case ("42"): return "1808";
                case ("43"): return "1812";
                case ("44"): return "1515"; // ??
                case ("5B"): return "2020";
                case ("52"): return "2211";
                case ("55"): return "2220";
                case ("66"): return "2525";
                default: throw new ArgumentException("Invalid Murata Part Number");
            }
        }

        public static double FromValueCode3(string valueCode, char sep = 'R')
        {
            if (valueCode.Length != 3)
                throw new ArgumentException("Value code has to be three digits");
            else
            {
                if (valueCode[0] == sep)
                {
                    return valueCode.Substring(1, 2).ToDouble() / 100;
                }
                else if (valueCode[1] == sep)
                {
                    return (valueCode.Substring(0, 1) + valueCode.Substring(2, 1)).ToDouble() / 10;
                }
                else
                {
                    return valueCode.Substring(0, 2).ToDouble() * Math.Pow(10, valueCode.Substring(2, 1).ToInt32());
                }
            }
        }
    }
}
