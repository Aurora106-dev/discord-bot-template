# Discord Bot Template (C#)

Simple Discord.Net bot with both slash commands and normal prefix commands.

## Requirements
- .NET SDK 8.0 (recommended)
- A Discord bot token
- A Discord application with bot and slash command permissions

If you use a different SDK version, adjust the project file as shown below.

## Install the same .NET version
This project targets `net8.0`. Install the .NET 8 SDK so `dotnet` can build and run it.

Official downloads (pick the SDK for your OS):
```
https://dotnet.microsoft.com/download/dotnet/8.0
```

Windows install guide (includes installer options and PowerShell script notes):
```
https://learn.microsoft.com/en-us/dotnet/core/install/windows
```

## Project file (example)
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Discord.Net" Version="3.16.0" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
  </ItemGroup>
</Project>
```

If you are on .NET 7 or .NET 6, change `TargetFramework` accordingly, for example:
- `net7.0`
- `net6.0`

If the Discord.Net package version you need is different, update the `PackageReference` version and run `dotnet restore`.

## Setup
1) Put your token and optional guild id in `Program.cs`:

```csharp
public const string Token = "YOUR_TOKEN";
public const string Guild = "YOUR_GUILD_ID";
```

- Leave `Guild` as an empty string to register slash commands globally.
- Use a guild id to register slash commands instantly for one server.

2) Restore and run:

```bash
dotnet restore
dotnet run
```

## Using the bot
- Normal command: `!ping`
- Slash command: `/ping`

## Creating commands

### Slash commands
Create a class in `Commands/Slash` and inherit from `InteractionModuleBase<SocketInteractionContext>`.

Example:
```csharp
using Discord.Interactions;

public sealed class HelloSlashModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("hello", "Says hello")]
    public async Task HelloAsync()
    {
        await RespondAsync("hello");
    }
}
```

### Normal prefix commands
Create a class in `Commands/Normal` and inherit from `ModuleBase<SocketCommandContext>`.

Example:
```csharp
using Discord.Commands;

public sealed class HelloCommandModule : ModuleBase<SocketCommandContext>
{
    [Command("hello")]
    public async Task HelloAsync()
    {
        await ReplyAsync("hello");
    }
}
```

## Common issues
- Slash commands not showing up: if `Guild` is empty, global registration can take a while. Use a guild id for instant registration.
- Token invalid: make sure you copied the bot token (not the client secret).
- Missing permissions: ensure the bot has `applications.commands` scope and appropriate permissions when you invite it.

## Updating dependencies
If you need newer packages, update `Discord.Net` or `Microsoft.Extensions.DependencyInjection` versions in the `.csproj` and run:

```bash
dotnet restore
```
