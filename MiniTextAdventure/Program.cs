using Microsoft.Extensions.DependencyInjection;
using MiniTextAdventure;

    public class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddHttpClient<ApiClient>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:5274");
        });

        services.AddSingleton<Game>();

        var provider = services.BuildServiceProvider();

        var api = provider.GetRequiredService<ApiClient>();

        Console.WriteLine("Login vereist!");
        Console.Write("Username: ");
        string username = Console.ReadLine();

        Console.Write("Password: ");
        string password = Console.ReadLine();

        bool ok = await api.Login(username, password);

        if (!ok)
        {
            Console.WriteLine("Login mislukt.");
            return;
        }

        Console.WriteLine("Login gelukt!");

        var game = new Game(api);
        await game.Start();
    }
}
