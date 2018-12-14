using System;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using LedApi.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorChangeController : ControllerBase
    {
        private readonly IOptions<LedApiConfiguration> _options;

        public ColorChangeController(IOptions<LedApiConfiguration> options)
        {
            _options = options;
        }
        [HttpGet("{red:int}/{blue:int}/{green:int}")]
        public IActionResult ChangeColor(int red, int blue, int green)
        {
            var color = Color.FromArgb(1, red, green, blue);
            var pin = _options.Value.NeoPixelPin;
            var ledCount = _options.Value.LedCount;
            try
            {
                var neoPixel = new ws281x.Net.Neopixel(ledCount, pin, rpi_ws281x.WS2811_STRIP_GRB);
                
                neoPixel.Begin();
                for (var i = 0; i < neoPixel.GetNumberOfPixels(); i++)
                {
                    neoPixel.SetPixelColor(i, Color.Blue);
                }
                //neoPixel.Dispose();
                return Ok("test");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500, ex);
            }
        }

        //[HttpGet("camera")]
        //public async Task<IActionResult> GetVideoFeed()
        //{
        //    var client = new TcpClient("192.168.1.152", 8080);
        //    //Request.HttpContext.Response.Headers.Add("Content-Type", "video/webm");
        //    var stream = client.GetStream();

        //    return new FileStreamResult(stream, "video/webm");
        //}
    }
}