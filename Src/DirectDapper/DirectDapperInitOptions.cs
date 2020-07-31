using System;
using System.Collections.Generic;
using DirectDapper.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace DirectDapper
{
    public class DirectDapperInitOptions
    {
        private readonly List<Action<IServiceCollection>> initActions;
        private readonly ResourcesConfiguration configuration;

        public DirectDapperInitOptions(ResourcesConfiguration configuration)
        {
            this.initActions = new List<Action<IServiceCollection>>();
            this.configuration = configuration;
        }

        public List<Action<IServiceCollection>>  InitActions => initActions;

        public List<ResourceSet> Sources => configuration.Sources;

        //Add Resource
    }

}