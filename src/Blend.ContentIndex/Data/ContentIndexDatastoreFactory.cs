using BlendInteractive.Datastore;
using System.Data.SqlClient;

namespace Blend.ContentIndex.Data
{
    public class ContentIndexDatastoreFactory : AbstractDatastoreFactory<ContentIndexDatastore>
    {
        public ContentIndexDatastoreFactory(string connectionString) : base(connectionString)
        {
        }

        public override string SqlResourcesPrefix => "Blend.ContentIndex.Data.Migrations";

        protected override string GetVersionProcedureName => "Blend_GetIndexVersion";

        protected override int CurrentVersion => 1;

        protected override ContentIndexDatastore GetDatastore(SqlConnection conn, SqlTransaction trans)
            => new ContentIndexDatastore(conn, trans);
    }
}
