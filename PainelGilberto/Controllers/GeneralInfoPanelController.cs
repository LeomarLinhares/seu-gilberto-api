using Microsoft.AspNetCore.Mvc;
using PainelGilberto.Services;

namespace PainelGilberto.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneralInfoPanelController : ControllerBase
    {
        private readonly IGeneralInfoService _generalInfoService;

        public GeneralInfoPanelController(IGeneralInfoService generalInfoService)
        {
            _generalInfoService = generalInfoService;
        }

        [HttpGet("roundsBySeason")]
        public IActionResult GetRoundsBySeason()
        {
            var data = _generalInfoService.GetRoundsBySeason();
            return Ok(data);
        }

        [HttpGet("dashboard/{seasonId}")]
        public IActionResult GetDashboard(int seasonId)
        {
            var data = _generalInfoService.GetAllDashboardData(seasonId);
            return Ok(data);
        }
    }
}
