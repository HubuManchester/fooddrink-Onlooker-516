using FoodPicker.Models;

namespace FoodPicker.Services;

/// 收藏夹服务 —— 单例，管理收藏和用户笔记
public class FavoritesService
{
    private static FavoritesService? _instance;
    public static FavoritesService Instance => _instance ??= new FavoritesService();

    private readonly List<FavoriteItem> _favorites = new();

    public event Action? FavoritesChanged;

    public bool IsFavorite(FoodItem food)
    {
        return _favorites.Any(f => f.Food.Name == food.Name);
    }

    public bool Add(FoodItem food)
    {
        if (IsFavorite(food))
            return false;

        _favorites.Add(new FavoriteItem { Food = food });
        FavoritesChanged?.Invoke();
        return true;
    }

    public void Remove(FavoriteItem item)
    {
        _favorites.Remove(item);
        FavoritesChanged?.Invoke();
    }

    public void RemoveByFoodName(string name)
    {
        var item = _favorites.FirstOrDefault(f => f.Food.Name == name);
        if (item != null)
        {
            _favorites.Remove(item);
            FavoritesChanged?.Invoke();
        }
    }

    public void UpdateNote(string foodName, string note)
    {
        var item = _favorites.FirstOrDefault(f => f.Food.Name == foodName);
        if (item != null)
        {
            item.Note = note;
            FavoritesChanged?.Invoke();
        }
    }

    public string GetNote(string foodName)
    {
        return _favorites.FirstOrDefault(f => f.Food.Name == foodName)?.Note ?? "";
    }

    public List<FavoriteItem> GetAll()
    {
        return new List<FavoriteItem>(_favorites);
    }

    public int Count => _favorites.Count;
}
