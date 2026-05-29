using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FoodPicker.Models;
using FoodPicker.Services;

namespace FoodPicker.ViewModels;

public class FavoritesViewModel : INotifyPropertyChanged
{
    private readonly FavoritesService _favService;

    public FavoritesViewModel()
    {
        _favService = FavoritesService.Instance;
        FavoriteFoods = new ObservableCollection<FoodItem>(_favService.GetAll());

        RemoveCommand = new Command<FoodItem>(OnRemove);
        ClearAllCommand = new Command(OnClearAll);

        // 监听收藏变化，自动刷新列表
        _favService.FavoritesChanged += OnFavoritesChanged;
    }

    public ObservableCollection<FoodItem> FavoriteFoods { get; }

    private bool _isEmpty = true;
    public bool IsEmpty
    {
        get => _isEmpty;
        set { _isEmpty = value; OnPropertyChanged(); }
    }

    public ICommand RemoveCommand { get; }
    public ICommand ClearAllCommand { get; }

    private void RefreshList()
    {
        FavoriteFoods.Clear();
        foreach (var food in _favService.GetAll())
            FavoriteFoods.Add(food);
        IsEmpty = FavoriteFoods.Count == 0;
    }

    private void OnFavoritesChanged()
    {
        RefreshList();
    }

    private async void OnRemove(FoodItem food)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "取消收藏",
            $"确定要把「{food.Name}」从收藏夹移除吗？",
            "移除", "取消");

        if (confirm)
        {
            _favService.Remove(food);
            // 列表通过 FavoritesChanged 事件自动刷新
        }
    }

    private async void OnClearAll()
    {
        if (FavoriteFoods.Count == 0) return;

        bool confirm = await Shell.Current.DisplayAlert(
            "清空收藏",
            "确定要清空所有收藏吗？这个操作不能撤销。",
            "清空", "取消");

        if (confirm)
        {
            // 拷贝一份再删，避免遍历时修改集合
            var all = FavoriteFoods.ToList();
            foreach (var f in all)
                _favService.Remove(f);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
