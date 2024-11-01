using Chat;

var chatHub = new Hub();

try
{
    chatHub.Subscribe();

    await chatHub.Connect();

    // Kullanıcı bilgileri
    Console.Write("Kullanıcı Adınız: ");
    string? username = Console.ReadLine();

    Console.Write("Server ID: ");
    Guid serverId = Guid.Parse(Console.ReadLine()!);

    Console.Write("Room ID: ");
    Guid roomId = Guid.Parse(Console.ReadLine()!);

    await chatHub.JoinRoom(serverId, roomId);

    while (true)
    {
        string message = Console.ReadLine()!;

        if (message.ToLower() == "exit")
            break;

        int cursorTop = Console.CursorTop - 1;
        Console.SetCursorPosition(0, cursorTop);
        Console.Write(new string(' ', Console.WindowWidth));

        // İmleci tekrar temizlenen satırın başına yerleştirin
        Console.SetCursorPosition(0, cursorTop);

        await chatHub.SendMessage(message, roomId);

    }
}
catch (Exception ex)
{
    Console.WriteLine($"Hata: {ex.Message} - {ex.InnerException.Message}");
}
finally
{
    await chatHub.DisposeAsync();
}
