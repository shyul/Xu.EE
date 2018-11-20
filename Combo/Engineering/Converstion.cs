using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combo
{
    public enum ResistanceUnit
    {
        mR,
        R,
        K,
        M,
        G
    }

    public enum CapacitanceUnit
    {
        pF,
        nF,
        uF,
        mF,
        F
    }

    public static class Converstion
    {
        public static double GetResistance(string value, ResistanceUnit unit)
        {
            double c = Convert.ToDouble(value);

            switch (unit)
            {
                case ResistanceUnit.mR:
                    c = c / 1e3;
                    break;

                case ResistanceUnit.K:
                    c = c * 1e3;
                    break;

                case ResistanceUnit.M:
                    c = c * 1e6;
                    break;

                case ResistanceUnit.G:
                    c = c * 1e9;
                    break;
            }

            return c;
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


        public static double GetCapacitance(string value, CapacitanceUnit unit)
        {
            double c = Convert.ToDouble(value);

            switch (unit)
            {
                case CapacitanceUnit.nF:
                    c = c * 1e3;
                    break;

                case CapacitanceUnit.uF:
                    c = c * 1e6;
                    break;

                case CapacitanceUnit.mF:
                    c = c * 1e9;
                    break;

                case CapacitanceUnit.F:
                    c = c * 1e12;
                    break;
            }

            return c;
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
    }
}
