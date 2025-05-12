using Discord;
using Discord.WebSocket;
using DiscordScraperBot.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace DiscordScraperBot.Models
{
    public static class DiscordBot
    {
        private static DiscordSocketClient client;
        static SocketGuild guild;

        public static async Task RunBotAsync()
        {
            if (client?.ConnectionState == ConnectionState.Connected)
            {
                return;
            }

            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.MessageContent
            };

            client = new DiscordSocketClient(config);
            client.SetStatusAsync(UserStatus.Invisible);
            //client.Log += Log;
            client.MessageReceived += GetServerData;

            //Paste the bot token here
            await client.LoginAsync(TokenType.Bot, "bot token here");
            await client.StartAsync();

            client.GuildAvailable += async guildServer =>
            {
                //Copy and paste the server id here
                if (guildServer.Id == 0101010101)
                {
                    guild = guildServer;
                    GetServerData();
                }
            };

            //Thread.Sleep(3000);
            await Task.Delay(-1);
        }

        public static async Task GetServerData(SocketMessage socketMessage = null)
        {
            //if (guild == null || guild?.Channels == null)
            //{
            //    await Task.Delay(2000);
            //}

            Server.Reset();
            foreach (var channel in guild.Channels.Reverse())
            {
                if (channel.ChannelType != ChannelType.Text)
                    continue;

                IMessageChannel MessageChannel = channel as IMessageChannel;
                var messages = await MessageChannel.GetMessagesAsync(500).FlattenAsync();

                MessageChannell messageChannell = new()
                {
                    Name = channel.Name,
                    Id = channel.Id,
                };

                foreach (var message in messages)
                {
                    Message messageClass = new();
                    messageClass.Author = message.Author.ToString();
                    messageClass.Content = message.Content;
                    messageClass.Date = message.Timestamp.LocalDateTime;

                    if (message.Attachments.Count > 0)
                    {
                        messageClass.Attachments = message.Attachments.ToList();
                    }
                    
                    messageChannell.Messages.Add(messageClass);
                }

                Server.Add(messageChannell);
            }
        }

        public static async Task SendMessage(string message, ulong channelId)
        {
            if (message != null && channelId != 0)
            {
                ISocketMessageChannel channel = client.GetChannel(channelId) as ISocketMessageChannel;
                if (channel != null)
                {
                    await channel.SendMessageAsync(message);
                }
            }
        }

        public static async Task SendFile(string path, ulong channelId, string message = null)
        {
            if (path != null && channelId != 0)
            {
                ISocketMessageChannel channel = client.GetChannel(channelId) as ISocketMessageChannel;
                if (channel != null)
                {
                    await channel.SendFileAsync(path, message);
                }
            }
        }
    }
}
