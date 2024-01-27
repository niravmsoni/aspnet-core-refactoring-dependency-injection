using Microsoft.Extensions.DependencyInjection;
using ProductImporter.Logic.Transformation.Util;
using ProductImporter.Transformations;

namespace ProductImporter.Logic.Transformations
{
    public static class DIRegistrations
    {
        public static IServiceCollection AddProductTransformations(this IServiceCollection services)
        {
            services.AddScoped<IProductTransformationContext, ProductTransformationContext>();
            services.AddScoped<INameDecapitaliser, NameDecapitaliser>();

            services.AddScoped<ICurrencyNormalizer, CurrencyNormalizer>();

            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IReferenceAdder, ReferenceAdder>();
            services.AddScoped<IReferenceGenerator, ReferenceGenerator>();
            services.AddSingleton<IIncrementingCounter, IncrementingCounter>();

            return services;
        }
    }
}
