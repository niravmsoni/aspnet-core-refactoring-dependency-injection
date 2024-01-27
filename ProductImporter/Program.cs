﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductImporter.CompositionRoot;

using var host = Host.CreateDefaultBuilder(args)
    .UseDefaultServiceProvider((context, options) => {
        options.ValidateScopes = true;
    })
    .ConfigureServices((context, services) =>
    {
        services.AddProductImporter();
    })
    .Build();

var productImporter = host.Services.GetRequiredService<ProductImporter.Logic.ProductImporter>();

productImporter.Run();