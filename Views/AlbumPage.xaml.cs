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
}
