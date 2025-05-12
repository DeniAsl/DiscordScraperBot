namespace DiscordScraperBot.Models
{
    public static class Server
    {
        private static List<MessageChannell> messageChannels = new();

        public static void Add(MessageChannell messageChannel)
        {
            messageChannels.Add(messageChannel);
        }

        public static void Reset()
        {
            messageChannels.Clear();
        }

        public static List<MessageChannell> GetAllChannels()
        {
            return messageChannels;
        }

        public static MessageChannell GetChannel(string name)
        {
            return messageChannels.FirstOrDefault(c => c.Name == name);
        }
    }
}
