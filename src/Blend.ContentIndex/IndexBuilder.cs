using Blend.ContentIndex.Data;

namespace Blend.ContentIndex
{
    /// <summary>
    /// Builds the index entries for a single piece of content.
    /// </summary>
    public record IndexBuilder(string Identifier, string Language = ContentIndexDefaults.DefaultLanguage, string StoreName = ContentIndexDefaults.DefaultStore)
    {
        public IList<(string Name, string Value)> PropertyValues = new List<(string, string)>();

        public IndexBuilder Add(string name, string value)
        {
            PropertyValues.Add((name, value));
            return this;
        }

        public IEnumerable<IndexRecord> AsRecords()
            => PropertyValues
                .Select(x => new IndexRecord(default, Identifier, Language, StoreName, x.Name, x.Value))
                .ToList();
    }
}
