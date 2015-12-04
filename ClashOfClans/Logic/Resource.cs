using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Logic
{
    public enum ResourceType
    {
        Gold = 3000001,
        Elixir = 3000002,
        DarkElixir = 3000003,
        WarGold = 3000004,
        WarElixir = 3000005,
        WarDarkElixir = 3000006
    }
    public class Resource
    {
        public Resource()
        {
            
        }

        public Resource(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }
        public ResourceType ResourceType { get; set; }
        public int Capacity { get; set; }
        public int Amount { get; set; }
        public override string ToString()
        {
            return string.Format("{0} - Capacity: {1}, Amount: {2}", ResourceType, Capacity.ToString("N0"), Amount.ToString("N0"));
        }
    }
}
