using FoodPicker.Models;

namespace FoodPicker.Models;

/// A saved food item with an optional personal note from the user
public class FavoriteItem
{
    public FoodItem Food { get; set; }
    public string Note { get; set; } = "";
    public DateTime SavedAt { get; set; } = DateTime.Now;
}
