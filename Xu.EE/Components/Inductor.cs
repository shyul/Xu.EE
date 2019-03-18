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
    [Serializable, DataContract]
    public enum InductorType
    {
        FILM,
        MULTILAYER,
        WIRE_WOUND,
        FERRITE_WIRE_WOUND,
        AIR_COIL,
        FERRITE_COIL,
        HIGH_POWER
    }

    /// <summary>
    /// Value in nH
    /// </summary>
    public class Inductor : LumpedComponent
    {
        public Inductor()
        {
            Level = 230;
            Unit = "nH";

            TableName = "Inductor";
            TablePath = @"Basic\Basic.accdb";

            SymbolName = "INDUCTOR";
            SymbolPath = @"Basic\Basic.SchLib";

            FootprintPath = @"Basic\Inductor.PcbLib";

            SimSubKind = "Inductor";
        }

        [DataMember]
        public InductorType InductorType { get; set; } = InductorType.WIRE_WOUND;

        [DataMember]
        public double Resistance { get; set; } = -1;

        [DataMember]
        public double SRF { get; set; } = -1; // MHz

        [DataMember]
        public double TempCurrentRating { get; set; } = -1;

        [DataMember]
        public double SatCurrentRating { get; set; } = -1;

        [IgnoreDataMember]
        public override string Name
        {
            get
            {
                if (SRF > 100)
                    return Comment + "_" + Tolerance.ToString("0.#") + "%_" + TempCurrentRating + "A_" + SRF / 1000 + "GHz";
                else
                    return Comment + "_" + Tolerance.ToString("0.#") + "%_" + TempCurrentRating + "A_" + SRF + "MHz";
            }
        }

        [IgnoreDataMember]
        public override string Comment
        {
            get
            {
                if (Value >= 1e3 && Value < 1e6) return (Value / 1e3).ToString() + "uH";
                else if (Value >= 1e6 && Value < 1e9) return (Value / 1e6).ToString() + "mH";
                else if (Value >= 1e9) return (Value / 1e9).ToString() + "H";
                return Value.ToString() + "nH";
            }
        }

        [IgnoreDataMember]
        public override string Description => ("IND," + MountType + "," + PackageName + "," + Comment.ToUpper() + "," + ToleranceDescription + "," +
            Voltage + "V," + MaterialTempCode + "," + TemperatureRange.ToStringShort() + "DEG(" + TempRangeType + ")," + Tag.ToUpper()).Trim(',');

    }
}
