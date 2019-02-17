/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2018 Xu Li - shyu.lee@gmail.com
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xu;

namespace Xu.EE
{
    public static class ComponentList
    {
        // Description as key and VendorName+VendorPartNumber as Hash
        public static Dictionary<string, HashSet<Capacitor>> Capacitors = new Dictionary<string, HashSet<Capacitor>>();


        public static Dictionary<string, HashSet<Resistor>> Resistors = new Dictionary<string, HashSet<Resistor>>();


        public static Dictionary<string, HashSet<Inductor>> Inductors = new Dictionary<string, HashSet<Inductor>>();



    }
}
