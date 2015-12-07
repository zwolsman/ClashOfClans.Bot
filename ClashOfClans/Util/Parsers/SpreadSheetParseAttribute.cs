using System;

namespace ClashOfClans.Util.Parsers
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SpreadSheetParseAttribute : Attribute
    {
        /// <summary>
        /// <para>Cell name to check, if not set uses property name</para>
        /// <para>Note: this is case sensitive</para>
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// <para>Converter to convert the value to a type.</para>
        /// <para>The converter must implement IParseConverter</para>
        /// <para>Note: this property is a type i.e typeof(DateParseConverter)</para>
        /// </summary>
        public Type Converter { get; set; }
    }

    public interface IParseConverter
    {
        object ConvertFrom(string value);
    }
}
