using NetCoreAPIMySQL.Model.Requests;
using System.Threading.Tasks;

namespace NetCoreAPIMySQL.Data.Repositories
{
    public interface ISongsUserRepository
    {
        Task<bool> InsertSongUser(SongUserRequest songUserRequest);
        Task<bool> DeleteSongUser(int id);
    }
}
