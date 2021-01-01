using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoinbitBackend.Services
{
    public class DBDapperRepository
    {
        private string _connectionString;
        public NpgsqlConnection npgsqlConnection;
        public DBDapperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<object> RunQueryScalar(string query)
        {
            object value;
            using (npgsqlConnection = new NpgsqlConnection(_connectionString))
            {
                await npgsqlConnection.OpenAsync();
                value = await npgsqlConnection.QueryAsync(query);

            }
            return value;
        }


        public IEnumerable<T> RunQuery<T>(string query)
        {
            IEnumerable<T> value;
            using (npgsqlConnection = new NpgsqlConnection(_connectionString))
            {
                npgsqlConnection.Open();
                value = npgsqlConnection.Query<T>(query);

            }
            return value;
        }

        public async Task<IEnumerable<T>> RunQueryAsync<T>(string query)
        {
            IEnumerable<T> value;
            using (npgsqlConnection = new NpgsqlConnection(_connectionString))
            {
                await npgsqlConnection.OpenAsync();
                value = await npgsqlConnection.QueryAsync<T>(query).ConfigureAwait(false);

            }
            return value;
        }
    }
}
