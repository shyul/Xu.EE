using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Combo
{
    /*
    public class PCBComponent
    {
        public string Designator;
        public string Footprint;

        public string FootprintRef;
        public string Type;
        public string Simulation;


    }*/

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum ComponentGrade : ulong
    {
        Industrial = 1,
        Extended = 2,
        AEC_Q100 = 4,
        AreoSpace = 8,
        Military = 16,
    }

    [Serializable, DataContract]
    public abstract class PCBComponent : IEquatable<PCBComponent>
    {
        public PCBComponent()
        {
            TempLow = 4;
            TempHigh = 40;
            IsSMT = false;
        }

        [DataMember, Browsable(true), ReadOnly(false)]
        public string SerialNumber { get; set; } = "10001001A-01"; // A: Rev Number, -01

        [DataMember, Browsable(true), ReadOnly(false)]
        [DisplayName("Vendor Name"), Description("Vendor Name")]
        public virtual string VendorName { get; private set; }

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual string PartNumber { get; private set; }

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual string Comment { get; private set; }

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual string Description { get; private set; }

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual double TempLow { get; private set; } = 0; // 0

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual double TempHigh { get; private set; } = 40; // 40

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual ComponentGrade Grade { get; private set; } = 0;

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual HashSet<string> Tags { get; private set; } = new HashSet<string>();

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual string SchSymbolName { get; private set; }

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual string SchLibPath { get; private set; }

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual string Designator { get; private set; }

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual string FootprintName { get; private set; }

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual string PcbLibPath { get; private set; }

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual double Height { get; private set; }

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual bool IsSMT { get; private set; }

        [DataMember, Browsable(true), ReadOnly(false)]
        public virtual string IsSMTString => IsSMT ? "SMD" : "TH";

        #region Equality
        public bool Equals(PCBComponent other) => (GetHashCode() == other.GetHashCode());
        public override bool Equals(object other) => (GetType() == other.GetType()) && (GetHashCode() == other.GetHashCode());
        public override int GetHashCode() => VendorName.GetHashCode() ^ PartNumber.GetHashCode();
        public static bool operator ==(PCBComponent s1, PCBComponent s2) => s1.Equals(s2);
        public static bool operator !=(PCBComponent s1, PCBComponent s2) => !s1.Equals(s2);
        #endregion Equality
    }


}
