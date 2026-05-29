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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // 每次切换到本页时刷新列表（处理Tab懒加载导致的首次不同步）
        _viewModel.LoadFromService();
    }
}
