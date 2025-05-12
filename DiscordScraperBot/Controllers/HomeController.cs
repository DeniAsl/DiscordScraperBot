using DiscordScraperBot.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DiscordScraperBot.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            DiscordBot.RunBotAsync();
        }

        public IActionResult Index(string channelName = null)
        {
            if (channelName != null)
            {
                ViewBag.Channel = Server.GetChannel(channelName);
            }
            return View(Server.GetAllChannels());
        }
        public async Task<IActionResult> Refresh()
        {
            await DiscordBot.GetServerData();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Send(string message, ulong channelId)
        {
            await DiscordBot.SendMessage(message, channelId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, string message, ulong channelId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            DiscordBot.SendFile(filePath, channelId, message);

            return RedirectToAction("Index");
        }
    }
}
