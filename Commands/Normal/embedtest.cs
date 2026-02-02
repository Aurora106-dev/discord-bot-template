using Discord;
using Discord.Commands;

public sealed class EmbedTestCommandModule : ModuleBase<SocketCommandContext>
{
    [Command("embedtest")]
    public async Task EmbedTestAsync()
    {
        var embed = new EmbedBuilder()
            .WithTitle("Embed Test")
            .WithDescription("hello world")
            .WithColor(Color.Blue)
            .Build();

        await ReplyAsync(embed: embed);
    }
}