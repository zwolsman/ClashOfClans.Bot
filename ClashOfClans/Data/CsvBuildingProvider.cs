using System.Collections.Generic;
using System.Linq;
using ClashOfClans.Buildings;
using ClashOfClans.Properties;
using ClashOfClans.Util.Csv;
using ClashOfClans.Util.Parsers;

namespace ClashOfClans.Data
{
    internal sealed class CsvBuildingProvider : IBuildingProvider
    {
        private readonly IEnumerable<Building> _buildings; 

        public CsvBuildingProvider()
        {
            var buildingTable = new CsvTable(Resources.buildings, true);
            var parser = new ClashCsvParser(buildingTable);

            _buildings = parser.Parse<Building>();
        }

        public Building GetBuildingData(BuildingType buildingType, int level)
        {
            var buildings = _buildings.Where(b => b.Name.Replace(" ", "") == buildingType.ToString());
            var building = buildings.ElementAt(level);

            building.Level = level;
            building.BuildingType = buildingType;
            return building;
        }
    }
}
