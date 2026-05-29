using FoodPicker.Helpers;
using FoodPicker.ViewModels;

namespace FoodPicker.Views;

public partial class CompassPage : ContentPage
{
    private readonly CompassViewModel _viewModel;

    public CompassPage()
    {
        InitializeComponent();
        _viewModel = new CompassViewModel();
        BindingContext = _viewModel;
        _viewModel.HeadingChanged += OnHeadingChanged;
    }

    private void OnSwipedLeft(object? sender, SwipedEventArgs e)
        => NavigationHelper.SwitchToTab(this, +1);
    private void OnSwipedRight(object? sender, SwipedEventArgs e)
        => NavigationHelper.SwitchToTab(this, -1);

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Start();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.Stop();
    }

    private async void OnHeadingChanged(double delta)
    {
        // 平滑旋转动画：罗盘跟着手机转
        try
        {
            await CompassRose.RotateTo(
                _viewModel.CompassRotation,
                400,
                Easing.CubicOut);
        }
        catch
        {
            // 动画失败不影响功能
        }
    }
}
