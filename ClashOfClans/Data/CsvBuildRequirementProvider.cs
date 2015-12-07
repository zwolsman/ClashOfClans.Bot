using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClashOfClans.Buildings;
using ClashOfClans.Properties;
using ClashOfClans.Util.Csv;
using ClashOfClans.Util.Parsers;

namespace ClashOfClans.Data
{
    internal sealed class CsvBuildRequirementProvider : IBuildRequirementsProvider
    {
        private readonly IEnumerable<BuildRequirements> _buildings;

        public CsvBuildRequirementProvider()
        {
            var buildingTable = new CsvTable(Resources.buildings, true);
            var parser = new ClashCsvParser(buildingTable);

            _buildings = parser.Parse<BuildRequirements>();
        }

        public BuildRequirements GetBuildingData(BuildingType buildingType, int level)
        {
            var buildings = _buildings.Where(b => b.Name.Replace(" ", "") == buildingType.ToString());
            var building = buildings.ElementAt(level);
            
            return building;
        }
    }
}
