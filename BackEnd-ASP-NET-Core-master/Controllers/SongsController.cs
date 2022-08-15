using Microsoft.AspNetCore.Mvc;
using NetCoreAPIMySQL.Data.Repositories;
using NetCoreAPIMySQL.Model;
using NetCoreAPIMySQL.Model.Requests;
using System.Threading.Tasks;

namespace WebTestNET.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly ISongRepository _songRepository;

        public SongsController(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        [HttpPost("lista_disponible")]
        public async Task<IActionResult> GetAllSongs(SongUserRequest songUserRequest)
        {
            return Ok(await _songRepository.GetAllSongs(songUserRequest));
        }

        [HttpPost("lista_favoritas")]
        public async Task<IActionResult> GetAllSongsUser(SongUserRequest songUserRequest)
        {
            return Ok(await _songRepository.GetAllSongsUser(songUserRequest));
        }

        [HttpGet("lista_general")]
        public async Task<IActionResult> GetAllSongsGeneral()
        {
            return Ok(await _songRepository.GetAllSongsGeneral());
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSong([FromBody] Song song)
        {
            if(song == null)
                return BadRequest(new { status = 0, message = "Algo fallo y no se puedo actualizar la canción" });

            if(!ModelState.IsValid)
                return BadRequest(new { status = 0, message = "Algo fallo y no se puedo actualizar la canción" });

            await _songRepository.UpdateSong(song);

            return Ok(new { status = 1, message = "Canción actualizada con exito" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            await _songRepository.DeleteSong(id);
            return Ok(new { status = 1, message = "Canción eliminada con exito" });
        }

        [HttpPost("addSong")]
        public async Task<IActionResult> InsertSong([FromBody] Song song)
        {
            var response =  await _songRepository.InsertSong(song);

            if (response == null)
                return Ok(new { status = 0, message = "Debes llenar todos los campos" });

            return Ok(new { status = 1, message = "Canción añadida con exito" });

        }
    }
}
