using Dapper;
using MySql.Data.MySqlClient;
using NetCoreAPIMySQL.Model;
using NetCoreAPIMySQL.Model.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreAPIMySQL.Data.Repositories
{
    public class SongRepository : ISongRepository
    {
        private MySQLConfiguration _connectionString;
        public SongRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<Song>> GetAllSongs(SongUserRequest songUserRequest)
        {
            var db = dbConnection();

            var sql = @"
                        SELECT id, title, `group`, `year`, `gender`
                         FROM songs
                         WHERE id NOT IN (SELECT song_id FROM songs_user WHERE user_id = @user_id) ";

            return await db.QueryAsync<Song>(sql, new { User_id =  songUserRequest.user_id });
        }

        public async Task<IEnumerable<Song>> GetAllSongsGeneral()
        {
            var db = dbConnection();

            var sql = @"
                        SELECT id, title, `group`, `year`, `gender`
                         FROM songs";

            return await db.QueryAsync<Song>(sql, new { });
        }

        public async Task<IEnumerable<Song>> GetAllSongsUser(SongUserRequest songUserRequest)
        {
            var db = dbConnection();

            var sql = @"
                        SELECT su.id as id_favorite, s.id, title, `group`, `year`, `gender`
                         FROM songs s
                         INNER JOIN songs_user su
                         ON s.id = su.song_id
                         WHERE user_id = @user_id ";

            return await db.QueryAsync<Song>(sql, new { User_id = songUserRequest.user_id });
        }

        public async Task<Song> GetSongDetails(int id)
        {
            var db = dbConnection();

            var sql = @"
                        SELECT id, title, `group`, `year`, `gender`
                         FROM songs
                         WHERE id = @id ";

            return await db.QueryFirstOrDefaultAsync<Song>(sql, new { Id = id });
        }

        public async Task<bool> InsertSong(Song song)
        {
            var db = dbConnection();

            var sql = @"
                        INSERT INTO songs (`title`, `group`, `year`, `gender`,`created_at`)
                        VALUES (@Title, @Group, @Year, @Gender, now()) ";

            var result = await db.ExecuteAsync(sql, new { song.Title, song.Group, song.Year, song.Gender });
            return result > 0;
        }

        public async Task<bool> UpdateSong(Song song)
        {
            var db = dbConnection();

            var sql = @"
                        UPDATE songs 
                              SET title = @Title, `group` = @Group, `year` = @Year, `gender` = @Gender
                        WHERE id = @Id";
             
            var result = await db.ExecuteAsync(sql, new { song.Title, song.Group, song.Year, song.Gender, song.Id });
            return result > 0;
        }

        public async Task<bool> DeleteSong(int id)
        {
            var db = dbConnection();

            var sql = @"
                        DELETE
                        FROM songs
                        WHERE id = @Id ";

            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }
}
