﻿using Microsoft.Extensions.DependencyInjection;
using ProductImporter.Model;
using ProductImporter.Logic.Shared;
using ProductImporter.Logic.Transformations;
using ProductImporter.Transformations;

namespace ProductImporter.Logic.Transformation;

public class ProductTransformer : IProductTransformer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IImportStatistics _importStatistics;

    public ProductTransformer(IServiceScopeFactory serviceScopeFactory, IImportStatistics importStatistics)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _importStatistics = importStatistics;
    }

    public Product ApplyTransformations(Product product)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var transformationContext = scope.ServiceProvider.GetRequiredService<IProductTransformationContext>();
        transformationContext.SetProduct(product);

        //Remove this
        //var nameCapitalizer = scope.ServiceProvider.GetRequiredService<INameDecapitaliser>();
        //var currencyNormalizer = scope.ServiceProvider.GetRequiredService<ICurrencyNormalizer>();
        //var referenceAdder = scope.ServiceProvider.GetRequiredService<IReferenceAdder>();

        //Newly added interface
        var productTranformers = scope.ServiceProvider.GetRequiredService<IEnumerable<IProductTransformation>>();

        Thread.Sleep(2000);

        //Demonstrates Polymorphism
        foreach(var productTranformer in productTranformers)
        {
            productTranformer.Execute();
        }

        //nameCapitalizer.Execute();
        //currencyNormalizer.Execute();
        //referenceAdder.Execute();

        if (transformationContext.IsProductChanged())
        {
            _importStatistics.IncrementTransformationCount();
        }

        return transformationContext.GetProduct();
    }
}
