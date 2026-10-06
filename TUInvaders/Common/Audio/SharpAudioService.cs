using SharpAudio;
using SharpAudio.Codec;
using Spectre.Console;


namespace TUInvaders.Common.Audio;

public sealed class SharpAudioService : IAudioService, IDisposable
{
    private readonly AudioEngine? _engine;
    private readonly string _assetsPath = AppContext.BaseDirectory;
    private readonly Dictionary<string, string> _soundPaths = [];
    private readonly Dictionary<string, CachedSound> _cachedSounds = [];

    public SharpAudioService()
    {
        _engine = AudioEngine.CreateDefault();

        if (_engine == null)
        {
            AnsiConsole.MarkupLine("[red]⚠️ WARNING: Error initializing Audio Engine. The game will continue withouth soundas.[/]");
        }
    }

    public void RegisterSound(string name, string filePath)
    {
        if (File.Exists(filePath))
        {
            _soundPaths[name] = filePath;
        }
    }

    public void Play(string soundName)
    {
        if (_engine == null) return;

        if (_soundPaths.TryGetValue(soundName, out var filePath))
        {
            var fullPath = $"{_assetsPath}{filePath}";
            if (!File.Exists(fullPath)) return;

            lock (_cachedSounds)
            {
                if (!_cachedSounds.TryGetValue(soundName, out var cached))
                {
                    cached = new CachedSound(File.ReadAllBytes(fullPath));
                    _cachedSounds[soundName] = cached;
                }

                cached.Play(_engine);
            }
        }
    }

    public void Dispose()
    {
        lock (_cachedSounds)
        {
            foreach (var cached in _cachedSounds.Values)
            {
                cached.Dispose();
            }
            _cachedSounds.Clear();
        }

        try { _engine?.Dispose(); } catch (SharpAudioException) { }
    }

    private sealed class CachedSound(byte[] wavData) : IDisposable
    {
        private readonly byte[] _wavData = wavData;
        private SoundStream? _activeStream;

        public void Play(AudioEngine engine)
        {
            _activeStream?.Dispose();
            _activeStream = null;

            var stream = new SoundStream(new MemoryStream(_wavData), engine);
            _activeStream = stream;

            try { stream.Volume = 0.6f; } catch { return; }
            stream.Play();
        }

        public void Dispose()
        {
            _activeStream?.Dispose();
            _activeStream = null;
        }
    }
}
