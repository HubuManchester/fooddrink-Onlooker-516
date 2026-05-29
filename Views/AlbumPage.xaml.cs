using FoodPicker.Helpers;
using FoodPicker.ViewModels;

namespace FoodPicker.Views;

public partial class AlbumPage : ContentPage
{
    private readonly AlbumViewModel _viewModel;

    public AlbumPage()
    {
        InitializeComponent();
        _viewModel = new AlbumViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        NavigationHelper.EnableSwipe(this);
    }
}
