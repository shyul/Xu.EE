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

    public class Capacitor : PCBComponent
    {
        // in pF / pico F
        public double Capacitance { get; private set; }
        public override string Comment => Converstion.CapacitanceToString(Capacitance);

        public double Size { get; private set; } // 01005, 0201, 0402, 0603, 0805, 1206, 1210 ...
        public double Voltage { get; private set; } // 4V, 6.3V, 10V, 16V, 25V, 50V, 100V
        public double Tolerance { get; private set; } // 0.1pF, 0.2pF, 1%, 2%, 5%
        public string Material { get; private set; } // X5R, X7R, C0G



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
