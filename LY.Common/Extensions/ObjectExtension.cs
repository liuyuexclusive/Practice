using System;
using System.Collections;
using System.Collections.Generic;

namespace LY.Common
{
    public static class ObjectExtension
    {
        public static IDictionary<string, object>  ToDic(this object obj)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            if (obj == null)
            {
                return result;
            }
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.Name.ToLower() == "id")
                {
                    continue;
                }
                var value = property.GetValue(obj);
                result.Add(property.Name, value);
            }
            return result;
        }
    }
}
