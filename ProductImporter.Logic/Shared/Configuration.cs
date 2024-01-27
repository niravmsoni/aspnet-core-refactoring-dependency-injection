namespace ProductImporter.Logic.Shared;

public class Configuration
{
    // We will deal with passing in configuration in a better (and secure) way in a future module
    // For now hardcoding the values is enough to practice with the concepts from this module
    public static object DatabaseConnectiontring { get; internal set; }
    public string SourceCsvPath => @"C:\Data\Pet Projects\Pluralsight-HandsOn\Dependency-Injection\HenryBeen\CompositionRoot\Implementation\ProductImporter\input-product.csv";
    public string TargetCsvPath => @"C:\Data\Pet Projects\Pluralsight-HandsOn\Dependency-Injection\HenryBeen\CompositionRoot\Implementation\ProductImporter\product-output.csv";
}