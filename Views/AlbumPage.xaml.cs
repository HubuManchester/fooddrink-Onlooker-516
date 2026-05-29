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

    private void OnSwipedLeft(object? sender, SwipedEventArgs e)
        => NavigationHelper.SwitchToTab(this, +1);
    private void OnSwipedRight(object? sender, SwipedEventArgs e)
        => NavigationHelper.SwitchToTab(this, -1);
}
