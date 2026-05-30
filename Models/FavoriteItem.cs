using FoodPicker.Models;

namespace FoodPicker.Models;

/// 
public class FavoriteItem
{
    public FoodItem Food { get; set; }
    public string Note { get; set; } = "";
    public DateTime SavedAt { get; set; } = DateTime.Now;
}
