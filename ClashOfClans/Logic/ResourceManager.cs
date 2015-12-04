using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Logic
{
    public class ResourceManager : IEnumerable<Resource>
    {
        private readonly Dictionary<ResourceType, Resource> Resources = new Dictionary<ResourceType, Resource>();

        public Resource GetResource(ResourceType resourceType)
        {
            if (!Resources.ContainsKey(resourceType))
            {
                Resources[resourceType] = new Resource(resourceType);
            }
            return Resources[resourceType];
        }

        public Resource this[ResourceType resourceType] => GetResource(resourceType);
        public Resource this[int resourceId] => GetResource((ResourceType)resourceId);

        public IEnumerator<Resource> GetEnumerator()
        {
            return Resources.Values.GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (ResourceType resourceType in Enum.GetValues(typeof (ResourceType)))
            {
                sb.AppendLine(GetResource(resourceType).ToString());
            }
            return sb.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
