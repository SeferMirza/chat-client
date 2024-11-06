using Chat;
using Chat.MenuOptions;
using Chat.Windows;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddSingleton<Window, MainMenu>()
    .AddSingleton<Window, Servers>()
    .AddSingleton<Window, ChatConnection>()
    .AddSingleton<WindowStack>()
    .AddSingleton<IMainMenuOptions, ServersOption>()
    .AddSingleton<IMainMenuOptions, ChatOption>()
    .AddSingleton<App>()
    .AddSingleton<ITool, ConsoleTool>()
    .AddSingleton<HttpClient>()
    .BuildServiceProvider();

var app = serviceProvider.GetRequiredService<App>();

await app.Run();
