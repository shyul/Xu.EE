using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combo
{
    /*
    public class PCBComponent
    {
        public string Designator;
        public string Footprint;
        public string FootprintPath;
        public string FootprintRef;
        public string Type;
        public string Simulation;
        public string PartNumber;
        public string Description;
    }*/

    [DesignerCategory("Code")]
    public abstract class PCBComponent : IEquatable<PCBComponent>
    {
        public PCBComponent()
        {
            TempLow = 4;
            TempHigh = 40;
            IsSMT = false;
        }

        public virtual string Vendor { get; private set; }
        public virtual string PartNumber { get; private set; }

        public virtual string Comment { get; private set; }
        public virtual string Description { get; private set; }

        public virtual double TempLow { get; private set; } // 0
        public virtual double TempHigh { get; private set; } // 40
        public virtual HashSet<string> Tags { get; private set; }

        public virtual string SchematicSymbol { get; private set; }
        public virtual string SchematicLibPath { get; private set; }
        public virtual string Designator { get; private set; }

        public virtual string Footprint { get; private set; }
        public virtual string FootprintLibPath { get; private set; }
        public virtual bool IsSMT { get; private set; }
        public virtual string IsSMTString => IsSMT ? "SMD" : "TH";

        public bool Equals(PCBComponent other) => (GetHashCode() == other.GetHashCode());
        public override bool Equals(object other) => (GetType() == other.GetType()) && (GetHashCode() == other.GetHashCode());
        public override int GetHashCode() => Vendor.GetHashCode() ^ PartNumber.GetHashCode();
    }


}
