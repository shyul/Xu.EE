/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2018 Xu Li - shyu.lee@gmail.com
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xu;

namespace Xu.EE
{
    /// <summary>
    /// Value in pF
    /// </summary>
    [Serializable, DataContract]
    public class Capacitor : LumpedComponent
    {
        public Capacitor()
        {
            Level = 220;
            Unit = "pF";

            TableName = "Capacitor";
            TablePath = @"Basic\Basic.accdb";

            SymbolName = "CAPACITOR";
            SymbolPath = @"Basic\Basic.SchLib";

            FootprintPath = @"Basic\Capacitor.PcbLib";

            SimSubKind = "Capacitor";
        }

        [DataMember]
        public string MaterialTempCode { get; set; } // X7R, C0G, and so on...

        [DataMember]
        public double Voltage { get; set; }

        [IgnoreDataMember]
        public override string Name => Comment + "_" + Tolerance.ToString("0.#") + "%_" + Voltage + "V_" + MaterialTempCode;
        //public override string Name => PackageName + "_" + Comment + "_" + Tolerance.ToString("0.#") + "%_" + Voltage + "V_" + MaterialTempCode;

        [IgnoreDataMember]
        public override string Comment
        {
            get
            {
                if (Value >= 1e3 && Value < 1e6) return (Value / 1e3).ToString() + "nF";
                else if (Value >= 1e6 && Value < 1e9) return (Value / 1e6).ToString() + "uF";
                else if (Value >= 1e9 && Value < 1e12) return (Value / 1e9).ToString() + "mF";
                else if (Value >= 1e12) return (Value / 1e12).ToString() + "F";
                return Value.ToString() + "pF";
            }
        }

        [IgnoreDataMember]
        public override string Description => ("CAP," + MountType + "," + PackageName + "," + Comment.ToUpper() + "," + ToleranceDescription + "," +
            Voltage + "V," + MaterialTempCode + "," + TemperatureRange.ToStringShort() + "DEG(" + TempRangeType + ")," + Tag.ToUpper()).Trim(',');  // { get => base.Description; set => base.Description = value; }
    }




}
