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
        try
        {
            // 
            var target = _viewModel.CompassRotation;
            await CompassRose.RotateTo(target + (delta > 0 ? 3 : -3), 250, Easing.CubicOut);
            await CompassRose.RotateTo(target, 200, Easing.SpringOut);
        }
        catch
        {
            // 
            try
            {
                await CompassRose.RotateTo(_viewModel.CompassRotation, 350, Easing.CubicOut);
            }
            catch { }
        }
    }
}
