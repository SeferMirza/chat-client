using Microsoft.AspNetCore.SignalR.Client;
using NAudio.Wave;

namespace Chat.Hubs;

public sealed class VoiceHub(ServerService serverService)
{
    private static HubConnection? _connection;
    private static WaveInEvent? _waveIn;
    private static WaveOutEvent? _waveOut;
    private static BufferedWaveProvider? _bufferedWaveProvider;

    static Guid _serverId = Guid.Empty;

    private HubConnection CreateConnection()
    {
        return new HubConnectionBuilder()
            .WithUrl(Path.Join(serverService.BaseUrl, "/voicehub"))
            .Build();
    }

    public async Task Connect(Guid serverId, string username)
    {
        _serverId = serverId;
        _connection ??= CreateConnection();

        await _connection.StartAsync();

        var voiceFormat = new WaveFormat(48000, 24, 1);  // 48kHz, 24-bit, stereo

        _waveIn = new()
        {
            WaveFormat = voiceFormat
        };
        _waveIn.DataAvailable += WaveIn_DataAvailable!;

        _waveOut = new WaveOutEvent();
        _bufferedWaveProvider = new BufferedWaveProvider(voiceFormat);
        _waveOut.Init(_bufferedWaveProvider);
        _waveOut.Play();

        _connection.On<string, byte[]>("ReceiveVoiceData", (userId, audioData) =>
        {
            _bufferedWaveProvider.AddSamples(audioData, 0, audioData.Length);
        });

        await _connection.InvokeAsync("Connect", username, _serverId);

        _waveIn.StartRecording();
    }

    public async Task DisposeAsync()
    {
        if(_connection != null)
        {
            _waveIn?.StopRecording();
            _waveOut?.Stop();

            _connection.Remove("ReceiveVoiceData");
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            _connection = null;

            _waveIn?.Dispose();
            _waveOut?.Dispose();
        }
    }

    private static void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
    {
        _connection?.InvokeAsync("SendVoiceData", _serverId, e.Buffer);
    }
}
