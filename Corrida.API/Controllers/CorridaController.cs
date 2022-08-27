using CorridaAPI.Model;
using CorridaAPI.Model.CorridaContext;
using CorridaAPI.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CorridaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
        catch (Exception e)
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
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodasCorrida([FromQuery] RequestQuery query)
    {
        var corridas = await _corridaService.ObterTodasCorrida(query);
        return Ok(corridas);
    }


}
