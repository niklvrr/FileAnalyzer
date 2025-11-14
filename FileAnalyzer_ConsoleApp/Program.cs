using FileAnalyzer.ConsoleApp.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace FileAnalyzer.ConsoleApp;

class Program
{
    static async Task Main(string[] args)
    {
        // Настройка Dependency Injection контейнера
        var services = new ServiceCollection();
        services.AddApplicationServices();
        var serviceProvider = services.BuildServiceProvider();

        // Получаем экземпляр приложения через DI и запускаем
        var app = serviceProvider.GetRequiredService<ConsoleApplication>();
        await app.RunAsync();
    }
}
