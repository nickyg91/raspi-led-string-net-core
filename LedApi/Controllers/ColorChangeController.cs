using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;
using LedApi.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Camera;

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
                var neoPixel = new ws281x.Net.Neopixel(ledCount, pin, rpi_ws281x.WS2812_STRIP);
                
                neoPixel.Begin();
                for (var i = 0; i < neoPixel.GetNumberOfPixels(); i++)
                {
                    neoPixel.SetPixelColor(i, color);
                }
                neoPixel.Show();
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

        [HttpGet("camera")]
        public async void GetCameraFeed()
        {
            try
            {
                var videoSettings = new CameraVideoSettings
                {
                    CaptureTimeoutMilliseconds = 0,
                    CaptureDisplayPreview = false,
                    ImageFlipVertically = true,
                    CaptureExposure = CameraExposureMode.Auto,
                    CaptureWidth = 1280,
                    CaptureHeight = 720
                };

                //IPAddress localAdd = IPAddress.Parse("127.0.0.1");
                //TcpListener listener = new TcpListener(localAdd, 8081);
                //Console.WriteLine("Listening...");
                //listener.Start();

                //---incoming client connected---
                //var client = listener.AcceptTcpClient();

                //---get the incoming data through a network stream---
                Pi.Camera.OpenVideoStream(videoSettings,
                    onDataCallback: async data =>
                    {
                        Response.StatusCode = (int) HttpStatusCode.PartialContent;
                        Response.ContentType = "video/webm";
                        await Response.Body.WriteAsync(data);
                    },
                    onExitCallback: null);
                Console.WriteLine("Camera started.");
                Console.WriteLine($"Is it busy? {Pi.Camera.IsBusy}");
                //return StatusCode((int)HttpStatusCode.PartialContent, await memstream.ReadAsync(bytes));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType()}: {ex.Message}");
                //return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}