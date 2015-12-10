using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ClashOfClans.Util.Csv;
using ClashOfClans.Logic.Building;
using ClashOfClans.Networking;
using ClashOfClans.Properties;
using log4net;
using ClashOfClans.Util;
using Ionic.Zlib;

namespace ClashOfClans.Logic
{
    public class Village
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Village));
        

        public void Read(ClashBinaryReader reader)
        {
            var homeData = reader.ReadByteArray();
            if (homeData == null)
                return;
            using (var binaryReader = new BinaryReader(new MemoryStream(homeData)))
            {
                
                var decompressedLength = binaryReader.ReadInt32();
                var compressedJson = binaryReader.ReadBytes(homeData.Length - 4);
                
                var json = ZlibStream.UncompressString(compressedJson);
                if (decompressedLength != json.Length)
                    if (decompressedLength - 1 != json.Length) // to prevent for running in error
                        throw new InvalidDataException(string.Format("Json length is not valid. {0} != {1}.", decompressedLength, json.Length));


                dynamic test = new JavaScriptSerializer().Deserialize<dynamic>(json);
                CsvProvider provider = new CsvProvider();
                BuildingFactory.RequirementsProvider = provider;
                BuildingFactory.PropertyProvider = provider;

                foreach (var building in test["buildings"])
                {
                    int id = int.Parse(building["data"].ToString());
                    BuildingType buildingType = (BuildingType)id;
                    int level = int.Parse(building["lvl"].ToString());

                    if (buildingType == BuildingType.GoldMine || buildingType == BuildingType.ElixirPump)
                    {
                        //TODO refactor shit
                        double[] collector_max = { 500, 1000, 1500, 2500, 10000, 20000, 30000, 50000, 75000, 100000, 150000, 200000 };
                        double[] collector_ph = { 200, 400, 600, 800, 1000, 1300, 1600, 1900, 2200, 2500, 3000, 3500 };
                        int collectedTime = int.Parse(building["res_time"].ToString());
                        var _timedif = (collector_max[level] / collector_ph[level] * 3600) - collectedTime;
                        int collected = (int)Math.Round(_timedif * (collector_ph[level] / 3600));

                        //logger.Debug(building);
                        logger.DebugFormat("Type: {0}, res_time: {1}, level: {2}, collected: {3}", buildingType, building["res_time"], level, collected);
                    }

                    var coolBuilding = BuildingFactory.Create(buildingType, level);
                    //logger.InfoFormat("Building: {0}", coolBuilding);

                }
                logger.InfoFormat("Raw json: {0}", json);
            }
        }


        public ResourceManager ResourceManager { get; set; } = new ResourceManager();
        
        public Resource Gold => ResourceManager.GetResource(ResourceType.Gold);
        public Resource Elixir => ResourceManager.GetResource(ResourceType.Elixir);
    }
}
