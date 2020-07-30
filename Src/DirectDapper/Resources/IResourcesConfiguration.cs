using System.Collections.Generic;

namespace DirectDapper.Resources
{
    public interface IResourcesConfiguration
    {
        List<ResourceSet> Sources { get; }

    }
}