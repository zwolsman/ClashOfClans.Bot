using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClashOfClans.Buildings;

namespace ClashOfClans.Data
{
    internal interface IBuildRequirementsProvider
    {
        BuildRequirements GetBuildingData(BuildingType buildingType, int level);
    }
}
