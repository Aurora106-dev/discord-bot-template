using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

var token = Config.Token;
if (string.IsNullOrWhiteSpace(token))
{
    Console.WriteLine("Token is missing");
    return;
}

var client = new DiscordSocketClient(new DiscordSocketConfig
{
    GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.MessageContent
});

var interactionService = new InteractionService(client.Rest);
var commandService = new CommandService(new CommandServiceConfig
{
    CaseSensitiveCommands = false
});

var services = new ServiceCollection()
    .AddSingleton(client)
    .AddSingleton(interactionService)
    .AddSingleton(commandService)
    .AddSingleton<CommandHandler>()
    .BuildServiceProvider();

client.Log += LogAsync;
interactionService.Log += LogAsync;
commandService.Log += LogAsync;

await services.GetRequiredService<CommandHandler>().InitializeAsync();

await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();

await Task.Delay(Timeout.Infinite);

static Task LogAsync(LogMessage message)
{
    Console.WriteLine($"[{DateTimeOffset.Now:O}] {message.Severity}: {message.Source} - {message.Message}");
    if (message.Exception != null)
    {
        Console.WriteLine(message.Exception);
    }
    return Task.CompletedTask;
}

public sealed class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactionService;
    private readonly CommandService _commandService;
    private readonly IServiceProvider _services;

    public CommandHandler(DiscordSocketClient client, InteractionService interactionService, CommandService commandService, IServiceProvider services)
    {
        _client = client;
        _interactionService = interactionService;
        _commandService = commandService;
        _services = services;
    }

    public async Task InitializeAsync()
    {
        await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        _client.Ready += OnReadyAsync;
        _client.InteractionCreated += OnInteractionCreatedAsync;
        _client.MessageReceived += OnMessageReceivedAsync;
    }

    private async Task OnReadyAsync()
    {
        var guild = Config.Guild;
        if (string.IsNullOrWhiteSpace(guild))
        {
            await _interactionService.RegisterCommandsGloballyAsync(true);
            return;
        }

        if (ulong.TryParse(guild, out var guildId))
        {
            await _interactionService.RegisterCommandsToGuildAsync(guildId, true);
        }
        else
        {
            await _interactionService.RegisterCommandsGloballyAsync(true);
        }
    }

    private async Task OnInteractionCreatedAsync(SocketInteraction interaction)
    {
        var ctx = new SocketInteractionContext(_client, interaction);
        await _interactionService.ExecuteCommandAsync(ctx, _services);
    }

    private async Task OnMessageReceivedAsync(SocketMessage rawMessage)
    {
        if (rawMessage is not SocketUserMessage message) return;
        if (message.Author.IsBot) return;

        var argPos = 0;
        if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;

        var ctx = new SocketCommandContext(_client, message);
        await _commandService.ExecuteAsync(ctx, argPos, _services);
    }
}

static class Config
{
    public const string Token = "yourbottokenhere";
    public const string Guild = "guildid only slash command or if you want global slash commands keep blank or just say guild idk anything u want";
}
