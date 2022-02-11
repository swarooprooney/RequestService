using Microsoft.AspNetCore.Mvc;
using RequestService.Policies;

namespace RequestService.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BingoController : ControllerBase
    {
        private readonly ClientPolicy _policy;
        private readonly IHttpClientFactory _clientFactory;

        public BingoController(ClientPolicy policy, IHttpClientFactory clientFactory)
        {
            _policy = policy;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetLucky()
        {
            //var httpclient = new HttpClient();
            //var response = await httpclient.GetAsync("https://localhost:7178/api/lucky/50");
            var client = _clientFactory.CreateClient();
            var response = await _policy.ImmediateRetryPolicy.ExecuteAsync(() => client.GetAsync("https://localhost:7178/api/lucky/30"));
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Bingo Indeed");
                return Ok();
            }
            Console.WriteLine("Lets get home");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}