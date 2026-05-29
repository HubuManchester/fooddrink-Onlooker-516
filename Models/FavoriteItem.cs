using FoodPicker.Models;

namespace FoodPicker.Models;

/// 包含用户个人笔记的收藏项
public class FavoriteItem
{
    public FoodItem Food { get; set; }
    public string Note { get; set; } = "";
    public DateTime SavedAt { get; set; } = DateTime.Now;
}
