using System.Collections.Generic;
using DirectDapper.Extensions;

namespace DirectDapper.Resources
{
    public abstract class ResourceSet
    {
        public string RootPath { get; }

        public string ResourceNamespace { get; }

        public ResourceSet(string rootPath, string resourceNamespace)
        {
            RootPath = rootPath.EnsureEndsWith('/');

            ResourceNamespace = resourceNamespace;
        }

        internal abstract void AddResources(Dictionary<string, ResourceItem> resources);
        protected string ConvertToRelativePath(string resourceName)
        {
            return resourceName.Substring(ResourceNamespace.Length + 1);
        }
    }
}