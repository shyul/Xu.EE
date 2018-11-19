using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combo
{
    public enum CapcitorSize : uint
    {
        CAP01005,
        CAP0201,
        CAP0402,
        CAP0603,
        CAP0805,
        CAP1206,
        CAP1210,
        CAP1812,

    }

    public class Capacitor : Component
    {
        // in pF / pico F
        public double Capacitance { get; private set; }
        public double Voltage { get; private set; }
        public double Tolerance { get; private set; }
        public string Material { get; private set; }

        public override string Comment => Converstion.CapacitanceToString(Capacitance);

        public override string Description
        {
            get {
                return "CAP," + IsSMTString + "," + Comment;
            }
        }

    }

    public class CapacitorMuRata : Capacitor
    {



    }

    public class CapacitorTDK : Capacitor
    {



    }

    public class CapacitorVenkel : Capacitor
    {



    }

    public class CapacitorPanasonic : Capacitor
    {



    }
}
