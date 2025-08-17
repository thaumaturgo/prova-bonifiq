using Microsoft.AspNetCore.Mvc;
using ProvaPub.Services;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Controllers
{
	/// <summary>
	/// Ao rodar o código abaixo o serviço deveria sempre retornar um número diferente, mas ele fica retornando sempre o mesmo número.
	/// 1 - Faça as alterações para que o retorno seja sempre diferente
	/// 2 - Tome cuidado 
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class Parte1Controller :  ControllerBase
	{
		private readonly IRandomService _randomService;

		public Parte1Controller(IRandomService randomService)
		{
			_randomService = randomService;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var result = await _randomService.GetRandom();
			if (result == null)
				return BadRequest("Todos os números no intervalo de 0 a 100 já foram utilizados");
			return Ok(result);
        }
	}
}
