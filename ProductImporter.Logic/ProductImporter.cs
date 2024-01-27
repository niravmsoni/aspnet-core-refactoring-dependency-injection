using Microsoft.Extensions.DependencyInjection;
using ProductImporter.Logic.Shared;
using ProductImporter.Logic.Source;
using ProductImporter.Logic.Target;
using ProductImporter.Logic.Transformation;

namespace ProductImporter.Logic;

public class ProductImporter
{
    private readonly IProductSource _productSource;
    private readonly IServiceProvider _serviceProvider;
    //private readonly IProductTransformer _productTransformer;
    private readonly IProductTarget _productTarget;
    private readonly IImportStatistics _importStatistics;

    public ProductImporter(
        IProductSource productSource, 
        IServiceProvider serviceProvider,
        //Remove IProductTransformer and inject IServiceProvider
        //IProductTransformer productTransformer,
        IProductTarget productTarget, 
        IImportStatistics importStatistics)
    {
        _productSource = productSource;
        _serviceProvider = serviceProvider;
        //_productTransformer = productTransformer;
        _productTarget = productTarget;
        _importStatistics = importStatistics;
    }

    public void Run()
    {
        _productSource.Open();
        _productTarget.Open();

        while (_productSource.hasMoreProducts())
        {
            var product = _productSource.GetNextProduct();

            //Throws InvalidOperationException when implementation is missing
            //var serviceProvider = _serviceProvider.GetRequiredService<IProductTransformer>();

            //Does not throw exception but returns null when type registration is missing
            var productTransformer = _serviceProvider.GetService<IProductTransformer>();

            
            var transformedProduct = productTransformer == null 
                ? product
                : productTransformer.ApplyTransformations(product);

            _productTarget.AddProduct(transformedProduct);
        }

        _productSource.Close();
        _productTarget.Close();

        Console.WriteLine("Importing complete!");
        Console.WriteLine(_importStatistics.GetStatistics());
    }
}