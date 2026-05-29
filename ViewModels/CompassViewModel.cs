using System.ComponentModel;
using System.Runtime.CompilerServices;
using FoodPicker.Models;
using FoodPicker.Services;

namespace FoodPicker.ViewModels;

public class CompassViewModel : INotifyPropertyChanged
{
    private readonly FoodDataService _foodService;
    private double _heading;

    public CompassViewModel()
    {
        _foodService = new FoodDataService();
    }

    private double _compassRotation;
    public double CompassRotation
    {
        get => _compassRotation;
        set { _compassRotation = value; OnPropertyChanged(); }
    }

    private string _directionText = "Detecting direction...";
    public string DirectionText
    {
        get => _directionText;
        set { _directionText = value; OnPropertyChanged(); }
    }

    private string _directionEmoji = "🧭";
    public string DirectionEmoji
    {
        get => _directionEmoji;
        set { _directionEmoji = value; OnPropertyChanged(); }
    }

    private string _headingDegrees = "";
    public string HeadingDegrees
    {
        get => _headingDegrees;
        set { _headingDegrees = value; OnPropertyChanged(); }
    }

    private FoodItem _recommendedFood;
    public FoodItem RecommendedFood
    {
        get => _recommendedFood;
        set { _recommendedFood = value; OnPropertyChanged(); }
    }

    private bool _hasReading;
    public bool HasReading
    {
        get => _hasReading;
        set { _hasReading = value; OnPropertyChanged(); }
    }

    // 上次旋转动画完成后的角度，用于增量动画
    private double _lastAnimHeading;
    private bool _firstReading = true;
    private double _smoothedDisplay;
    private int _lastDirectionIdx = -1;

    // 8-direction names and categories
    private static readonly string[] DirectionNames =
        { "North", "Northeast", "East", "Southeast", "South", "Southwest", "West", "Northwest" };

    private static readonly string[] DirectionEmojis =
        { "⬆️", "↗️", "➡️", "↘️", "⬇️", "↙️", "⬅️", "↖️" };

    private static readonly string[] DirectionCategories =
        { "Northern Snacks", "Northeastern Cuisine", "Shanghai Delicacies", "Cantonese Cuisine",
          "Cantonese Cuisine", "Sichuan Cuisine", "Northwest Flavors", "Northwest Flavors" };

    // Notify View to animate when heading changes
    public event Action<double>? HeadingChanged;

    public void Start()
    {
        if (!Compass.Default.IsSupported)
        {
            DirectionText = "Compass not supported on this device";
            return;
        }

        if (Compass.Default.IsMonitoring) return;

        Compass.Default.ReadingChanged += OnReadingChanged;
        Compass.Default.Start(SensorSpeed.UI);
    }

    public void Stop()
    {
        if (!Compass.Default.IsMonitoring) return;

        Compass.Default.ReadingChanged -= OnReadingChanged;
        Compass.Default.Stop();
    }

    private void OnReadingChanged(object? sender, CompassChangedEventArgs e)
    {
        var rawHeading = e.Reading.HeadingMagneticNorth;

        // 指数平滑滤波，减少抖动
        const double smoothing = 0.12;  // 越小越平滑
        if (_firstReading)
        {
            _heading = rawHeading;
            _firstReading = false;
        }
        else
        {
            _heading = _heading * (1 - smoothing) + rawHeading * smoothing;
        }

        // 变动小于 2 度就忽略，减少无意义的刷新
        if (Math.Abs(_heading - _smoothedDisplay) < 2) return;
        _smoothedDisplay = _heading;

        var idx = GetDirectionIndex(_heading);

        DirectionText = $"{DirectionEmojis[idx]}  Facing {DirectionNames[idx]}";
        HeadingDegrees = $"{_heading:F0}°";
        HasReading = true;

        // 方位变了才换推荐
        if (idx != _lastDirectionIdx)
        {
            _lastDirectionIdx = idx;
            var category = DirectionCategories[idx];
            RecommendByCategory(category);
        }

        // 通知 View 做旋转动画（增量角度）
        double delta = _heading - _lastAnimHeading;
        if (delta > 180) delta -= 360;
        if (delta < -180) delta += 360;

        _lastAnimHeading = _heading;
        CompassRotation += delta;
        HeadingChanged?.Invoke(delta);
    }

    private int GetDirectionIndex(double h)
    {
        // 8方位：每45度一个区间，北从 -22.5~22.5（即337.5~22.5）
        var normalized = (h + 22.5) % 360;
        if (normalized < 0) normalized += 360;
        return (int)(normalized / 45) % 8;
    }

    private void RecommendByCategory(string category)
    {
        // 从 FoodDataService 随机挑一个该分类的食物
        FoodItem? pick = null;
        for (int i = 0; i < 50; i++)
        {
            var f = _foodService.GetRandomFood();
            if (f.Category == category)
            {
                pick = f;
                break;
            }
        }
        RecommendedFood = pick ?? _foodService.GetRandomFood();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
