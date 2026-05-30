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
    public string Price { get; set; } = "";         // e.g. "$$"
    public string Description { get; set; } = "";
}

public class NearbyViewModel : INotifyPropertyChanged
{
    private string _locationText = "Detecting location...";
    private string _statusEmoji = "📍";
    private bool _isLoading;

    public NearbyViewModel()
    {
        NearbyFoods = new ObservableCollection<NearFood>();
        RefreshCommand = new Command(async () =>
        {
            try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
            catch { /* Silently ignore if not supported */ }
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
                LocationText = "Location permission not granted. Showing default results.";
                StatusEmoji = "⚠️";
                GenerateNearbyFoods();
            }
            else
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location == null)
                {
                    LocationText = "📍  ";
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
            LocationText = "Device does not support location. Showing defaults.";
            StatusEmoji = "⚠️";
            GenerateNearbyFoods();
        }
        catch (PermissionException)
        {
            LocationText = "Location permission denied. Showing defaults.";
            StatusEmoji = "⚠️";
            GenerateNearbyFoods();
        }
        catch (Exception)
        {
            LocationText = "📍  ";
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
                Name = "Shu Daxia Hotpot",
                Emoji = "🍲",
                Distance = "~200m",
                Category = "Sichuan Cuisine",
                Rating = "4.8",
                Price = "$$$",
                Description = "Authentic Chongqing hotpot with fresh tripe, traditional decor."
            },
            new()
            {
                Name = "Ma Ji Noodles",
                Emoji = "🍝",
                Distance = "~350m",
                Category = "Northwest Flavors",
                Rating = "4.6",
                Price = "$",
                Description = "Hand-pulled noodles in rich clear broth with tender beef."
            },
            new()
            {
                Name = "Luo Amei Luosifen",
                Emoji = "🍜",
                Distance = "~500m",
                Category = "Guangxi Cuisine",
                Rating = "4.5",
                Price = "$",
                Description = "Authentic sour bamboo shoots with chewy rice noodles."
            },
            new()
            {
                Name = "Gang Ji Cafe",
                Emoji = "🍖",
                Distance = "~650m",
                Category = "Cantonese Cuisine",
                Rating = "4.7",
                Price = "$$",
                Description = "Signature char siu rice and milk tea, a taste of Hong Kong."
            },
            new()
            {
                Name = "Lao Wang Jianbing",
                Emoji = "🫓",
                Distance = "~800m",
                Category = "Northern Snacks",
                Rating = "4.4",
                Price = "$",
                Description = "Tianjin-style crepe with crispy cracker and secret sauce."
            },
        };

        foreach (var f in foods)
            NearbyFoods.Add(f);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
