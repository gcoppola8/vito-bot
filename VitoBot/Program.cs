using Discord;
using Discord.WebSocket;

using VitoBot;


internal class Program
{
    static private readonly string BOT_TOKEN = Environment.GetEnvironmentVariable("BOT_TOKEN_ENV");
    
    private static async Task Main(string[] args)
    {

        validateConfig();
        
        AzureGameService azureGameService = new AzureGameService();

        VitoBotInstance bot = new VitoBotInstance(BOT_TOKEN, azureGameService);
        await bot.Connect();
    }

    private static void validateConfig()
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BOT_TOKEN_ENV")))
        {
            throw new ArgumentException("Missing environment variable: BOT_TOKEN_ENV");
        }
        
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RG_NAME")))
        {
            throw new ArgumentException("Missing environment variable: RG_NAME");
        }
        
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("VM_NAME")))
        {
            throw new ArgumentException("Missing environment variable: VM_NAME");
        }
        
    }
}