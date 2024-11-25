using Microsoft.AspNetCore.SignalR.Client;
using NAudio.Wave;

namespace Chat;

public sealed class VoiceHub(ITool _tool)
{
    private static HubConnection? _connection;
    private static WaveInEvent? _waveIn;
    private static WaveOutEvent? _waveOut;
    private static BufferedWaveProvider? _bufferedWaveProvider;

    public async Task Do()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5181/voicehub")
            .Build();
        _waveIn = new WaveInEvent();
        _waveIn.WaveFormat = new WaveFormat(44100, 1); // 44.1kHz, mono
        _waveIn.DataAvailable += WaveIn_DataAvailable;

        _waveOut = new WaveOutEvent();
        _bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(44100, 1));
        _waveOut.Init(_bufferedWaveProvider);
        _waveOut.Play();

        _connection.On<string, byte[]>("ReceiveVoiceData", (userId, audioData) =>
        {
            _bufferedWaveProvider.AddSamples(audioData, 0, audioData.Length);
        });

        await _connection.StartAsync();
        _tool.WriteLine("Bağlantı kuruldu!");

        _tool.Write("Oda adı giriniz: ");
        string roomName = _tool.ReadLine();
        await _connection.InvokeAsync("JoinRoom", roomName);
        _tool.WriteLine($"{roomName} odasına katıldınız.");

        _waveIn.StartRecording();

        _tool.WriteLine("Çıkmak için bir tuşa basın...");
        _tool.ReadKey();

        await _connection.InvokeAsync("LeaveRoom");
        _waveIn.StopRecording();
        _waveOut.Stop();
        await _connection.StopAsync();
    }

    private static async void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
    {
        if(_connection != null)
        {
            if (_connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SendVoiceData", e.Buffer);
            }
        }
    }
}
