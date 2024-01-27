using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductImporter.Logic.Shared;
using ProductImporter.Logic.Source;
using ProductImporter.Logic.Target;
using ProductImporter.Logic.Transformation;

namespace ProductImporter.Logic
{
    public static class DIRegistrations
    {
        public static IServiceCollection AddProductImporterLogic(this IServiceCollection services)
        {
            services.AddTransient<IPriceParser, PriceParser>();
            services.AddTransient<IProductSource, ProductSource>();

            services.AddTransient<IProductFormatter, ProductFormatter>();
            services.AddTransient<IProductTarget, CsvProductTarget>();

            services.AddTransient<ProductImporter>();

            services.AddSingleton<IImportStatistics, ImportStatistics>();

            //Most recommended - Implement interface without any behavior
            services.AddTransient<IProductTransformer, NullProductTransformer>();

            services.AddOptions<ProductSourceOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    configuration.GetSection(ProductSourceOptions.SectionName).Bind(options);
                });

            services.AddOptions<CsvProductTargetOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    //Part before colon: -Section Name
                    configuration.GetSection(CsvProductTargetOptions.SectionName).Bind(options);
                });

            return services;
        }
    }
}
