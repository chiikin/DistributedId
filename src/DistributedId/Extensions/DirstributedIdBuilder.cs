using DistributedId.Core;
using DistributedId.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public interface IDirstributedIdBuilder
    {
        IServiceCollection Services { get; }
    }
    internal class DirstributedIdBuilder: IDirstributedIdBuilder
    {
        public DirstributedIdBuilder(IServiceCollection services)
        {
            Services = services;
            services.AddSingleton<IDirstributedIdBuilder>(this);
        }
        public  IServiceCollection Services { get;}
    }
}
