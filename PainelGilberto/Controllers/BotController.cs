using Microsoft.AspNetCore.Mvc;
using PainelGilberto.DTOs;
using PainelGilberto.Interfaces;

namespace PainelGilberto.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BotController : ControllerBase
    {
        private readonly IBotService _botService;
        public BotController(IBotService botService)
        {
            _botService = botService;
        }
        [HttpGet("getBotData")]
        public IActionResult GetBotData()
        {
            var data = _botService.GetBotData();
            return Ok(data);
        }
        [HttpPost("sendMessage")]
        public IActionResult SendMessage([FromBody] string message)
        {
            var result = _botService.SendMessage(message);
            return Ok(result);
        }

        [HttpPost("anotarPontuacaoDaRodadaAtual")]
        public async Task<IActionResult> AnotarPontuacaoDaRodadaAtual([FromBody] ScoreNotationDTO scoreNotation)
        {
            try
            {
                await _botService.AnotarPontuacaoDaRodadaAtual(scoreNotation);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
