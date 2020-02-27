using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Channels;


namespace MyJsonLib
{
    public static class MyJsonConverter
    {
        public static string Serialize<T>(T obj)
        {
            Type t = obj.GetType();

            PropertyInfo[] info = t.GetProperties();
            StringBuilder sb = new StringBuilder("{");

            foreach (var propinfo in info)
            {
                if (propinfo.PropertyType.IsGenericType && propinfo.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var value = propinfo.GetValue(obj) as IEnumerable;

                    sb.Append($"\"{propinfo.Name}\":");
                    sb.Append("[");

                    foreach (var item in value)
                    {
                        if (item is string)
                        {
                            sb.Append($"\"{item}\"");
                        }
                        else
                        {
                            sb.Append($"{item}");
                        }

                        sb.Append(",");
                    }

                    if (sb[sb.Length-1] == ',')
                    {
                        sb.Length--;
                    }
                    
                    sb.Append("]");

                }
                else

                sb.Append(propinfo.PropertyType == typeof(string)
                    ? $"\"{propinfo.Name}\":\"{propinfo.GetValue(obj)}\""
                    : $"\"{propinfo.Name}\":{propinfo.GetValue(obj)}");
                sb.Append(",");
            }

            sb.Length--;
            sb.Append("}");

            return sb.ToString();
        }


        //public static T Deserialize<T>(string json)
        //{

        //}
    }
}
