using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combo
{
    public enum ResistorSize : uint
    {
        RES01005,
        RES0201,
        RES0402,
        RES0603,
        RES0805,
        RES1206,
        RES1210,
        RES1812,

    }

    [DesignerCategory("Code")]
    public class Resistor : Component
    {
    }
}
