using Discord;
using Discord.Interactions;

public sealed class EmbedTestSlashModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("embedtest", "Replies with a test embed")]
    public async Task EmbedTestAsync()
    {
        var embed = new EmbedBuilder()
            .WithTitle("Embed Test")
            .WithDescription("hello world")
            .WithColor(Color.Blue)
            .Build();

        await RespondAsync(embed: embed);
    }
}