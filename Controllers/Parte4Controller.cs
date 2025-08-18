using Microsoft.AspNetCore.Mvc;
using ProvaPub.Dtos;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Controllers;


/// <summary>
/// O Código abaixo faz uma chmada para a regra de negócio que valida se um consumidor pode fazer uma compra.
/// Crie o teste unitário para esse Service. Se necessário, faça as alterações no código para que seja possível realizar os testes.
/// Tente criar a maior cobertura possível nos testes.
/// 
/// Utilize o framework de testes que desejar. 
/// Crie o teste na pasta "Tests" da solution
/// </summary>
[ApiController]
[Route("[controller]")]
public class Parte4Controller : ControllerBase
{
    private readonly ICustomerService _customerService;

    public Parte4Controller(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("CanPurchase")]
    public async Task<IActionResult> CanPurchase([FromQuery] PurchaseRequest request)
    {
        return Ok(await _customerService.CanPurchase(request));
    }
}
