using System;
using System.Reflection;
using System.Threading.Tasks;
using AltV.Net.Async;
using AltV.Net;
using Discord.WebSocket;
using Discord;
using Discord.Commands;

namespace DiscordBot
{
    public class Main : AsyncResource
    {
        public override void OnStart()
        {

            try
            {
                Configuration.FetchConfiguration();
                if (Configuration.discordBotToken.Length <= 0) { Alt.Log($"Discord-Token in der config.json fehlt!"); return; }
                MainAsync().GetAwaiter().GetResult();
                Alt.OnServer<string, string, string>("DiscordBot:DiscordLog", DiscordLog);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public override void OnStop()
        {
            //
        }

        private DiscordSocketClient _client;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandler;


            _client.Log += Log;
            var token = Configuration.discordBotToken;
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            //await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        #region Logs

        public async void DiscordLog(string title, string content, string color)
        {
            try
            { 
                ITextChannel textChannel = _client.GetChannel(Configuration.discordBotChannel) as ITextChannel;
                if (textChannel == null) return;
                var EmbedBuilder = new EmbedBuilder().WithColor(GetColor(color)).WithTitle(title).WithDescription(content).WithFooter(footer => footer.WithText("Much Roleplay - 2020").WithIconUrl("https://i.imgur.com/Qigi1rX.png"));
                Embed embedLog = EmbedBuilder.Build();
                textChannel.SendMessageAsync(embed: embedLog);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static Discord.Color GetColor(string col)
        {
            switch(col)
            {
                case "red": return Color.Red;
                case "green": return Color.Green;
                case "blue": return Color.Blue;
            }
            return Color.Blue;
        }
        #endregion

        private Task CommandHandler(SocketMessage message)
        {
            string command = "";
            int lengthOfCommand = -1;
            if (!message.Content.StartsWith('!')) return Task.CompletedTask;
            if (message.Author.IsBot) return Task.CompletedTask;

            if (message.Content.Contains(' ')) lengthOfCommand = message.Content.IndexOf(' ');
            else lengthOfCommand = message.Content.Length;

            command = message.Content.Substring(1, lengthOfCommand - 1).ToLower();

            //Commands
            if (command.Equals("hello"))
                message.Channel.SendMessageAsync("Hallo!");
            
            return Task.CompletedTask;
        }
    }
}
