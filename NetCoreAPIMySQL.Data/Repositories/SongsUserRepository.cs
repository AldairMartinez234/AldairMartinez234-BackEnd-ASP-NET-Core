using Dapper;
using MySql.Data.MySqlClient;
using NetCoreAPIMySQL.Model.Requests;
using System.Threading.Tasks;

namespace NetCoreAPIMySQL.Data.Repositories
{
    public class SongsUserRepository : ISongsUserRepository
    {
        private MySQLConfiguration _connectionString;
        public SongsUserRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<bool> DeleteSongUser(int id)
        {
            var db = dbConnection();

            var sql = @"
                        DELETE
                        FROM songs_user
                        WHERE id = @id ";

            var result = await db.ExecuteAsync(sql, new { id = id });
            return result > 0;
        }

        public async Task<bool> InsertSongUser(SongUserRequest songUserRequest)
        {
            var db = dbConnection();

            var sql = @"
                         INSERT INTO songs_user (user_id, song_id, created_at)
                          VALUES (@User_id, @Song_id, now()) ";

            var result = await db.ExecuteAsync(sql, new { songUserRequest.user_id, songUserRequest.song_id });
            return result > 0;
        }
    }
}
