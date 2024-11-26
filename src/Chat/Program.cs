using Chat;
using Chat.Commands;
using Chat.Consoles;
using Chat.Exceptions;
using Chat.Hubs;
using Chat.MenuOptions;
using Chat.Windows;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddTransient<ChatHub>()
    .AddTransient<VoiceHub>()
    .AddKeyedSingleton<Window, MainMenu>(nameof(MainMenu))
    .AddKeyedSingleton<Window, Servers>(nameof(Servers))
    .AddKeyedSingleton<Window, ChatConnection>(nameof(ChatConnection))
    .AddKeyedSingleton<Window, VoiceConnection>(nameof(VoiceConnection))
    .AddSingleton<IWindowManager, WindowManager>()
    .AddSingleton<ICommand, ServerDetail>()
    .AddSingleton<ICommand, ExitServer>()
    .AddSingleton<ICommand, Help>()
    .AddSingleton<CommandRegistry>()
    .AddSingleton<IMainMenuOptions, VoiceOption>()
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
