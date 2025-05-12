using Discord;

namespace DiscordScraperBot.Models
{
    public class Message
    {
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTimeOffset Date { get; set; }
        public List<IAttachment> Attachments { get; set; } = new List<IAttachment>();
    }
}
