namespace ProductImporter.Logic.Target;

/// <summary>
/// Options pattern implementation
/// </summary>
public class CsvProductTargetOptions
{
    //Hard-coding sectionName here. This should match to contents before colon in appsettings.json
    public const string SectionName = "CsvProductTarget";
    public string TargetCsvPath { get; set; }
}