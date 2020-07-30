using System.Collections.Generic;

namespace DirectDapper.Resources
{
    public interface IResourceManager
    {
        /// <summary>
        /// Used to get an  resource file.
        /// Can return null if resource is not found!
        /// </summary>
        /// <param name="fullResourcePath">Full path of the resource</param>
        /// <returns>The resource</returns>
        ResourceItem GetResource(string fullResourcePath);

        /// <summary>
        /// Used to get the list of  resource file.
        /// </summary>
        /// <param name="fullResourcePath">Full path of the resource</param>
        /// <returns>The list of resource</returns>
        IEnumerable<ResourceItem> GetResources(string fullResourcePath);
    }
}