using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ClashOfClans.Buildings;
using ClashOfClans.Data;
using ClashOfClans.Util.Csv;
//using ClashOfClans.Logic.Building;
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
                        throw new InvalidDataException($"Json length is not valid. {decompressedLength} != {json.Length}.");


                dynamic test = new JavaScriptSerializer().Deserialize<dynamic>(json);
                var provider = new CsvBuildingProvider();

                foreach (var building in test["buildings"])
                {
                    var id = (int) building["data"];
                    var buildingType = (BuildingType)id;
                    var level = (int) building["lvl"];

                    var coolBuilding = provider.GetBuildingData(buildingType, level);
                    coolBuilding.Requirements = new CsvBuildRequirementProvider().GetBuildingData(buildingType, level);

                }
                logger.InfoFormat("Raw json: {0}", json);
            }
        }


        public ResourceManager ResourceManager { get; set; } = new ResourceManager();
        
        public Resource Gold => ResourceManager.GetResource(ResourceType.Gold);
        public Resource Elixir => ResourceManager.GetResource(ResourceType.Elixir);
    }
}
