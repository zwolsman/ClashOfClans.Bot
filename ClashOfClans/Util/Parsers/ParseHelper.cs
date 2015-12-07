using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Util.Parsers
{
    internal static class ParseHelper
    {
        private static readonly Dictionary<Type, IParseConverter> _converters;
        private static object _lock;

        static ParseHelper()
        {
            _converters = new Dictionary<Type, IParseConverter>();
            _lock = new object();
        }

        private static IParseConverter GetParseConverter(Type type)
        {
            if (_converters.ContainsKey(type))
                return _converters[type];

            lock(_lock)
            {
                if (_converters.ContainsKey(type))
                    return _converters[type];

                var converter = (IParseConverter)Activator.CreateInstance(type);
                _converters[type] = converter;

                return converter;
            }
        }

        public static IEnumerable<ParseProperty> GetParsingProperties(Type type)
        {
            SpreadSheetParseAttribute attribute = null;
            return type.GetProperties()                    // get all public properties
                        .Where(p => TryGetAttribute(p, out attribute))          // those who have the attribute
                        .Select(p => new ParseProperty
                        {
                            Property = p,
                            Attribute = attribute
                        });
        }

        public static void SetConvertedValue(ParseProperty parsingProperty, object obj, string value)
        {
            var converterType = parsingProperty.Attribute.Converter;
            var targetType = parsingProperty.Property.PropertyType;

            object convertedValue;

            if (converterType == null)
            {
                convertedValue = Convert.ChangeType(value, targetType);
            }
            else
            {
                if(!converterType.GetInterfaces().Contains(typeof(IParseConverter)))
                    throw new InvalidOperationException("Converter doesn't implement IParseConverter");

                var converter = GetParseConverter(converterType);
                convertedValue = converter.ConvertFrom(value);
            }

            parsingProperty.Property.SetValue(obj, convertedValue);
        }

        //public static IEnumerable<string> GetParsingPropertiesNames(IEnumerable<ParseProperty> parsingProperties)
        //    => parsingProperties.Select(parsingProperty =>
        //        parsingProperty.Attribute.ColumnName ?? parsingProperty.Property.Name);

        private static bool TryGetAttribute<T>(MemberInfo memberInfo, out T customAttribute) where T : Attribute
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            if (attributes == null)
            {
                customAttribute = null;
                return false;
            }

            customAttribute = (T)attributes;
            return true;
        }
    }

    internal class ParseProperty
    {
        public PropertyInfo Property { get; set; }
        public SpreadSheetParseAttribute Attribute { get; set; }
        public string ColumnName
            => Attribute.ColumnName ?? Property.Name;
    }

}
