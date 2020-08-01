using System.Collections.Generic;
using DirectDapper.Extensions;

namespace DirectDapper.Resources
{
    public abstract class ResourceSet
    {
        public string RootPath { get; protected set;}

        public ResourceSet(string rootPath)
        {
            RootPath = rootPath.EnsureEndsWith('/');
        }

        internal abstract void AddResources(Dictionary<string, ResourceItem> resources);
 
    }
}