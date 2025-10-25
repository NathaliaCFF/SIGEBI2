using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Mappers;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController : ControllerBase
    {
        private readonly IReporteService _reporteService;

        public ReporteController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        [HttpGet("libros-mas-prestados")]
        public async Task<IActionResult> ObtenerLibrosMasPrestados()
        {
            var result = await _reporteService.ObtenerLibrosMasPrestadosAsync();

            if (!result.Success || result.Data == null || !result.Data.Any())
                return NotFound(result.Message);

            var dtoList = result.Data.ToDtoList();
            return Ok(dtoList);
        }
    }
}
