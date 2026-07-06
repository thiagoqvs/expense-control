using ExpenseControl.Api.Dtos;
using ExpenseControl.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly TransacaoService _service;

    public TransacoesController(TransacaoService service)
    {
        _service = service;
    }

    /// <summary>GET /api/transacoes — lista todas as transações cadastradas.</summary>
    [HttpGet]
    public ActionResult<List<TransacaoDto>> Listar()
    {
        return Ok(_service.Listar());
    }

    /// <summary>
    /// POST /api/transacoes — cadastra uma nova transação.
    /// Valida: pessoa existente e regra de menor de idade (só despesa).
    /// </summary>
    [HttpPost]
    public ActionResult<TransacaoDto> Criar([FromBody] CriarTransacaoDto dto)
    {
        try
        {
            var transacao = _service.Criar(dto);
            return CreatedAtAction(nameof(Listar), new { id = transacao.Id }, transacao);
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}
