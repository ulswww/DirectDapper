using System;
using System.Collections.Generic;
using System.Linq;
using DirectDapper.Extensions;

namespace DirectDapper.Resources
{
    public class ResourceManager:IResourceManager
    {
        private readonly IResourcesConfiguration _configuration;
        private readonly Lazy<Dictionary<string, ResourceItem>> _resources;

        public ResourceManager(IResourcesConfiguration configuration)
        {
            _configuration = configuration;
            _resources = new Lazy<Dictionary<string, ResourceItem>>(
                CreateResourcesDictionary,
                true
            );
        }

        /// <inheritdoc/>
        public ResourceItem GetResource(string fullPath)
        {
            var encodedPath = ResourcePathHelper.EncodeAsResourcesPath(fullPath);
            return _resources.Value.GetOrDefault(encodedPath);
        }

        public IEnumerable<ResourceItem> GetResources(string fullPath)
        {
            var encodedPath = ResourcePathHelper.EncodeAsResourcesPath(fullPath);
            if (encodedPath.Length > 0 && !encodedPath.EndsWith("."))
            {
                encodedPath = encodedPath + ".";
            }

            // We will assume that any file starting with this path, is in that directory.
            // NOTE: This may include false positives, but helps in the majority of cases until 
            // https://github.com/aspnet/FileSystem/issues/184 is solved.

            return _resources.Value.Where(k => k.Key.StartsWith(encodedPath)).Select(d => d.Value);
        }

        private Dictionary<string, ResourceItem> CreateResourcesDictionary()
        {
            var resources = new Dictionary<string, ResourceItem>(StringComparer.OrdinalIgnoreCase);

            foreach (var source in _configuration.Sources)
            {
                source.AddResources(resources);
            }

            return resources;
        }
    }
}