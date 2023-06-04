using Blend.ContentIndex;
using Blend.ContentIndex.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blend.Cms12.Business.ContentIndex
{
    public static class ContentIndexStartupExtensions
    {
        public static IServiceCollection AddContentIndex(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton(new ContentIndexDatastoreFactory(config["ConnectionStrings:EPiServerDB"]));
            services.AddTransient<IndexService>();
            services.AddTransient<OptimizelyContentIndexService>();

            return services;
        }

    }
}
