using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Xu;
using Xu.EE;

namespace AltiumLib
{
    class Program
    {
        static void Main(string[] args)
        {
            string AppPath = AppDomain.CurrentDomain.BaseDirectory;
            string LibPath = AppPath + @"..\";

            Console.WriteLine(LibPath);

            //KOASpeer.InitiateAll();
            Murata.ImportAll(LibPath);
            ComponentList.WriteToFile(LibPath + @"Library\Basic\Basic.accdb");

            Console.ReadKey();
        }


    }
}
