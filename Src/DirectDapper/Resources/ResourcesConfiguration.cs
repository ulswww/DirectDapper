using System.Collections.Generic;

namespace DirectDapper.Resources
{
    public class ResourcesConfiguration : IResourcesConfiguration
    {
        public List<ResourceSet> Sources { get; }

        public ResourcesConfiguration()
        {
            Sources = new List<ResourceSet>();
        }
    }
}