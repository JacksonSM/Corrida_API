using CorridaAPI.Model.CorridaContext;
using CorridaAPI.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CorridaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorridaController : ControllerBase
    {
        private readonly ICorridaService _corridaService;

        public CorridaController(ICorridaService corridaService)
        {
            _corridaService = corridaService;
        }

        [HttpPost]
        public IActionResult AbrirCorrida(Corrida corrida)
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            try
            {
                _corridaService.Abrir(corrida);
            }
            catch(Exception e) 
            {
                return UnprocessableEntity(e);
            }


            return Ok(corrida);

        }
        [HttpPost("{corridaId:length(24)}/{estado}/")]
        public async Task<IActionResult> AtualizarEstado(string corridaId, string estado, float? estrelas)
        {
            try
            {
                await _corridaService.AtulizarEstado(corridaId, estado, estrelas);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
        [HttpGet("ObterTodasCorridaMototaxista")]
        public async Task<IActionResult> ObterTodasCorridaMototaxista(int mototaxistaId)
        {
            var corridas = await _corridaService.ObterTodasCorridaMototaxista(mototaxistaId);
            return Ok(corridas);
        }
        [HttpGet("ObterTodasCorridaPassageiro")]
        public async Task<IActionResult> ObterTodasCorridaPassageiro(int passageiroId)
        {
            var corridas = await _corridaService.ObterTodasCorridaPassageiro(passageiroId);
            return Ok(corridas);
        }
        [HttpGet("ObterTodasCorridaCidade")]
        public async Task<IActionResult> ObterTodasCorridaCidade(string cidade)
        {
            var corridas = await _corridaService.ObterTodasCorridaCidade(cidade);
            return Ok(corridas);
        }
        [HttpGet("ObterTodasCorridaBairro")]
        public async Task<IActionResult> ObterTodasCorridaBairro(string bairro)
        {
            var corridas = await _corridaService.ObterTodasCorridaBairro(bairro);
            return Ok(corridas);
        }
    }
}
