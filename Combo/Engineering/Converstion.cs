using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combo
{
    public static class Converstion
    {
        public static double StringToCapacitance(string values, string unit)
        {

            return 0;
        }

        // Convert from pF
        public static string CapacitanceToString(double value)
        {
            if (value < 1e3)
                return value.ToString() + " pF";
            else if (value >= 1e3 && value < 1e6)
                return (value / 1e3).ToString() + " nF";
            else if (value >= 1e6 && value < 1e9)
                return (value / 1e6).ToString() + " uF";
            else if (value >= 1e9 && value < 1e12)
                return (value / 1e9).ToString() + " mF";
            else
                return (value / 1e12).ToString() + " F";
        }

        public static double StringToResistance(string values, string unit)
        {

            return 0;
        }

        // Convert from Ohm
        public static string ResistanceToString(double value)
        {
            if (value < 1)
                return (value * 1000).ToString() + " mR";
            else if (value >= 1 && value < 1e3)
                return value.ToString() + " R";
            else if (value >= 1e3 && value < 1e6)
                return (value / 1e3).ToString() + " K";
            else if (value >= 1e6 && value < 1e9)
                return (value / 1e6).ToString() + " M";
            else
                return (value / 1e9).ToString() + " G";
        }
    }
}
