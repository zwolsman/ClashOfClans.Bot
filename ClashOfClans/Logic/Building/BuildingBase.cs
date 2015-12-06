using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClashOfClans.Data.Csv;
using ClashOfClans.Properties;
using ClashOfClans.Util;

namespace ClashOfClans.Logic.Building
{
    public abstract class BaseBuilding
    {
        public UpgradeRequirements Requirements { get; }
        public BuildingProperties Properties { get; set; }

        protected BaseBuilding(UpgradeRequirements requirements, BuildingProperties props)
        {
            Requirements = requirements;
            Properties = props;
        }

        public override string ToString()
        {
            return $"{Properties.Name} level {Properties.Level}";
        }
    }

    public interface IRequirementsProvider
    {
        UpgradeRequirements GetRequirements(BuildingType type, int level); // dit vind ik niet verkeerd
    }

    public interface IPropertyProvider
    {
        BuildingProperties GetBuildingProperties(BuildingType type, int level);
    }

    public class CsvProvider : IRequirementsProvider, IPropertyProvider
    {

        public UpgradeRequirements GetRequirements(BuildingType type, int level)
        {
            var reqs = new UpgradeRequirements();
            var row = CsvHelper.FindData(type, level);
            if (row != null)
            {
                // parse them rowssThem

                int days;
                int hours;
                int minutes;

                int.TryParse(row[9].ToString(), out days);
                int.TryParse(row[10].ToString(), out hours);
                int.TryParse(row[11].ToString(), out minutes);
                reqs.BuildingTime = new TimeSpan(days, hours, minutes, 0);

                reqs.ResourceType =
                    (ResourceType) Array.IndexOf(Enum.GetNames(typeof (ResourceType)), row[12].ToString()) + 3000001;
                reqs.BuildCost = int.Parse(row[13].ToString());

            }

            return reqs;
        }


        public BuildingProperties GetBuildingProperties(BuildingType type, int level)
        {
            var props = new BuildingProperties();
            props.Level = level;
            var row = CsvHelper.FindData(type, 0);
            if (row != null)
            {
                props.Name = row[0].ToString();

            }
            return props;
        }
    }

    public static class CsvHelper
    {
        private static readonly CsvTable buildingTable = new CsvTable(Resources.buildings, true);
        public static object[] FindData(BuildingType buildingType, int level)
        {
            int id = -1;
            level += 1;
            for (int i = 0; i < buildingTable.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(buildingTable.Rows[i].ItemArray[0].ToString()))
                {
                    id++;
                }

                if (id == (int)buildingType - 1000000)
                {
                    object[] baseRow = buildingTable.Rows[i].ItemArray;

                    object[] specificRow = buildingTable.Rows[i + level].ItemArray;
                    for (int j = 0; j < baseRow.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(specificRow[j]?.ToString()))
                        {
                            baseRow[j] = specificRow[j];
                        }
                    }

                        
                    return baseRow;
                }
            }
            return null;
        }
    }

    public static class BuildingFactory
    {
        private static Dictionary<BuildingType, Type> _buildingTypes = new Dictionary<BuildingType, Type>()
        {
            // replace with reflection magic
            [BuildingType.TownHall] = typeof(TownHallBuilding)
        }; 

        public static IRequirementsProvider RequirementsProvider { get; set; }
        public static IPropertyProvider PropertyProvider { get; set; }

        public static BaseBuilding Create(BuildingType buildingType, int level)
        {
            var type = typeof (TownHallBuilding);//_buildingTypes[buildingType];
           /* var constructorInfo = type.GetConstructor(BindingFlags.Public, null, new[] {typeof (UpgradeRequirements)},
                null);

            var creator = Creator<BaseBuilding>.GetCreator(constructorInfo);
          
            return creator(RequirementsProvider);*/

            var requirements = RequirementsProvider.GetRequirements(buildingType, level);
            var properties = PropertyProvider.GetBuildingProperties(buildingType, level);

            var instance = (BaseBuilding)Activator.CreateInstance(type, new object[] {requirements, properties});
            

            return instance;
        }
    }

    public class TownHallBuilding : BaseBuilding
    {
        public TownHallBuilding(UpgradeRequirements requirements, BuildingProperties props) : base(requirements, props)
        {
        }
    }

    public enum BuildingType
    {
        TroopHousing = 1000001 - 1,
        TownHall = 1000002 - 1,
        ElixirPump = 1000003 - 1,
        ElixirStorage = 1000004 - 1,
        GoldMine = 1000005 - 1,
        GoldStorage = 1000006 - 1,
        Barrack= 1000007 - 1,
        Laboratory = 1000008 - 1,
        Cannon = 1000009 - 1,
        ArcherTower = 1000010 - 1,
        Wall = 1000011 - 1,
        WizardTower = 1000012 - 1,
        AirDefense = 1000013 - 1,
        Mortar = 1000014 - 1,
        AllianceCastle = 1000015 - 1,
        WorkerBuilding = 1000016 - 1,
        CommunicationsMast = 1000017 - 1,
        GoblinMainBuilding = 1000018 - 1,
        GoblinHut = 1000019 - 1,
        TeslaTower = 1000020 - 1,
        SpellForge = 1000021 - 1,
        Bow = 1000022 - 1,
        HeroAltarBarbarianKing = 1000023 - 1,
        DarkElixirPump = 1000024 - 1,
        DarkElixirStorage = 1000025 - 1,
        HeroAltarArcherQueen = 1000026 - 1,
        DarkElixirBarrack = 1000027 - 1,
        DarkTower = 1000028 - 1
    }

    public class UpgradeRequirements
    {
        public int TownHallLevel { get; set; }
        public TimeSpan BuildingTime { get; set; }
        public int BuildCost { get; set; }
        public ResourceType ResourceType { get; set; }
    }

    public class BuildingProperties
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public string Name { get; set; }
        public int Level { get; set; }

        // enzoo door
    }

    public interface IProducingBuilding
    {
        int ProducingPerHour { get; }
        int MaxResource { get; }

        ResourceType Resource { get; }
    }
    
}

