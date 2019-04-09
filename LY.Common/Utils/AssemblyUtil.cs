using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LY.Common
{
    public static class AssemblyUtil
    {
        public static IEnumerable<Type> DomainTypes
        { 
            get
            {
                var assembly = Assembly.Load(new AssemblyName("LY.Domain"));
                var types = assembly.ExportedTypes.Where(x => typeof(Entity).IsAssignableFrom(x));
                return types;
            }
        }
    }
}
