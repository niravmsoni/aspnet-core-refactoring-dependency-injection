﻿using Microsoft.Extensions.Options;
using ProductImporter.Logic.Shared;
using ProductImporter.Model;

namespace ProductImporter.Logic.Target;

public class CsvProductTarget : IProductTarget
{
    private readonly IOptions<CsvProductTargetOptions> _productTargetOptions;
    private readonly IProductFormatter _productFormatter;
    private readonly IImportStatistics _importStatistics;

    private StreamWriter? _streamWriter;

    public CsvProductTarget(IOptions<CsvProductTargetOptions> productTargetOptions, IProductFormatter productFormatter, IImportStatistics importStatistics)
    {
        _productTargetOptions = productTargetOptions;
        _productFormatter = productFormatter;
        _importStatistics = importStatistics;
    }

    public void Open()
    {
        _streamWriter = new StreamWriter(_productTargetOptions.Value.TargetCsvPath);

        var headerLine = _productFormatter.GetHeaderLine();
        _streamWriter.WriteLine(headerLine);
    }

    public void AddProduct(Product product)
    {
        if (_streamWriter == null)
            throw new InvalidOperationException("Cannot add products to a target that is not yet open");

        var productLine = _productFormatter.Format(product);

        _importStatistics.IncrementOutputCount();
        _streamWriter.WriteLine(productLine);
    }

    public void Close()
    {
        if (_streamWriter == null)
            throw new InvalidOperationException("Cannot close a target that is not yet open");

        _streamWriter.Flush();
        _streamWriter.Close();
    }
}
