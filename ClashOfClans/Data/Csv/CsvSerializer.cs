using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Data.Csv
{

    //TODO
    /*class CsvSerializer
    {
        public static object[] Deserialize(CsvTable table, Type objectType)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            if (objectType == null)
                throw new ArgumentNullException("objectType");
            if (!objectType.IsSubclassOf(typeof(CoCData)))
                throw new ArgumentException("objectType is not a subclass of type CoCData.");

            var rows = table.Rows;
            var properties = objectType.GetProperties();
            var parentObj = (object)null;
            var objList = new List<object>();
            var id = -1;

            for (int x = 0; x < rows.Count; x++)
            {
                var childObj = Activator.CreateInstance(objectType);
                for (int i = 0; i < properties.Length; i++) // set property value loop
                {
                    var property = properties[i];
                    if (property.DeclaringType != objectType)
                    {
                        property.SetMethod.Invoke(childObj, new object[] { id });
                        continue;
                    }

                    if (HasIgnoreAttribute(property))
                        continue; // ignore CsvIgnoreAttribute

                    var propertyName = GetPropertyAttributeName(property);
                    var value = rows[x][propertyName];
                    var parameters = new object[] { value };

                    if (parentObj != null && value == DBNull.Value) // get data from parent
                        parameters = new object[] { property.GetMethod.Invoke(parentObj, null) };
                    else if (value == DBNull.Value)
                        continue; // keep default value

                    var isParent = property.Name == "Name" && value != DBNull.Value;
                    property.SetMethod.Invoke(childObj, parameters);

                    if (isParent)
                    {
                        id += 1;
                        parentObj = childObj;
                    }
                }
                objList.Add(childObj);
            }
            return objList.ToArray();
        }

        public static T[] Deserialize<T>(CsvTable table)
        {
            var tType = typeof(T);
            var objs = Deserialize(table, tType);
            return Array.ConvertAll(objs, obj => (T)obj);
        }

        public static CsvTable Serialize(object[] objArray)
        {
            throw new NotImplementedException();
        }

        private static string GetPropertyAttributeName(PropertyInfo property)
        {
            var propertyAttribute = (CsvPropertyAttribute)property.GetCustomAttributes(typeof(CsvPropertyAttribute), false)
                                                                          .FirstOrDefault();
            return propertyAttribute == null ? property.Name : propertyAttribute.PropertyName;
        }

        private static bool HasIgnoreAttribute(PropertyInfo property)
        {
            var ignoreAttribute = property.GetCustomAttributes(typeof(CsvIgnoreAttribute), false);
            if (ignoreAttribute.Length > 0)
                return true;
            return false;
        }
    }*/
}
