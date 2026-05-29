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
        SearchFoodCommand = new Command(async () => await OnSearchFood());
    }

    // 用户输入的美食名称
    public string CravingText
    {
        get => _cravingText;
        set { _cravingText = value; OnPropertyChanged(); }
    }

    public ICommand SearchFoodCommand { get; }

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
        var now = DateTime.UtcNow;
        if ((now - _lastShakeTime).TotalMilliseconds < 800) return;
        _lastShakeTime = now;

        DoRandomPick();
        SoundHelper.PlayShakeSound();
        ShakeHappened?.Invoke();

        try { HapticFeedback.Default.Perform(HapticFeedbackType.LongPress); }
        catch { /* 忽略 */ }
    }

    private void OnRandomPick()
    {
        DoRandomPick();
        SoundHelper.PlayShakeSound();
        ShakeHappened?.Invoke();

        try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
        catch { /* 忽略 */ }
    }

    private void DoRandomPick()
    {
        CurrentFood = _foodService.GetRandomFoodExcluding(CurrentFood);
    }

    private async void OnAddFavorite()
    {
        try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
        catch { /* 忽略 */ }

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

    private async Task OnSearchFood()
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

        // 在53种食物中搜索
        var keyword = CravingText.Trim();
        var found = _foodService.SearchFood(keyword);

        if (found != null && found.Name != CurrentFood.Name)
        {
            CurrentFood = found;
            SoundHelper.PlayShakeSound();
            ShakeHappened?.Invoke();
        }
        else if (found != null)
        {
            // 搜到的就是当前显示的
            await Shell.Current.DisplayAlert("找到了！", $"「{found.Name}」已经显示在卡片上了~", "好的");
        }
        else
        {
            // 没搜到，随机推荐一个当惊喜
            CurrentFood = _foodService.GetRandomFoodExcluding(CurrentFood);
            SoundHelper.PlayShakeSound();
            ShakeHappened?.Invoke();
            await Shell.Current.DisplayAlert("没找到", $"没有搜到「{keyword}」相关的美食，为你随机推荐了一个~", "好的");
        }
    }

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
