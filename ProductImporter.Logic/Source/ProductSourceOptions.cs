namespace ProductImporter.Logic.Source;

/// <summary>
/// New class for using Options pattern
/// </summary>
public class ProductSourceOptions
{
    public const string SectionName = "ProductSource";
    public string SourceCsvPath { get; set; }
}