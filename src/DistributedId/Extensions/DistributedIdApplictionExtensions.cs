using DistributedId;
using DistributedId.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DistributedIdApplictionExtensions
    {
        #region 注册服务
        public static IDirstributedIdBuilder AddDirstributedId(this IServiceCollection services)
        {
            var builder = new DirstributedIdBuilder(services);
            services.AddSingleton<IDistributedIdFactory, DistributedIdFactory>();
            return builder;
        }

        public static IDirstributedIdBuilder AddStore<T>(this IDirstributedIdBuilder builder) where T : class, ISequencedIdStore
        {
            builder.Services.AddTransient<ISequencedIdStore, T>();
            return builder;
        }

        #endregion

        #region 注册中间件
        public static void UseIdSequenceStoreService(this IApplicationBuilder builder)
        {
            PathString pathString = new PathString("/distribute");
            builder.Use((next) =>
            {
                return async (context) =>
                {

                    if (context.Request.Path.StartsWithSegments(pathString))
                    {
                        //TODO:

                    }
                    else
                    {
                        await next(context);
                    }
                };
            });
        }
        #endregion
    }
}
