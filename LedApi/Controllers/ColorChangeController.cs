using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            using (var neoPixel = new ws281x.Net.Neopixel(ledCount, pin, rpi_ws281x.WS2812_STRIP))
            {
                neoPixel.Begin();
                for (var i = 0; i < neoPixel.GetNumberOfPixels(); i++)
                {
                    neoPixel.SetPixelColor(i, color);
                }
                neoPixel.Show();
            }
            return Ok(color.Name);
        }
    }
}