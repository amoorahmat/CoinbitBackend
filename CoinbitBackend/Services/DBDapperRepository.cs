using Dapper;
using Npgsql;
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
    }
}
