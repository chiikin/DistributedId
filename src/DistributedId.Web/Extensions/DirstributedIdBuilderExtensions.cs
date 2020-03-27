using DistributedId.Core;
using DistributedId.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DirstributedIdBuilderExtensions
    {
        public static IDirstributedIdBuilder AddDbContextStore(this IDirstributedIdBuilder builder)
        {
            builder.Services.AddTransient<ISequencedIdStore, SequenceIdRepository>();
            return builder;
        }
    }
}
