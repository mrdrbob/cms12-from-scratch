namespace Blend.ContentIndex
{
    public record ContentQuery()
    {
        public string? StoreName { get; set; }

        public string? Language { get; set; }

        public string? Identifier { get; set; }

        public IList<PropertyCriteria> Criteria = new List<PropertyCriteria>();
    }

    public record PropertyCriteria(string PropertyName, ICriteria Criteria);

    public interface ICriteria { }

    public record MatchAnyCriteria(IEnumerable<string> Values) : ICriteria;

    public record MatchAllCriteria(IEnumerable<string> Values) : ICriteria;

    public record MatchCriteria(string Value) : ICriteria;

    public static class ContentQueryExtensions
    {
        public static ContentQuery Where(this ContentQuery query, string name, ICriteria criteria)
        {
            query.Criteria.Add(new PropertyCriteria(name, criteria));
            return query;
        }

        public static ContentQuery WhereMatch(this ContentQuery query, string name, string value) => Where(query, name, new MatchCriteria(value));

        public static ContentQuery WhereMatchAny(this ContentQuery query, string name, params string[] values) => Where(query, name, new MatchAnyCriteria(values));

        public static ContentQuery WhereMatchAll(this ContentQuery query, string name, params string[] values) => Where(query, name, new MatchAllCriteria(values));
    }
}
