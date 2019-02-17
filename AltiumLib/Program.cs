using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xu;
using Xu.EE;

namespace AltiumLib
{
    class Program
    {
        static void Main(string[] args)
        {
            KOASpeer.InitiateAll(@"E:\Basic.accdb");
            Murata.ImportAll(@"E:\Basic.accdb");
        }
    }
}
