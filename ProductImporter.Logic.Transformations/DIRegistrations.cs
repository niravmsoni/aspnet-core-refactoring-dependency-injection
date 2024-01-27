using Microsoft.Extensions.DependencyInjection;
using ProductImporter.Logic.Transformation.Util;
using ProductImporter.Transformations;

namespace ProductImporter.Logic.Transformations
{
    public static class DIRegistrations
    {
        public static IServiceCollection AddProductTransformations(this IServiceCollection services,
            Action<ProductTransformationOptions> optionsModifier)
        {
            var options = new ProductTransformationOptions();
            optionsModifier(options);

            services.AddScoped<IProductTransformationContext, ProductTransformationContext>();
            services.AddScoped<INameDecapitaliser, NameDecapitaliser>();

            //Make decisions based on the option supplied
            if (options.EnableCurrencyNormalizer)
            {
                services.AddScoped<ICurrencyNormalizer, CurrencyNormalizer>();
            }
            else
            {
                services.AddScoped<ICurrencyNormalizer, NullCurrencyNormalizer>();
            }

            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IReferenceAdder, ReferenceAdder>();
            services.AddScoped<IReferenceGenerator, ReferenceGenerator>();
            services.AddSingleton<IIncrementingCounter, IncrementingCounter>();

            return services;
        }
    }
}
