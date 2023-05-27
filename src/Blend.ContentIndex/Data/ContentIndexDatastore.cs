using BlendInteractive.Datastore;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Blend.ContentIndex.Data
{
    public class ContentIndexDatastore : AbstractDatastore
    {
        public ContentIndexDatastore(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        private SqlCommand CreateCommand(FormattableString sql)
        {
            var command = Connection.CreateCommand();
            command.Transaction = Transaction;
            command.CommandType = CommandType.Text;

            // Build parameters and values
            var paramNames = new List<string>();
            for (var x = 0; x < sql.ArgumentCount; x++)
            {
                var argName = $"@param{x}";
                var argValue = sql.GetArgument(x);

                var parameter = command.CreateParameter();
                parameter.ParameterName = argName;
                parameter.Value = argValue ?? DBNull.Value;
                command.Parameters.Add(parameter);

                paramNames.Add(argName);
            }

            // Replace "{...}" with "@param-1" in SQL.
            command.CommandText = string.Format(sql.Format, paramNames.ToArray());

            return command;
        }

        protected IEnumerable<T> Query<T>(FormattableString sql, Func<SqlDataReader, T> transform)
        {
            using var command = CreateCommand(sql);
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return transform(reader);
            }
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(FormattableString sql, Func<SqlDataReader, T> transform)
        {
            using var command = CreateCommand(sql);
            using SqlDataReader reader = await command.ExecuteReaderAsync();

            var output = new List<T>();
            while (await reader.ReadAsync())
            {
                output.Add(transform(reader));
            }
            return output;
        }

        protected object Execute(FormattableString sql)
        {
            using var command = CreateCommand(sql);
            return command.ExecuteScalar();
        }

        protected async Task<object?> ExecuteAsync(FormattableString sql)
        {
            using var command = CreateCommand(sql);
            return await command.ExecuteScalarAsync();
        }

        public void Insert(IndexRecord record)
            => Execute($"INSERT INTO Blend_Index (Identifier, Language, StoreName, Name, Value) VALUES ({record.Identifier}, {record.Language}, {record.StoreName}, {record.Name}, {record.Value});");

        public IEnumerable<string> QueryContent(ContentQuery query)
        {
            using var command = new SqlCommand();
            command.Connection = Connection;
            command.Transaction = Transaction;

            QueryBuilder.Select(command, query);

            using var reader = command.ExecuteReader();

            var output = new List<string>();
            while (reader.Read())
                output.Add(reader.GetString(0));
            return output;
        }

        public void Delete(ContentQuery query)
        {
            using var command = new SqlCommand();
            command.Connection = Connection;
            command.Transaction = Transaction;

            QueryBuilder.Delete(command, query);

            command.ExecuteNonQuery();
        }

        private class QueryBuilder
        {
            public static void Select(SqlCommand command, ContentQuery query)
            {
                var builder = new QueryBuilder(command);
                builder.ApplySelect(query);
            }

            public static void Delete(SqlCommand command, ContentQuery query)
            {
                var builder = new QueryBuilder(command);
                builder.ApplyDelete(query);
            }

            private int currentParameterCount = 0;
            private readonly SqlCommand command;

            private QueryBuilder(SqlCommand command)
            {
                this.command = command;
            }

            private string NextParam(object value)
            {
                var name = $"@param{currentParameterCount++}";
                command.Parameters.AddWithValue(name, value);
                return name;
            }

            private string InParams(IEnumerable<string> values)
            {
                var names = values.Select(v => NextParam(v));
                return string.Join(", ", names);
            }

            private void ApplySelect(ContentQuery query)
            {
                Apply(query, "SELECT Identifier ");
            }
            private void ApplyDelete(ContentQuery query)
            {
                Apply(query, "DELETE ", true);
            }

            private void Apply(ContentQuery query, string commandType, bool forceCommand = false)
            {
                var subquery = GetSubquery(query);


                if (subquery.Length > 0)
                {
                    command.CommandText = forceCommand ? commandType + " FROM Blend_Index WHERE Identifier IN (" + subquery.ToString() + ")" : subquery.ToString();
                }
                else
                {
                    var sql = new StringBuilder();
                    sql.Append($"{commandType} FROM Blend_Index \n");

                    if (query.StoreName is not null)
                    {
                        sql.Append($" WHERE StoreName = {NextParam(query.StoreName)}");
                    }
                    if (query.Language is not null)
                    {
                        sql.Append(sql.Length > 0 ? " AND " : " WHERE ");
                        sql.Append($" Language = {NextParam(query.Language)}");
                    }
                    if (query.Identifier is not null)
                    {
                        sql.Append(sql.Length > 0 ? " AND " : " WHERE ");
                        sql.Append($" Identifier = {NextParam(query.Identifier)}");
                    }
                    if (subquery.Length > 0)
                    {
                        sql.Append(sql.Length > 0 ? " AND " : " WHERE ");
                        sql.Append(" Identifier IN (").Append(subquery).Append(") ");
                    }

                    command.CommandText = sql.ToString();
                }

            }


            private StringBuilder GetSubquery(ContentQuery query)
            {
                var subq = new StringBuilder();
                foreach (var propertyCriteria in query.Criteria)
                {
                    if (subq.Length > 0)
                        subq.Append(" INTERSECT \n");
                    subq.Append($"SELECT Identifier FROM Blend_Index ");

                    var conditions = new StringBuilder();
                    if (query.StoreName is not null)
                    {
                        conditions.Append($" WHERE StoreName = {NextParam(query.StoreName)}");
                    }
                    if (query.Language is not null)
                    {
                        conditions.Append(conditions.Length > 0 ? " AND " : " WHERE ");
                        conditions.Append($" Language = {NextParam(query.Language)}");
                    }
                    if (query.Identifier is not null)
                    {
                        conditions.Append(conditions.Length > 0 ? " AND " : " WHERE ");
                        conditions.Append($" Identifier = {NextParam(query.Identifier)}");
                    }

                    switch (propertyCriteria.Criteria)
                    {
                        case MatchCriteria match:
                            conditions.Append(conditions.Length > 0 ? " AND " : " WHERE ");
                            conditions.Append($" Name = {NextParam(propertyCriteria.PropertyName)} AND Value = {NextParam(match.Value)}");
                            break;
                        case MatchAnyCriteria any:
                            conditions.Append(conditions.Length > 0 ? " AND " : " WHERE ");
                            conditions.Append($" Name = {NextParam(propertyCriteria.PropertyName)} AND Value in ({InParams(any.Values)})");
                            break;
                        case MatchAllCriteria all:
                            conditions.Append(conditions.Length > 0 ? " AND " : " WHERE ");
                            conditions.Append($" Name = {NextParam(propertyCriteria.PropertyName)} AND Value in ({InParams(all.Values)}) \n GROUP BY Identifier HAVING COUNT(*) = {all.Values.Count()}");
                            break;
                    };

                    subq.Append(conditions);
                }
                return subq;
            }
        }
    }
}
