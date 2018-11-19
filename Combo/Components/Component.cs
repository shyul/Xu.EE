using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combo
{
    [DesignerCategory("Code")]
    public class Component
    {
        public virtual string Vendor { get; private set; }
        public virtual string PartNumber { get; private set; }



        public virtual string Designator { get; private set; }

        public virtual string Footprint { get; private set; }
        public virtual bool IsSMT { get; private set; }

        public virtual string IsSMTString => IsSMT ? "SM" : "TH";

        public virtual string SymbolPath { get; private set; }
        public virtual string FootprintPath { get; private set; }

        public virtual HashSet<string> Tags { get; private set; }

        public virtual string Comment { get; private set; }
        public virtual string Description { get; private set; }


    }


}
