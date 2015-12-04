using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Data.Csv
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CsvPropertyAttribute : Attribute
    {
        public CsvPropertyAttribute(string name)
        {
            PropertyName = name;
        }
        public string PropertyName { get; set; }

    }
}
