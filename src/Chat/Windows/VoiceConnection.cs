namespace Chat.Windows;

public class VoiceConnection(VoiceHub voiceHub) : Window
{
    public override string Name => nameof(VoiceConnection);

    public async override Task Open()
    {
        await voiceHub.Do();
    }
}
