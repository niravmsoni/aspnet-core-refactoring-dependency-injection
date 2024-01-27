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
            services.AddScoped<IProductTransformation, NameDecapitaliser>();

            //Make decisions based on the option supplied
            if (options.EnableCurrencyNormalizer)
            {
                services.AddScoped<IProductTransformation, CurrencyNormalizer>();
            }
            else
            {
                services.AddScoped<IProductTransformation, NullCurrencyNormalizer>();
            }

            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IProductTransformation, ReferenceAdder>();
            services.AddScoped<IReferenceGenerator, ReferenceGenerator>();
            services.AddSingleton<IIncrementingCounter, IncrementingCounter>();

            return services;
        }
    }
}
