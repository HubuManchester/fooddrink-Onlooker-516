using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FoodPicker.ViewModels;

public class NearFood
{
    public string Name { get; set; } = "";
    public string Emoji { get; set; } = "";
    public string Distance { get; set; } = "";
    public string Category { get; set; } = "";
    public string Rating { get; set; } = "";       // e.g. "4.5"
    public string Price { get; set; } = "";         // e.g. "¥¥"
    public string Description { get; set; } = "";
}

public class NearbyViewModel : INotifyPropertyChanged
{
    private string _locationText = "正在获取位置...";
    private string _statusEmoji = "📍";
    private bool _isLoading;

    public NearbyViewModel()
    {
        NearbyFoods = new ObservableCollection<NearFood>();
        RefreshCommand = new Command(async () =>
        {
            try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
            catch { /* 模拟器可能不支持，忽略 */ }
            await LoadLocationAsync();
        });
    }

    public string LocationText
    {
        get => _locationText;
        set { _locationText = value; OnPropertyChanged(); }
    }

    public string StatusEmoji
    {
        get => _statusEmoji;
        set { _statusEmoji = value; OnPropertyChanged(); }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public ObservableCollection<NearFood> NearbyFoods { get; }
    public ICommand RefreshCommand { get; }

    public async Task LoadLocationAsync()
    {
        IsLoading = true;
        StatusEmoji = "⏳";

        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status != PermissionStatus.Granted)
            {
                LocationText = "定位权限未开启，显示默认推荐";
                StatusEmoji = "⚠️";
                GenerateNearbyFoods();
            }
            else
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location == null)
                {
                    LocationText = "📍  探索周边美食";
                    StatusEmoji = "✅";
                    GenerateNearbyFoods();
                }
                else
                {
                    string addressText;
                    try
                    {
                        var placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);
                        var p = placemarks?.FirstOrDefault();
                        if (p != null && !string.IsNullOrEmpty(p.Locality))
                        {
                            addressText = $"{p.Locality}{(string.IsNullOrEmpty(p.SubLocality) ? "" : " " + p.SubLocality)}";
                        }
                        else
                        {
                            addressText = $"{location.Latitude:F2}°, {location.Longitude:F2}°";
                        }
                    }
                    catch
                    {
                        addressText = $"{location.Latitude:F2}°, {location.Longitude:F2}°";
                    }

                    LocationText = $"📍  {addressText}";
                    StatusEmoji = "✅";
                    GenerateNearbyFoods();
                }
            }
        }
        catch (FeatureNotSupportedException)
        {
            LocationText = "此设备不支持定位，显示默认推荐";
            StatusEmoji = "⚠️";
            GenerateNearbyFoods();
        }
        catch (PermissionException)
        {
            LocationText = "定位权限被拒绝，显示默认推荐";
            StatusEmoji = "⚠️";
            GenerateNearbyFoods();
        }
        catch (Exception)
        {
            LocationText = "📍  探索周边美食";
            StatusEmoji = "✅";
            GenerateNearbyFoods();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void GenerateNearbyFoods()
    {
        NearbyFoods.Clear();

        var foods = new List<NearFood>
        {
            new()
            {
                Name = "蜀大侠火锅",
                Emoji = "🍲",
                Distance = "约 200m",
                Category = "川渝美食",
                Rating = "4.8",
                Price = "¥¥¥",
                Description = "正宗重庆牛油火锅，毛肚新鲜脆嫩，店内装修古风十足"
            },
            new()
            {
                Name = "马记拉面",
                Emoji = "🍝",
                Distance = "约 350m",
                Category = "西北风味",
                Rating = "4.6",
                Price = "¥",
                Description = "手工拉制，汤清味浓，牛肉软烂，一碗管饱"
            },
            new()
            {
                Name = "螺阿妹螺蛳粉",
                Emoji = "🍜",
                Distance = "约 500m",
                Category = "广西美食",
                Rating = "4.5",
                Price = "¥",
                Description = "酸笋正宗够味，粉条Q弹，免费加粉吃到撑"
            },
            new()
            {
                Name = "港记茶餐厅",
                Emoji = "🍖",
                Distance = "约 650m",
                Category = "粤式美食",
                Rating = "4.7",
                Price = "¥¥",
                Description = "招牌叉烧饭和丝袜奶茶，一秒穿越到香港街头"
            },
            new()
            {
                Name = "老王煎饼",
                Emoji = "🫓",
                Distance = "约 800m",
                Category = "北方小吃",
                Rating = "4.4",
                Price = "¥",
                Description = "天津老师傅手艺，薄脆自己炸，酱料秘制配方"
            },
        };

        foreach (var f in foods)
            NearbyFoods.Add(f);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
