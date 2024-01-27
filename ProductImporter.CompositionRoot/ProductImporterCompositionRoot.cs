using Microsoft.Extensions.DependencyInjection;
using ProductImporter.Logic.Transformations;
using ProductImporter.Logic;

namespace ProductImporter.CompositionRoot
{
    public static class ProductImporterCompositionRoot
    {
        public static IServiceCollection AddProductImporter(this IServiceCollection services)
        {
            //Hooking up dependencies for Logic project
            services.AddProductImporterLogic();

            //Hooking up dependencies for transformations project
            //Specifying currency normalizer to be true
            services.AddProductTransformations(o => o.EnableCurrencyNormalizer = true);
            return services;
        }
    }
}
