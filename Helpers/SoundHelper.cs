using Plugin.Maui.Audio;

namespace FoodPicker.Helpers;

/// 用程序生成一个“签筒摇晃”音效 —— 模拟木签在竹筒里碰撞的声音
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
            // 音效失败不影响功能
        }
        finally
        {
            _isPlaying = false;
        }
    }

    private static string GenerateRattleWav()
    {
        int sampleRate = 44100;
        double duration = 0.65;          // 总时长
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

        // ---- 生成签筒摇晃音效 ----
        // 思路：在随机时间点触发短促的“木签撞击”脉冲，叠加上木质共鸣音
        double[] buffer = new double[sampleCount];

        // 预计算一串随机撞击事件
        int eventCount = 45;  // 撞击次数
        for (int e = 0; e < eventCount; e++)
        {
            double t0 = (_rng.NextDouble() * 0.6 + 0.05) * duration;  // 撞击发生时间 (避开首尾)
            double impactStrength = _rng.NextDouble() * 0.4 + 0.6;     // 0.6~1.0 随机强度

            // 撞击脉冲中心频率（模拟不同粗细木签的不同音高）
            double clickFreq = 1800 + _rng.NextDouble() * 2500;

            int startIdx = (int)(t0 * sampleRate);
            int clickLen = (int)(0.025 * sampleRate);  // 每次撞击持续约 25ms

            for (int i = 0; i < clickLen && startIdx + i < sampleCount; i++)
            {
                double t = (double)i / sampleRate;
                double clickEnv = Math.Exp(-t * 200);  // 快速衰减
                double wave = Math.Sin(2 * Math.PI * clickFreq * t);
                buffer[startIdx + i] += wave * clickEnv * impactStrength * 0.35;
            }
        }

        // 添加持续的低频“筒身共鸣”噪音（模拟竹筒的隆隆声）
        for (int i = 0; i < sampleCount; i++)
        {
            double t = (double)i / sampleRate;
            // 低频随机噪声
            double lowRumble = (Math.Sin(2 * Math.PI * 120 * t + _rng.NextDouble() * 0.3)
                              + Math.Sin(2 * Math.PI * 230 * t + _rng.NextDouble() * 0.3)) * 0.06;

            // 中高频“沙沙”声（木签互相摩擦）
            double hiss = (_rng.NextDouble() * 2 - 1) * 0.04;

            // 总体衰减包络（先强后弱，模拟摇晃渐停）
            double masterEnv;
            if (t < 0.1)
                masterEnv = t / 0.1;           // 0→0.1s 渐入
            else if (t < 0.5)
                masterEnv = 1.0;               // 0.1→0.5s 保持
            else
                masterEnv = 1.0 - (t - 0.5) / 0.15;  // 0.5→0.65s 渐弱

            masterEnv = Math.Max(0, Math.Min(1, masterEnv));

            buffer[i] += lowRumble + hiss;
            buffer[i] *= masterEnv;
        }

        // 防止削波
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
