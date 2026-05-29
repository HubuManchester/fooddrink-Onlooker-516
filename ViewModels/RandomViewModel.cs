using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FoodPicker.Helpers;
using FoodPicker.Models;
using FoodPicker.Services;

namespace FoodPicker.ViewModels;

public class RandomViewModel : INotifyPropertyChanged
{
    private readonly FoodDataService _foodService;
    private FoodItem _currentFood;
    private DateTime _lastShakeTime = DateTime.MinValue;

    private string _cravingText = "";

    public RandomViewModel()
    {
        _foodService = new FoodDataService();
        _currentFood = _foodService.GetRandomFood();

        RandomPickCommand = new Command(OnRandomPick);
        AddFavoriteCommand = new Command(OnAddFavorite);
        SpeakCravingCommand = new Command(async () => await OnSpeakCraving());
    }

    // 用户输入的美食名称
    public string CravingText
    {
        get => _cravingText;
        set { _cravingText = value; OnPropertyChanged(); }
    }

    public ICommand SpeakCravingCommand { get; }

    public FoodItem CurrentFood
    {
        get => _currentFood;
        set
        {
            _currentFood = value;
            OnPropertyChanged();
        }
    }

    public ICommand RandomPickCommand { get; }
    public ICommand AddFavoriteCommand { get; }

    // 当摇晃发生时通知 View 做动画
    public event Action? ShakeHappened;

    public void StartShakeDetection()
    {
        if (Accelerometer.Default.IsMonitoring) return;

        Accelerometer.Default.ShakeDetected += OnShakeDetected;
        Accelerometer.Default.Start(SensorSpeed.Game);
    }

    public void StopShakeDetection()
    {
        if (!Accelerometer.Default.IsMonitoring) return;

        Accelerometer.Default.ShakeDetected -= OnShakeDetected;
        Accelerometer.Default.Stop();
    }

    private void OnShakeDetected(object? sender, EventArgs e)
    {
        // 防抖：800ms 内不重复触发
        var now = DateTime.UtcNow;
        if ((now - _lastShakeTime).TotalMilliseconds < 800) return;
        _lastShakeTime = now;

        DoRandomPick();
        SoundHelper.PlayShakeSound();
        ShakeHappened?.Invoke();

        try { HapticFeedback.Default.Perform(HapticFeedbackType.LongPress); }
        catch { /* 模拟器可能不支持，忽略 */ }
    }

    private void OnRandomPick()
    {
        DoRandomPick();
        SoundHelper.PlayShakeSound();
        ShakeHappened?.Invoke();

        try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
        catch { /* 模拟器可能不支持，忽略 */ }
    }

    private void DoRandomPick()
    {
        CurrentFood = _foodService.GetRandomFoodExcluding(CurrentFood);

        // 语音播报当前推荐
        try
        {
            TextToSpeech.Default.SpeakAsync($"今天吃——{CurrentFood.Name}");
        }
        catch { /* TTS 失败不影响功能 */ }
    }

    private async void OnAddFavorite()
    {
        try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
        catch { /* 模拟器可能不支持，忽略 */ }

        var favService = FavoritesService.Instance;

        if (favService.IsFavorite(CurrentFood))
        {
            await Shell.Current.DisplayAlert("提示", $"「{CurrentFood.Name}」已经在收藏夹里了~", "知道了");
        }
        else
        {
            favService.Add(CurrentFood);
            await Shell.Current.DisplayAlert("已收藏", $"「{CurrentFood.Name}」已加入收藏夹！", "好的");
        }
    }

    private async Task OnSpeakCraving()
    {
        try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
        catch { /* 忽略 */ }

        // 输入验证
        var (isValid, errorMsg) = ValidateCraving();
        if (!isValid)
        {
            await Shell.Current.DisplayAlert("提示", errorMsg, "知道了");
            return;
        }

        // 验证通过 → TTS 朗读
        try
        {
            await TextToSpeech.Default.SpeakAsync($"你想吃——{CravingText.Trim()}");
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("出错了",
                "语音播报失败。请检查：\n1. 手机是否安装了文字转语音引擎\n2. 系统音量是否已开启\n3. 媒体音量是否静音", "好的");
        }
    }

    /// 输入验证：不为空、长度检查
    private (bool isValid, string message) ValidateCraving()
    {
        if (string.IsNullOrWhiteSpace(CravingText))
            return (false, "请输入你想吃的美食名称~");

        if (CravingText.Trim().Length < 2)
            return (false, "名称太短了，至少输入 2 个字哦");

        return (true, "");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
