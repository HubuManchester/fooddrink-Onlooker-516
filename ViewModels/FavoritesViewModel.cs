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
    private readonly Random _rng = new();

    public FavoritesViewModel()
    {
        _favService = FavoritesService.Instance;
        FavoriteItems = new ObservableCollection<FavoriteItem>(_favService.GetAll());

        RemoveCommand = new Command<FavoriteItem>(OnRemove);
        ClearAllCommand = new Command(OnClearAll);
        EditNoteCommand = new Command<FavoriteItem>(async (item) => await OnEditNote(item));
        RandomPickCommand = new Command(OnRandomPick);

        _favService.FavoritesChanged += OnFavoritesChanged;
    }

    public ObservableCollection<FavoriteItem> FavoriteItems { get; }

    private bool _isEmpty = true;
    public bool IsEmpty
    {
        get => _isEmpty;
        set { _isEmpty = value; OnPropertyChanged(); }
    }

    private string _statsText = "";
    public string StatsText
    {
        get => _statsText;
        set { _statsText = value; OnPropertyChanged(); }
    }

    private bool _hasStats;
    public bool HasStats
    {
        get => _hasStats;
        set { _hasStats = value; OnPropertyChanged(); }
    }

    public ICommand RemoveCommand { get; }
    public ICommand ClearAllCommand { get; }
    public ICommand EditNoteCommand { get; }
    public ICommand RandomPickCommand { get; }

    private void RefreshList()
    {
        FavoriteItems.Clear();
        foreach (var item in _favService.GetAll())
            FavoriteItems.Add(item);
        IsEmpty = FavoriteItems.Count == 0;

        // 更新统计
        UpdateStats();
    }

    private void UpdateStats()
    {
        var count = FavoriteItems.Count;
        HasStats = count > 0;

        if (count == 0)
        {
            StatsText = "";
            return;
        }

        // 找收藏最多的分类
        var top = FavoriteItems
            .GroupBy(f => f.Food.Category)
            .OrderByDescending(g => g.Count())
            .First();

        StatsText = $"共收藏 {count} 道美食 · 最爱 {top.Key}";
        OnPropertyChanged(nameof(StatsText));
    }

    public void LoadFromService()
    {
        RefreshList();
    }

    private void OnFavoritesChanged()
    {
        RefreshList();
    }

    private async void OnRandomPick()
    {
        if (FavoriteItems.Count == 0) return;

        try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
        catch { }

        var pick = FavoriteItems[_rng.Next(FavoriteItems.Count)];
        await Shell.Current.DisplayAlert(
            $"🎲 今天吃「{pick.Food.Name}」吧！",
            $"{pick.Food.Emoji}  {pick.Food.Category}\n\n{pick.Food.Description}",
            "好的");
    }

    private async void OnRemove(FavoriteItem item)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "取消收藏",
            $"确定要把「{item.Food.Name}」从收藏夹移除吗？",
            "移除", "取消");

        if (confirm) _favService.Remove(item);
    }

    private async void OnClearAll()
    {
        if (FavoriteItems.Count == 0) return;

        bool confirm = await Shell.Current.DisplayAlert(
            "清空收藏",
            "确定要清空所有收藏吗？这个操作不能撤销。",
            "清空", "取消");

        if (confirm)
        {
            var all = FavoriteItems.ToList();
            foreach (var f in all) _favService.Remove(f);
        }
    }

    private async Task OnEditNote(FavoriteItem item)
    {
        var currentNote = _favService.GetNote(item.Food.Name);

        var newNote = await Shell.Current.DisplayPromptAsync(
            $"📝 {item.Food.Emoji} {item.Food.Name}",
            "记录你的美食体验：",
            "保存", "取消",
            placeholder: "比如：上次和闺蜜一起吃的，超满足！",
            initialValue: currentNote,
            maxLength: 200);

        if (newNote == null) return;

        _favService.UpdateNote(item.Food.Name, newNote);
        try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
        catch { }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
