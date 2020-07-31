using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace DirectDapper
{
    public class DirectDapperInitOptions
    {
        private readonly List<Action<IServiceCollection>> initActions;

        public DirectDapperInitOptions()
        {
            this.initActions = new List<Action<IServiceCollection>>();
        }

        public List<Action<IServiceCollection>>  InitActions => initActions;
    }
}