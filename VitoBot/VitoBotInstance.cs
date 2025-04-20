using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VitoBot;


internal class VitoBotInstance
{
    private static DiscordSocketClient _client;
    private static AzureGameService _gameService;
    private string _botToken;

    public VitoBotInstance(string botToken, AzureGameService gameService)
    {
        _gameService = gameService;
        _botToken = botToken;
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages
        });

        _client.Log += LogAsync;
        _client.Ready += ReadyAsync;
        _client.SlashCommandExecuted += HandleSlashCommandAsync;
    }

    public async Task Connect()
    {
        await _client.LoginAsync(TokenType.Bot, _botToken);
        await _client.StartAsync();

        // Keep bot running
        await Task.Delay(-1);
    }

    private static Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log);
        return Task.CompletedTask;
    }

    private static async Task ReadyAsync()
    {
        var testCommand = new SlashCommandBuilder()
                .WithName("hello")
                .WithDescription("Responds with some text").Build();

        var serverCommand = new SlashCommandBuilder()
            .WithName("server")
            .WithDescription("Manage the gaming server")
            .AddOption(new SlashCommandOptionBuilder()
                .WithName("op")
                .WithDescription("Operation to execute start|stop")
                .AddChoice("Start", 1)
                .AddChoice("Stop", 2)
                .WithType(ApplicationCommandOptionType.Integer)
            ).Build();

        foreach (var guild in _client.Guilds)
        {
            await guild.CreateApplicationCommandAsync(testCommand);
            await guild.CreateApplicationCommandAsync(serverCommand);

            Console.WriteLine($"Registered commands for guild: {guild.Name}");
        }

        Console.WriteLine($"Registered commands for {_client.Guilds.Count} guilds.");

    }

    private static async Task HandleSlashCommandAsync(SocketSlashCommand command)
    {
        if (command.CommandName == "test") await command.RespondAsync("Hello! This is a response to /test.");

        switch (command.Data.Name)
        {
            case "server":
                await HandleServerCommand(command);
                break;
            default:
                await command.RespondAsync("I don't know how to handle this command.");
                break;
        }

    }

    private static async Task HandleServerCommand(SocketSlashCommand command)
    {
        var opObject = command.Data.Options.FirstOrDefault();
        if (opObject == null)
        {
            await command.RespondAsync("I don't know how to handle this command.");
            return;
        }

        var op = (Int64) opObject.Value;

        if (op == 1) //START
        {
            _gameService.StartMinecraftServer();
            await command.RespondAsync("Minecraft server started");
        }
        else if (op == 2) //STOP
        {
            _gameService.StopMinecraftServer();
            await command.RespondAsync("Minecraft server stopped");
        }
        else
        {
            await command.RespondAsync("I don't know how to handle this command.");
            return;
        }
    }
}
