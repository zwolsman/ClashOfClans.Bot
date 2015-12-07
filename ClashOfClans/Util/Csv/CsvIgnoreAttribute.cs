using System;

namespace ClashOfClans.Util.Csv
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CsvIgnoreAttribute : Attribute
    {
    }
}