using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using StreamMusicBot.Services;
using StreamMusicBot.Entities;

namespace StreamMusicBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _cmdService;
        private readonly IServiceProvider _services;
        private readonly Config _config;

        public CommandHandler(DiscordSocketClient client, CommandService cmdService, Config config, IServiceProvider services)
        {
            _client = client;
            _cmdService = cmdService;
            _services = services;
            _config = config;
        }

        public async Task InitializeAsync()
        {
            await _cmdService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _cmdService.Log += LogAsync;
            _client.MessageReceived += HandleMessageAsync;
        }

        private async Task HandleMessageAsync(SocketMessage socketMessage)
        {
            if (socketMessage.Author.IsBot) return;

            var userMessage = socketMessage as SocketUserMessage;
            if (userMessage is null) return;

            var context = new SocketCommandContext(_client, userMessage);
            int argPos = 0;

            if (userMessage.HasStringPrefix(_config.Prefics, ref argPos) || userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos))
            { 
                var result = await _cmdService.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }

            
        }

        private Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }
    }
}
