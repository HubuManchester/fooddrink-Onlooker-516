namespace FoodPicker.Models;

public class PhotoItem
{
    public string FilePath { get; set; } = "";
    public string Category { get; set; } = "";
    public DateTime TakenAt { get; set; } = DateTime.Now;
}
