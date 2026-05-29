using System.Collections.ObjectModel;
using FoodPicker.Models;

namespace FoodPicker.Services;

/// 收藏夹服务 —— 单例，所有页面共享同一份收藏数据
public class FavoritesService
{
    private static FavoritesService? _instance;
    public static FavoritesService Instance => _instance ??= new FavoritesService();

    private readonly List<FoodItem> _favorites = new();

    // 事件：收藏列表变化时通知订阅者（FavoritesViewModel 会监听）
    public event Action? FavoritesChanged;

    public bool IsFavorite(FoodItem food)
    {
        return _favorites.Any(f => f.Name == food.Name);
    }

    public bool Add(FoodItem food)
    {
        if (IsFavorite(food))
            return false;  // 已经收藏过了

        _favorites.Add(food);
        FavoritesChanged?.Invoke();
        return true;
    }

    public void Remove(FoodItem food)
    {
        var existing = _favorites.FirstOrDefault(f => f.Name == food.Name);
        if (existing != null)
        {
            _favorites.Remove(existing);
            FavoritesChanged?.Invoke();
        }
    }

    public List<FoodItem> GetAll()
    {
        // 返回副本，防止外部直接修改
        return new List<FoodItem>(_favorites);
    }

    public int Count => _favorites.Count;
}
