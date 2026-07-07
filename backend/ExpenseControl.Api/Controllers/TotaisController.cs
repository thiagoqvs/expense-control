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

    
    [HttpGet]
    public ActionResult<ConsultaTotaisDto> Consultar()
    {
        return Ok(_service.Calcular());
    }
}
