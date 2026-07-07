using ExpenseControl.Api.Dtos;
using ExpenseControl.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PessoasController : ControllerBase
{
    private readonly PessoaService _service;

    public PessoasController(PessoaService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<PessoaDto>> Listar()
    {
        return Ok(_service.Listar());
    }

    [HttpPost]
    public ActionResult<PessoaDto> Criar([FromBody] CriarPessoaDto dto)
    {
        try
        {
            var pessoa = _service.Criar(dto);
            return CreatedAtAction(nameof(Listar), new { id = pessoa.Id }, pessoa);
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    
    [HttpDelete("{id:guid}")]
    public IActionResult Deletar(Guid id)
    {
        try
        {
            _service.Deletar(id);
            return NoContent();
        }
        catch (RegraDeNegocioException ex)
        {
            return NotFound(new { erro = ex.Message });
        }
    }
}
