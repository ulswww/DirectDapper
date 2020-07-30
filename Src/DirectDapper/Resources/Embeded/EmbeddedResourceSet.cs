using System.Collections.Generic;
using System.Reflection;
using DirectDapper.Extensions;

namespace DirectDapper.Resources.Embedded
{
    public class EmbeddedResourceSet:ResourceSet
    {
        public Assembly Assembly { get; }

        public EmbeddedResourceSet(string rootPath, Assembly assembly, string resourceNamespace):base(rootPath,resourceNamespace)
        {
            Assembly = assembly;
        }

        internal override void AddResources(Dictionary<string, ResourceItem> resources)
        {
            foreach (var resourceName in Assembly.GetManifestResourceNames())
            {
                if (!resourceName.StartsWith(ResourceNamespace))
                {
                    continue;
                }

                using (var stream = Assembly.GetManifestResourceStream(resourceName))
                {
                    var relativePath = ConvertToRelativePath(resourceName);
                    var filePath = ResourcePathHelper.NormalizePath(RootPath) + relativePath;

                    resources[filePath] = new EmbeddedResourceItem(
                        filePath,
                        stream.GetAllBytes(),
                        Assembly
                    );
                }
            }
        }
    }
}