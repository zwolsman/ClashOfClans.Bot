using ClashOfClans.Data.ParseConverters;
using ClashOfClans.Util.Parsers;

namespace ClashOfClans.Buildings
{
    internal sealed class Building
    {
        public BuildingType BuildingType { get; set; }
        public int Level { get; set; }

        [SpreadSheetParse]
        public string Name { get; set; }

        //[SpreadSheetParse(Converter = typeof (BuildingClassConverter))]
        //public BuildingClass BuildingClass { get; set; }
        
        //[SpreadSheetParse]
        //public int Hitpoint { get; set; }

        //[SpreadSheetParse]
        //public int RegenTime { get; set; }

        public BuildRequirements Requirements { get; set; }

// <-- TimeSpan
    }
}
