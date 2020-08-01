using System;
using System.Collections.Generic;
using DirectDapper.Extensions;

namespace DirectDapper.Resources
{
    public abstract class ResourceSet
    {
        public string RootPath { get; protected set;}

        protected  string Id{get;}

        public ResourceSet(string rootPath)
        {
            RootPath = rootPath.EnsureEndsWith('/');

            Id = Guid.NewGuid().ToString();
        }

        internal abstract void AddResources(Dictionary<string, ResourceItem> resources);
 
    }
}