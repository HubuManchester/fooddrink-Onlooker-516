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

    public RandomViewModel()
    {
        _foodService = new FoodDataService();
        _currentFood = _foodService.GetRandomFood();

        RandomPickCommand = new Command(OnRandomPick);
        AddFavoriteCommand = new Command(OnAddFavorite);
    }

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
    }

    private void OnRandomPick()
    {
        DoRandomPick();
        SoundHelper.PlayShakeSound();
        ShakeHappened?.Invoke();
    }

    private void DoRandomPick()
    {
        CurrentFood = _foodService.GetRandomFoodExcluding(CurrentFood);
    }

    private async void OnAddFavorite()
    {
        await Shell.Current.DisplayAlert("已收藏", $"「{CurrentFood.Name}」已加入收藏夹！", "好的");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
