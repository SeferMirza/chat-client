using Chat;
using Chat.Commands;
using Chat.Exceptions;
using Chat.MenuOptions;
using Chat.Windows;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddTransient<Hub>()
    .AddKeyedSingleton<Window, MainMenu>(nameof(MainMenu))
    .AddKeyedSingleton<Window, Servers>(nameof(Servers))
    .AddKeyedSingleton<Window, ChatConnection>(nameof(ChatConnection))
    .AddSingleton<IWindowManager, WindowManager>()
    .AddSingleton<ICommand, ServerDetail>()
    .AddSingleton<ICommand, ExitServer>()
    .AddSingleton<ICommand, Help>()
    .AddSingleton<CommandRegistry>()
    .AddSingleton<IMainMenuOptions, ServersOption>()
    .AddSingleton<IMainMenuOptions, ChatOption>()
    .AddSingleton<App>()
    .AddSingleton<ITool, ConsoleTool>()
    .AddSingleton<IRouter, Router>()
    .AddSingleton<IConsoleInfo, ConsoleInfo>()
    .AddSingleton<HttpClient>()
    .AddSingleton<ServerService>()
    .AddSingleton<IExceptionHandler, GlobalExceptionHandler>()
    .BuildServiceProvider();

var app = serviceProvider.GetRequiredService<App>();

await app.Run();
