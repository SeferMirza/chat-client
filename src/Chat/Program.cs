using Chat;
using Chat.Commands;
using Chat.Consoles;
using Chat.Exceptions;
using Chat.Hubs;
using Chat.Windows;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddTransient<ChatHub>()
    .AddTransient<VoiceHub>()
    .AddKeyedSingleton<Window, Servers>(nameof(Servers))
    .AddKeyedSingleton<Window, ChatConnection>(ServerType.Chat.ToString())
    .AddKeyedSingleton<Window, VoiceConnection>(ServerType.Voice.ToString())
    .AddSingleton<IWindowManager, WindowManager>()
    .AddSingleton<ICommand, ServerInfo>()
    .AddSingleton<ICommand, ExitServer>()
    .AddSingleton<ICommand, Help>()
    .AddSingleton<CommandRegistry>()
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
