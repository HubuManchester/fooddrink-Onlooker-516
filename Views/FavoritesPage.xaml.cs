using FoodPicker.ViewModels;

namespace FoodPicker.Views;

public partial class FavoritesPage : ContentPage
{
    private readonly FavoritesViewModel _viewModel;

    public FavoritesPage()
    {
        InitializeComponent();
        _viewModel = new FavoritesViewModel();
        BindingContext = _viewModel;
    }
}
