using Chat.Exceptions;
using Microsoft.AspNetCore.SignalR.Client;
using NAudio.Wave;

namespace Chat.Hubs;

public sealed class VoiceHub(ServerService serverService)
{
    static HubConnection? _connection;
    static WaveInEvent? _waveIn;
    static WaveOutEvent? _waveOut;
    static BufferedWaveProvider? _bufferedWaveProvider;
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

        _connection.On<User>("UserJoined", (user) =>
        {
            serverService.CurrentServer!.ConnectedUsers.Add(user);
        });

        _connection.On<User>("UserDisconnected", (user) =>
        {
            serverService.CurrentServer!.ConnectedUsers.Remove(user);
        });

        await _connection.InvokeAsync("Connect", username, _serverId);

        _waveIn.StartRecording();
    }

    public async Task DisposeAsync()
    {
        if (_connection != null)
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
