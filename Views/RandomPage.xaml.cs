using FoodPicker.ViewModels;

namespace FoodPicker.Views;

public partial class RandomPage : ContentPage
{
    private readonly RandomViewModel _viewModel;

    public RandomPage()
    {
        InitializeComponent();
        _viewModel = new RandomViewModel();
        BindingContext = _viewModel;

        _viewModel.ShakeHappened += OnShakeHappened;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.StartShakeDetection();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.StopShakeDetection();
    }

    private async void OnShakeHappened()
    {
        try
        {
            // 
            await EmojiLabel.ScaleTo(1.3, 100, Easing.CubicOut);
            await EmojiLabel.ScaleTo(0.9, 80, Easing.CubicIn);
            await EmojiLabel.ScaleTo(1.0, 100, Easing.CubicOut);

            // 
            await Task.WhenAll(
                FoodCard.ScaleTo(0.88, 120, Easing.CubicIn),
                FoodCard.RotateTo(4, 120, Easing.CubicIn)
            );

            await Task.Delay(40);

            // 
            await Task.WhenAll(
                FoodCard.ScaleTo(1.03, 180, Easing.CubicOut),
                FoodCard.RotateTo(-1.5, 180, Easing.CubicOut)
            );

            // 
            await Task.WhenAll(
                FoodCard.ScaleTo(1.0, 140, Easing.CubicOut),
                FoodCard.RotateTo(0, 140, Easing.CubicOut)
            );
        }
        catch
        {
            // 
        }
    }
}
