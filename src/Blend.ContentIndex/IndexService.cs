using Blend.ContentIndex.Data;

namespace Blend.ContentIndex
{
    public class IndexService
    {
        private readonly ContentIndexDatastoreFactory datastoreFactory;

        public IndexService(ContentIndexDatastoreFactory datastoreFactory)
        {
            this.datastoreFactory = datastoreFactory;
        }

        public void Update(IndexBuilder indexBuilder)
        {
            datastoreFactory.Execute((db) =>
            {
                var deleteQuery = new ContentQuery { Identifier = indexBuilder.Identifier, StoreName = indexBuilder.StoreName, Language = indexBuilder.Language };
                db.Delete(deleteQuery);

                foreach (var record in indexBuilder.AsRecords())
                {
                    db.Insert(record);
                }
            });
        }

        public IEnumerable<string> Query(ContentQuery query)
            => datastoreFactory.Query(db => db.QueryContent(query));

        public void Delete(string identifier, string? storeName = null, string? language = null)
            => datastoreFactory.Execute(db =>
            {
                var deleteQuery = new ContentQuery { Identifier = identifier, StoreName = storeName, Language = language };
            });

        public void Delete(ContentQuery query)
            => datastoreFactory.Execute(db => db.Delete(query));
    }
}
