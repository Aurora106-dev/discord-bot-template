using Discord.Commands;

public sealed class PingCommandModule : ModuleBase<SocketCommandContext>
{
    [Command("ping")]
    public async Task PingAsync()
    {
        await ReplyAsync("pong");
    }
}