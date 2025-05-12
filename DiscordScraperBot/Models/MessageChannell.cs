namespace DiscordScraperBot.Models
{
    public class MessageChannell
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
