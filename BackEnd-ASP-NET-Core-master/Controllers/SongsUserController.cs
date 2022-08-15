using Microsoft.AspNetCore.Mvc;
using NetCoreAPIMySQL.Data.Repositories;
using NetCoreAPIMySQL.Model.Requests;
using System.Threading.Tasks;

namespace WebTestNET.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SongsUserController : ControllerBase
    {
        private readonly ISongsUserRepository _songsUserRepository;

        public SongsUserController(ISongsUserRepository songsUserRepository)
        {
            _songsUserRepository = songsUserRepository;
        }

        [HttpPost]
        public IActionResult InsertSongUser(SongUserRequest songUserRequest)
        {
            if (songUserRequest.user_id != null && songUserRequest.song_id != null)
            {
                var response = _songsUserRepository.InsertSongUser(songUserRequest);

                if (response == null)
                    return Ok(new { status = 0, message = "Error al agregar la cancion a tu lista de favoritos" });

                return Ok(new { status = 1, message = "Cancion agregada a tu lista de favoritos" });
            }
            else
            {
                return Ok(new { status = 0, message = "Error al agregar la cancion a tu lista de favoritos" });
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSongUser(int id)
        {
            await _songsUserRepository.DeleteSongUser(id);
            return Ok(new { status = 1, message = "Canción eliminada con exito de tu lista de favoritos" });
        }

    }
}
