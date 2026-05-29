using FoodPicker.ViewModels;

namespace FoodPicker.Views;

public partial class NearbyPage : ContentPage
{
    private readonly NearbyViewModel _viewModel;

    public NearbyPage()
    {
        InitializeComponent();
        _viewModel = new NearbyViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadLocationAsync();
    }
}
