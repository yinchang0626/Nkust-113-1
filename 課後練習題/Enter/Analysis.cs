using CsvHelper.Configuration.Attributes;
namespace Analysis
{
    public class Entry
    {
        [Name("year")] public required string Year { get; set; }
        [Name("education")] public required string Education { get; set; }
        [Name("item")] public required string Item { get; set; }
        [Name("total")] public required string Total { get; set; }
        [Name("men person")] public required string Male { get; set; }
        [Name("female person")] public required string Female { get; set; }

    }
}