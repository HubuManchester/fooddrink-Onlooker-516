using Plugin.Maui.Audio;

namespace FoodPicker.Helpers;

/// 
public static class SoundHelper
{
    private static bool _isPlaying;
    private static readonly Random _rng = new();

    public static async void PlayShakeSound()
    {
        if (_isPlaying) return;
        _isPlaying = true;

        try
        {
            var filePath = GenerateRattleWav();
            var player = AudioManager.Current.CreatePlayer(filePath);
            player.Play();
            await Task.Delay(800);
        }
        catch
        {
            // 
        }
        finally
        {
            _isPlaying = false;
        }
    }

    private static string GenerateRattleWav()
    {
        int sampleRate = 44100;
        double duration = 0.65;          // Total duration in seconds
        int sampleCount = (int)(sampleRate * duration);

        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        // ---- WAV header ----
        writer.Write(new[] { 'R', 'I', 'F', 'F' });
        writer.Write(36 + sampleCount * 2);
        writer.Write(new[] { 'W', 'A', 'V', 'E' });

        writer.Write(new[] { 'f', 'm', 't', ' ' });
        writer.Write(16);
        writer.Write((short)1);          // PCM
        writer.Write((short)1);          // mono
        writer.Write(sampleRate);
        writer.Write(sampleRate * 2);
        writer.Write((short)2);
        writer.Write((short)16);

        writer.Write(new[] { 'd', 'a', 't', 'a' });
        writer.Write(sampleCount * 2);

        // 
        // 
        double[] buffer = new double[sampleCount];

        // 
        int eventCount = 45;  // Number of impact events
        for (int e = 0; e < eventCount; e++)
        {
            double t0 = (_rng.NextDouble() * 0.6 + 0.05) * duration;  // Impact time (avoid start and end)
      double impactStrength = _rng.NextDouble() * 0.4 + 0.6;   // 0.6~1.0 

            // 
            double clickFreq = 1800 + _rng.NextDouble() * 2500;

            int startIdx = (int)(t0 * sampleRate);
      int clickLen = (int)(0.025 * sampleRate); // 25ms

            for (int i = 0; i < clickLen && startIdx + i < sampleCount; i++)
            {
                double t = (double)i / sampleRate;
        double clickEnv = Math.Exp(-t * 200); // 
                double wave = Math.Sin(2 * Math.PI * clickFreq * t);
                buffer[startIdx + i] += wave * clickEnv * impactStrength * 0.35;
            }
        }

        // 
        for (int i = 0; i < sampleCount; i++)
        {
            double t = (double)i / sampleRate;
            // 
            double lowRumble = (Math.Sin(2 * Math.PI * 120 * t + _rng.NextDouble() * 0.3)
                              + Math.Sin(2 * Math.PI * 230 * t + _rng.NextDouble() * 0.3)) * 0.06;

            // 
            double hiss = (_rng.NextDouble() * 2 - 1) * 0.04;

            // 
            double masterEnv;
            if (t < 0.1)
        masterEnv = t / 0.1;      // 0→0.1s 
            else if (t < 0.5)
        masterEnv = 1.0;        // 0.1→0.5s 
            else
        masterEnv = 1.0 - (t - 0.5) / 0.15; // 0.5→0.65s 

            masterEnv = Math.Max(0, Math.Min(1, masterEnv));

            buffer[i] += lowRumble + hiss;
            buffer[i] *= masterEnv;
        }

        // 
        double max = 0;
        for (int i = 0; i < sampleCount; i++)
            if (Math.Abs(buffer[i]) > max) max = Math.Abs(buffer[i]);
        double gain = max > 0 ? 28000 / max : 1;

        for (int i = 0; i < sampleCount; i++)
        {
            short sample = (short)Math.Max(-32768, Math.Min(32767, buffer[i] * gain));
            writer.Write(sample);
        }

        var filePath = Path.Combine(FileSystem.CacheDirectory, "shake_rattle.wav");
        File.WriteAllBytes(filePath, ms.ToArray());
        return filePath;
    }
}
