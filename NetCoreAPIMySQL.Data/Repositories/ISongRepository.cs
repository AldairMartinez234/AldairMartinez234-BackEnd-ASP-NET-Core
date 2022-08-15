using NetCoreAPIMySQL.Model;
using NetCoreAPIMySQL.Model.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreAPIMySQL.Data.Repositories
{
    public interface ISongRepository
    {
        Task<IEnumerable<Song>> GetAllSongs(SongUserRequest songUserRequest);
        Task<IEnumerable<Song>> GetAllSongsUser(SongUserRequest songUserRequest);
        Task<IEnumerable<Song>> GetAllSongsGeneral();
        Task<Song> GetSongDetails(int id);
        Task<bool> InsertSong(Song song);
        Task<bool> UpdateSong(Song song);
        Task<bool> DeleteSong(int id);
    }
}
