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

namespace Xu.EE
{
    public static class KOASpeer
    {
        public static void InitiateAll()
        {
            InitiateEmptyTables();
            RK73H_Generator();
        }

        public static DataTable InitiateEmptyTable(string packageCode)
        {
            string packageName = Packages[packageCode].packageName;
            DataTable dt = AccessDb.GetTable("Resistor - " + packageName);
            dt.Rows.Add(new object[] { "NC_" + packageName, "N/A", "NC", "1/GMIN", packageName, "RES,SM," + packageName + ",NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "RESISTOR", @"Basic\Basic.SchLib", packageName +"_NC", @"Basic\Resistor.PcbLib",
                "Ideal Simulation Data", "General", "Resistor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            /*dt.Rows.Add(new object[] { "0R_" + Packages[packageCode].current + "A", "210-000000A-01", "0R", "0.05", packageName,
                "RES,SM," + packageName + ",0.05R MAX," + Packages[packageCode].current + "A,THICK_FILM,-55~" + ((packageName == "01005") ? "+125" : "+155") + "DEG(TJ),AEC-Q200",
                "KOA Speer", "RK73Z" + packageCode + "T" + Packages[packageCode].pack,
                "RESISTOR", @"Basic\Basic.SchLib", packageName + ((packageName == "0201" || packageName == "0402") ? "_GREEN" : string.Empty), @"Basic\Resistor.PcbLib",
                "Ideal Simulation Data", "General", "Resistor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });*/
            dt.AcceptChanges();
            return dt;
        }

        public static void InitiateEmptyTables()
        {
            foreach (string p in Packages.Keys)
            {
                DataTable dt = InitiateEmptyTable(p);
                ComponentList.Accdb_Basic.Add(dt.TableName, dt);
            }
        }

        public static Resistor GetResistor(string packageCode)
        {
            Resistor r = new Resistor()
            {
                VendorName = "KOA Speer",

                Tolerance = 1,
                Voltage = Packages[packageCode].voltage,
                PackageName = Packages[packageCode].packageName,
                PowerRating = Packages[packageCode].power,

                TableName = "Resistor - " + Packages[packageCode].packageName,
            };
            r.Tags.Add("AEC-Q200");
            return r;
        }

        public static void RK73H_Generator()
        {
            Dictionary<string, double> values = new Dictionary<string, double>();

            foreach (string p in Packages.Keys)
            {
                values.Clear();
                string PackageName = Packages[p].packageName;

                ResistorZ rz = new ResistorZ(p);
                ComponentList.Resistors.InsertCmp(rz);

                switch (PackageName)
                {
                    case ("01005"):
                        {
     
                            Decade.ValueGenerator(values, Decade.E24Values, new Range<double>(10, 2e6));
                            foreach (string val in values.Keys)
                            {
                                Resistor r = GetResistor(p);
                                r.TemperatureRange = new Range<double>(-55, 125);
                                r.FootprintName = "01005";
                                r.Value = values[val];
                                r.VendorPartNumber = "RK73H" + p + "T" + Packages[p].pack + val + "F";

                                if (r.Value >= 10 && r.Value <= 91000)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                else if (r.Value > 100000 && r.Value <= 2000000)
                                {
                                    r.TemperatureCoefficient = 250;
                                }
                                ComponentList.Resistors.InsertCmp(r);
                            }
                        }
                        break;
                    case ("0201"):
                        {
                            Decade.ValueGenerator(values, Decade.E24Values, new Range<double>(1, 9.1));
                            Decade.ValueGenerator(values, Decade.E96Values, new Range<double>(10, 1e6 - 1));
                            Decade.ValueGenerator(values, Decade.E24Values, new Range<double>(1e6, 1e7));
                            foreach (string val in values.Keys)
                            {
                                Resistor r = GetResistor(p);
                                r.FootprintName = "0201";
                                r.Value = values[val];
                                r.VendorPartNumber = "RK73H" + p + "T" + Packages[p].pack + val + "F";
                                if (r.Value >= 1 && r.Value < 10)
                                {
                                    r.TemperatureCoefficient = 400;
                                }
                                else
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                ComponentList.Resistors.InsertCmp(r);
                            }
                        }
                        break;
                    case ("0402"):
                        {
                            Decade.ValueGenerator(values, Decade.E96Values, new Range<double>(1, 1e7));
                            foreach (string val in values.Keys)
                            {
                                Resistor r = GetResistor(p);
                                r.FootprintName = r.PackageName + "_BLUE";
                                r.Value = values[val];
                                r.VendorPartNumber = "RK73H" + p + "T" + Packages[p].pack + val + "F";
                                if (r.Value >= 1 && r.Value < 10)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                else if (r.Value >= 10 && r.Value <= 1000000)
                                {
                                    r.TemperatureCoefficient = 100;
                                }
                                else if (r.Value > 1000000)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                ComponentList.Resistors.InsertCmp(r);
                            }
                        }
                        break;
                    case ("0603"):
                        {
                            Decade.ValueGenerator(values, Decade.E96Values, new Range<double>(1, 1e7));
                            foreach (string val in values.Keys)
                            {
                                Resistor r = GetResistor(p);
                                r.FootprintName = r.PackageName + "_BLUE";
                                r.Value = values[val];
                                r.VendorPartNumber = "RK73H" + p + "T" + Packages[p].pack + val + "F";
                                if (r.Value >= 1 && r.Value < 10)
                                {
                                    r.TemperatureCoefficient = 200;
                                    r.PowerRating = 0.125;
                                }
                                else if (r.Value >= 10 && r.Value <= 1000)
                                {
                                    r.TemperatureCoefficient = 100;
                                    r.PowerRating = 0.125;
                                }
                                else if (r.Value > 1000 && r.Value <= 1000000)
                                {
                                    r.TemperatureCoefficient = 100;
                                    r.PowerRating = 0.1;
                                }
                                else if (r.Value > 1000000)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                ComponentList.Resistors.InsertCmp(r);
                            }
                        }
                        break;
                    case ("0805"):
                        {
                            Decade.ValueGenerator(values, Decade.E96Values, new Range<double>(1, 1e7));
                            foreach (string val in values.Keys)
                            {
                                Resistor r = GetResistor(p);
                                r.FootprintName = r.PackageName + "_BLUE";
                                r.Value = values[val];
                                r.VendorPartNumber = "RK73H" + p + "T" + Packages[p].pack + val + "F";
                                if (r.Value >= 1 && r.Value < 10)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                else if (r.Value >= 10 && r.Value <= 1000000)
                                {
                                    r.TemperatureCoefficient = 100;
                                }
                                else if (r.Value > 1000000)
                                {
                                    r.TemperatureCoefficient = 400;
                                }
                                ComponentList.Resistors.InsertCmp(r);
                            }
                        }
                        break;
                    case ("1206"):
                    case ("1210"):
                    case ("2010"):
                    case ("2512"):
                        {
                            Decade.ValueGenerator(values, Decade.E96Values, new Range<double>(1, 1e7));
                            foreach (string val in values.Keys)
                            {
                                Resistor r = GetResistor(p);
                                r.FootprintName = r.PackageName + "_BLUE";
                                r.Value = values[val];
                                r.VendorPartNumber = "RK73H" + p + "T" + Packages[p].pack + val + "F";
                                if (r.Value >= 1 && r.Value < 10)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                else if (r.Value >= 10 && r.Value <= 1000000)
                                {
                                    r.TemperatureCoefficient = 100;
                                }
                                else if (r.Value > 1000000 && r.Value <= 5600000)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                else
                                {
                                    r.TemperatureCoefficient = 400;
                                }
                                ComponentList.Resistors.InsertCmp(r);
                            }
                        }
                        break;
                }
            }
        }

        public static readonly Dictionary<string, (string packageName, string pack, double power, double voltage, double current)> Packages =
            new Dictionary<string, (string packageName, string pack, double power, double voltage, double current)>()
                {
                    { "1F", ("01005", "TBL", 0.03, 30, 0.5) },
                    { "1H", ("0201", "TC", 0.05, 50, 0.5) },
                    { "1E", ("0402", "TP", 0.1, 75, 1) },
                    { "1J", ("0603", "TD", 0.1, 75, 1) },
                    { "2A", ("0805", "TD", 0.25, 150, 2) },
                    { "2B", ("1206", "TD", 0.25, 200, 2) },
                    { "2E", ("1210", "TD", 0.5, 200, 2) },
                    { "2H", ("2010", "TE", 0.75, 200, 2) },
                    { "3A", ("2512", "TE", 1, 200, 2) },
                };


    }

    [Serializable, DataContract]
    public class ResistorZ : Resistor
    {
        public ResistorZ(string packageCode)
        {
            Level = 210;
            Variation = (1, string.Empty);

            PackageName = KOASpeer.Packages[packageCode].packageName;
            CurrentRating = KOASpeer.Packages[packageCode].current;

            Value = 0;

            Unit = string.Empty;
            Tolerance = 1;

            TemperatureRange = new Range<double>(-55, 155);

            VendorName = "KOA Speer";
            VendorPartNumber = "RK73Z" + packageCode + "T" + KOASpeer.Packages[packageCode].pack;

            SymbolName = "RESISTOR";
            SymbolPath = @"Basic\Basic.SchLib";

            FootprintPath = @"Basic\Resistor.PcbLib";
            FootprintName = PackageName + ((PackageName == "0201" || PackageName == "0402") ? "_GREEN" : string.Empty);

            PackageName = KOASpeer.Packages[packageCode].packageName;
            PowerRating = KOASpeer.Packages[packageCode].power;

            TableName = "Resistor - " + KOASpeer.Packages[packageCode].packageName;

            SimDescription = "Ideal Simulation Data";
            SimKind = "General";
            SimSubKind = "Resistor";
            SimSpicePrefix = "X";
            SimNetlist = string.Empty;
            SimPortMap = "(1:1),(2:2)";
            SimFile = string.Empty;
            SimModel = "Ideal";
        }

        public override string Name => Comment + "_" + CurrentRating + "A";

        public override string Description => "RES,SM," + PackageName + ",0.05R MAX," + CurrentRating + "A,THICK_FILM,-55~" + ((PackageName == "01005") ? "+125" : "+155") + "DEG(TJ),AEC-Q200";

        [DataMember]
        public double CurrentRating { get; set; } = -1;
    }
}
