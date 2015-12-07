using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClashOfClans.Buildings;
using ClashOfClans.Util;
using ClashOfClans.Util.Parsers;

namespace ClashOfClans.Data.ParseConverters
{
    internal sealed class BuildingClassConverter : IParseConverter
    {
        public object ConvertFrom(string value)
        {
            BuildingClass buildingClass;
            return Enum.TryParse(value.RemoveSpaces(), out buildingClass)
                ? buildingClass
                : BuildingClass.Unkown;
        }
    }
}
