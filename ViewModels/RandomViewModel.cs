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
        UpdateFavoriteState();

        RandomPickCommand = new Command(OnRandomPick);
        AddFavoriteCommand = new Command(OnAddFavorite);
        SearchFoodCommand = new Command(async () => await OnSearchFood());
        ShareCommand = new Command(async () => await OnShare());
    }

    // User's food craving input
    public string CravingText
    {
        get => _cravingText;
        set { _cravingText = value; OnPropertyChanged(); }
    }

    public ICommand SearchFoodCommand { get; }
    public ICommand ShareCommand { get; }

    public FoodItem CurrentFood
    {
        get => _currentFood;
        set
        {
            _currentFood = value;
            OnPropertyChanged();
            UpdateFavoriteState();
        }
    }

    private bool _isFavorited;
    public bool IsFavorited
    {
        get => _isFavorited;
        set { _isFavorited = value; OnPropertyChanged(); OnPropertyChanged(nameof(FavBtnText)); }
    }

    public string FavBtnText => IsFavorited ? "♥ Saved" : "♥ Save";

    public ICommand RandomPickCommand { get; }
    public ICommand AddFavoriteCommand { get; }

    // Notify View to play animation when shake happens
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
        catch { /* Silently ignore if not supported */ }
    }

    private void OnRandomPick()
    {
        DoRandomPick();
        SoundHelper.PlayShakeSound();
        ShakeHappened?.Invoke();

        try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
        catch { /* Silently ignore if not supported */ }
    }

    private void DoRandomPick()
    {
        CurrentFood = _foodService.GetRandomFoodExcluding(CurrentFood);
    }

    private void UpdateFavoriteState()
    {
        IsFavorited = FavoritesService.Instance.IsFavorite(CurrentFood);
    }

    private async Task OnShare()
    {
        try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
        catch { }

        var text = $"{CurrentFood.Emoji} How about 「{CurrentFood.Name}」today?\n"
                 + $"{CurrentFood.Category} - {CurrentFood.Description}\n"
                 + $"💡 Pairing: {CurrentFood.Pairing}";

        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = text,
            Title = $"FoodPicker - {CurrentFood.Name}"
        });
    }

    private async void OnAddFavorite()
    {
        try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
        catch { /* Silently ignore */ }

        var favService = FavoritesService.Instance;

        if (favService.IsFavorite(CurrentFood))
        {
            await Shell.Current.DisplayAlert("Already Saved", $"「{CurrentFood.Name}」is already in your favourites!", "OK");
        }
        else
        {
            favService.Add(CurrentFood);
            UpdateFavoriteState();
            await Shell.Current.DisplayAlert("Saved!", $"「{CurrentFood.Name}」has been added to favourites!", "OK");
        }
    }

    private async Task OnSearchFood()
    {
        try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
        catch { /* Silently ignore */ }

        // Input validation
        var (isValid, errorMsg) = ValidateCraving();
        if (!isValid)
        {
            await Shell.Current.DisplayAlert("Oops", errorMsg, "OK");
            return;
        }

        // Search across 53 food items
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
            await Shell.Current.DisplayAlert("Found it!", $"「{found.Name}」is already on the card!", "OK");
        }
        else
        {
            CurrentFood = _foodService.GetRandomFoodExcluding(CurrentFood);
            SoundHelper.PlayShakeSound();
            ShakeHappened?.Invoke();
            await Shell.Current.DisplayAlert("Not Found", $"Nothing matched 「{keyword}」. Here is a random pick instead!", "OK");
        }
    }

    private (bool isValid, string message) ValidateCraving()
    {
        if (string.IsNullOrWhiteSpace(CravingText))
            return (false, "Please enter a food name to search for!");

        if (CravingText.Trim().Length < 2)
            return (false, "Name is too short! Please enter at least 2 characters.");

        return (true, "");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
