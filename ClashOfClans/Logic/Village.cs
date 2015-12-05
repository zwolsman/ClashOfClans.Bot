using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ClashOfClans.Data.Csv;
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

                    var coolBuilding = BuildingFactory.Create(buildingType, level);
                    logger.InfoFormat("Building: {0}", coolBuilding);

                }
                logger.InfoFormat("Raw json: {0}", json);
            }
        }


        public ResourceManager ResourceManager { get; set; } = new ResourceManager();
        
        public Resource Gold => ResourceManager.GetResource(ResourceType.Gold);
        public Resource Elixir => ResourceManager.GetResource(ResourceType.Elixir);
    }
}
