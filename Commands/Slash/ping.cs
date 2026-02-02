using Discord.Interactions;

public sealed class PingSlashModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("ping", "Replies with pong")]
    public async Task PingAsync()
    {
        await RespondAsync("pong");
    }
}