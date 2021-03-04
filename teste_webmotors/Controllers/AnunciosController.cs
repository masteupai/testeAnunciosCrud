using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net;
using System.Threading.Tasks;
using teste_webmotors.Model;
using teste_webmotors.Services;
using teste_webmotors.ViewModel;

namespace teste_webmotors.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnunciosController : Controller
    {
        private readonly IAnunciosService _anunciosService;


        public AnunciosController(IAnunciosService anunciosService)
        {
            _anunciosService = anunciosService;
        }

        [HttpGet("listar/{pagina}/{quantidade}")]
        public async Task<ActionResult> List(int pagina ,int quantidade)
        {
            try
            {
                var retorno = await _anunciosService.Listar(quantidade,pagina);

                return await ValidaRetorno(retorno.StatusCode, retorno.Objeto);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{anuncioId}")]
        public async Task<ActionResult> Get(int anuncioId)
        {
            try
            {
                var retorno = await _anunciosService.BuscarPorId(anuncioId);

                return await ValidaRetorno(retorno.StatusCode, retorno.Objeto);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AnuncioVMIncluir anuncioVM)
        {
            try
            {
                var retorno = await _anunciosService.Adicionar(anuncioVM);

                return await ValidaRetorno(retorno.StatusCode, retorno.Objeto);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(AnuncioVMAlterar anuncioVM)
        {
            try
            {
                var retorno = await _anunciosService.Alterar(anuncioVM);

                return await ValidaRetorno(retorno.StatusCode, retorno.Objeto);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }

        }

        [HttpDelete("{anuncioId}")]
        public async Task<ActionResult> Delete(int anuncioId)
        {
            try
            {
                var retorno = await _anunciosService.Remover(anuncioId);

                return await ValidaRetorno(retorno.StatusCode);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }

        private async Task<ActionResult> ValidaRetorno(int statusCode, object obj = null)
        {
            switch (statusCode)
            {
                case (int)HttpStatusCode.OK:
                    return Ok(obj);
                case (int)HttpStatusCode.Created:
                    return Created("", obj);
                case (int)HttpStatusCode.NoContent:
                    return NoContent();
                case (int)HttpStatusCode.NotFound:
                    return NotFound();
                case (int)HttpStatusCode.BadRequest:
                    return BadRequest();
                default:
                    return StatusCode(500);
            }
        }
    }
}
