using System;

namespace ClashOfClans.Util.Csv
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CsvPropertyAttribute : Attribute
    {
        public CsvPropertyAttribute(string name)
        {
            PropertyName = name;
        }

        public string PropertyName { get; set; }
    }
}