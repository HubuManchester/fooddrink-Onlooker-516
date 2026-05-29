using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FoodPicker.Models;

namespace FoodPicker.ViewModels;

public class AlbumViewModel : INotifyPropertyChanged
{
    public class CategoryItem : INotifyPropertyChanged
    {
        public string Name { get; set; } = "";
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected))); }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    private static readonly string[] CategoryNames =
        { "All", "🍻 Friends & Fun", "💰 Tasty & Cheap", "☕ Solo Treat", "🌙 Late Night Cravings", "🎂 Special Days" };

    public ObservableCollection<CategoryItem> FilterCategories { get; } = new();

    private readonly List<PhotoItem> _allPhotos = new();

    public AlbumViewModel()
    {
        FilteredPhotos = new ObservableCollection<PhotoItem>();
        foreach (var name in CategoryNames)
            FilterCategories.Add(new CategoryItem { Name = name, IsSelected = name == "All" });
        TakePhotoCommand = new Command(async () => await CaptureAsync(true));
        PickPhotoCommand = new Command(async () => await CaptureAsync(false));
        DeletePhotoCommand = new Command<PhotoItem>(OnDeletePhoto);
        ViewPhotoCommand = new Command<PhotoItem>(async (p) => await ViewPhotoAsync(p));
        FilterCommand = new Command<string>(OnFilter);
    }

    public ObservableCollection<PhotoItem> FilteredPhotos { get; }

    private bool _isEmpty = true;
    public bool IsEmpty
    {
        get => _isEmpty;
        set { _isEmpty = value; OnPropertyChanged(); }
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    private string _selectedFilter = "All";
    public string SelectedFilter
    {
        get => _selectedFilter;
        set { _selectedFilter = value; OnPropertyChanged(); }
    }

    public ICommand TakePhotoCommand { get; }
    public ICommand PickPhotoCommand { get; }
    public ICommand DeletePhotoCommand { get; }
    public ICommand ViewPhotoCommand { get; }
    public ICommand FilterCommand { get; }

    private async Task CaptureAsync(bool useCamera)
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            // 权限检查
            if (useCamera)
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    await Shell.Current.DisplayAlert("Permission", "Camera permission is required to take photos.", "OK");
                    return;
                }
            }

            // 拍照或选图
            FileResult? file = useCamera
                ? await MediaPicker.Default.CapturePhotoAsync()
                : await MediaPicker.Default.PickPhotoAsync();

            if (file == null) return; // 用户Cancel

            // Let user pick a category
            var category = await Shell.Current.DisplayActionSheet(
                "Choose a category", "Cancel", null, CategoryNames[1..]);
            if (category == null || category == "Cancel") category = "💰 Tasty & Cheap";

            // Save文件
            var appDir = FileSystem.AppDataDirectory;
            var fileName = $"food_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
            var filePath = Path.Combine(appDir, fileName);

            using (var sourceStream = await file.OpenReadAsync())
            using (var destStream = File.OpenWrite(filePath))
            {
                await sourceStream.CopyToAsync(destStream);
            }

            var photo = new PhotoItem
            {
                FilePath = filePath,
                Category = category,
                TakenAt = DateTime.Now
            };

            _allPhotos.Add(photo);
            ApplyFilter();
        }
        catch (FeatureNotSupportedException)
        {
            await Shell.Current.DisplayAlert("Note", "This device does not support this feature", "OK");
        }
        catch (PermissionException)
        {
            await Shell.Current.DisplayAlert("Note", "Permission denied. Please enable it in Settings.", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Operation failed: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void OnFilter(string category)
    {
        SelectedFilter = category;

        // 更新高亮状态
        foreach (var c in FilterCategories)
            c.IsSelected = c.Name == category;

        ApplyFilter();
    }

    private void ApplyFilter()
    {
        FilteredPhotos.Clear();
        var items = SelectedFilter == "All"
            ? _allPhotos
            : _allPhotos.Where(p => p.Category == SelectedFilter);

        foreach (var p in items)
            FilteredPhotos.Add(p);

        IsEmpty = FilteredPhotos.Count == 0;
    }

    private async void OnDeletePhoto(PhotoItem photo)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Delete Photo", $"Delete this photo from 「{photo.Category}」?", "Delete", "Cancel");
        if (!confirm) return;

        try
        {
            if (File.Exists(photo.FilePath))
                File.Delete(photo.FilePath);

            _allPhotos.Remove(photo);
            ApplyFilter();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Delete failed: {ex.Message}", "OK");
        }
    }

    private async Task ViewPhotoAsync(PhotoItem photo)
    {
        if (!File.Exists(photo.FilePath))
        {
            await Shell.Current.DisplayAlert("Note", "Photo file not found", "OK");
            return;
        }

        try
        {
            await Launcher.Default.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(photo.FilePath)
            });
        }
        catch
        {
            await Shell.Current.DisplayAlert("Note", "Cannot open photo", "OK");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
