using FoodPicker.Helpers;
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

    private void OnSwipedLeft(object? sender, SwipedEventArgs e)
        => NavigationHelper.SwitchToTab(this, +1);
    private void OnSwipedRight(object? sender, SwipedEventArgs e)
        => NavigationHelper.SwitchToTab(this, -1);

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
            // emoji 先弹跳
            await EmojiLabel.ScaleTo(1.3, 100, Easing.CubicOut);
            await EmojiLabel.ScaleTo(0.9, 80, Easing.CubicIn);
            await EmojiLabel.ScaleTo(1.0, 100, Easing.CubicOut);

            // 卡片缩小+轻微旋转
            await Task.WhenAll(
                FoodCard.ScaleTo(0.88, 120, Easing.CubicIn),
                FoodCard.RotateTo(4, 120, Easing.CubicIn)
            );

            await Task.Delay(40);

            // 弹回原位
            await Task.WhenAll(
                FoodCard.ScaleTo(1.03, 180, Easing.CubicOut),
                FoodCard.RotateTo(-1.5, 180, Easing.CubicOut)
            );

            // 回弹到正常
            await Task.WhenAll(
                FoodCard.ScaleTo(1.0, 140, Easing.CubicOut),
                FoodCard.RotateTo(0, 140, Easing.CubicOut)
            );
        }
        catch
        {
            // 动画失败不影响功能
        }
    }
}
