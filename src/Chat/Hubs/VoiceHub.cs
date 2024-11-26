using Chat.Consoles;
using Chat.Exceptions;
using Microsoft.AspNetCore.SignalR.Client;
using NAudio.Wave;

namespace Chat.Hubs;

public sealed class VoiceHub(ITool _tool)
{
    private static HubConnection? _connection;
    private static WaveInEvent? _waveIn;
    private static WaveOutEvent? _waveOut;
    private static BufferedWaveProvider? _bufferedWaveProvider;
    static Guid _serverId = Guid.Empty;

    private HubConnection CreateConnection()
    {
        return new HubConnectionBuilder()
            .WithUrl("http://192.168.1.113:5181/voicehub")
            .Build();
    }

    public async Task Connect(Guid serverId, string username)
    {
        _serverId = serverId;
        _connection ??= CreateConnection();

        await _connection.StartAsync();

        _waveIn = new WaveInEvent();
        _waveIn.WaveFormat = new WaveFormat(44100, 1); // 44.1kHz, mono
        _waveIn.DataAvailable += WaveIn_DataAvailable!;

        _waveOut = new WaveOutEvent();
        _bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(44100, 1));
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
        }
    }

    private static async void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
    {
        if(_connection != null)
        {
            if (_connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SendVoiceData", _serverId, e.Buffer);
            }
        }
    }
}
