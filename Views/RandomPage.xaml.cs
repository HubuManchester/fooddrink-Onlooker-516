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
        // 摇晃动画：卡片缩小 → 轻转 → 弹回
        try
        {
            // 阶段1：缩小+轻微旋转
            await Task.WhenAll(
                FoodCard.ScaleTo(0.85, 120, Easing.CubicIn),
                FoodCard.RotateTo(5, 120, Easing.CubicIn)
            );

            // 短暂停顿
            await Task.Delay(50);

            // 阶段2：弹回原位 + 弹性效果
            await Task.WhenAll(
                FoodCard.ScaleTo(1.05, 200, Easing.CubicOut),
                FoodCard.RotateTo(-2, 200, Easing.CubicOut)
            );

            // 阶段3：回弹到正常
            await Task.WhenAll(
                FoodCard.ScaleTo(1.0, 160, Easing.CubicOut),
                FoodCard.RotateTo(0, 160, Easing.CubicOut)
            );
        }
        catch
        {
            // 动画失败不影响功能
        }
    }
}
