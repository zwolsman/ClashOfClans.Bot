using ClashOfClans.Buildings;

namespace ClashOfClans.Data
{
    internal interface IBuildingProvider
    {
        Building GetBuildingData(BuildingType buildingType, int level);
    }
}
