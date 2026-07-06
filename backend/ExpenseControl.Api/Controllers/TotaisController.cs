using ExpenseControl.Api.Dtos;
using ExpenseControl.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TotaisController : ControllerBase
{
    private readonly TotaisService _service;

    public TotaisController(TotaisService service)
    {
        _service = service;
    }

    /// <summary>
    /// GET /api/totais — retorna o total de receitas, despesas e saldo de cada
    /// pessoa, além do total geral consolidado de todas as pessoas.
    /// </summary>
    [HttpGet]
    public ActionResult<ConsultaTotaisDto> Consultar()
    {
        return Ok(_service.Calcular());
    }
}
