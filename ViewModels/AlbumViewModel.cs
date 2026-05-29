using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FoodPicker.Models;

namespace FoodPicker.ViewModels;

public class AlbumViewModel : INotifyPropertyChanged
{
    // 照片分类
    public static readonly string[] Categories =
        { "全部", "🍻 朋友聚会", "💰 好吃不贵", "☕ 独自享受", "🌙 深夜放毒", "🎂 特别日子" };

    private readonly List<PhotoItem> _allPhotos = new();
    private string _selectedCategory = "全部";

    public AlbumViewModel()
    {
        FilteredPhotos = new ObservableCollection<PhotoItem>();
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

    private string _selectedFilter = "全部";
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
                    await Shell.Current.DisplayAlert("提示", "需要相机权限才能拍照", "知道了");
                    return;
                }
            }

            // 拍照或选图
            FileResult? file = useCamera
                ? await MediaPicker.Default.CapturePhotoAsync()
                : await MediaPicker.Default.PickPhotoAsync();

            if (file == null) return; // 用户取消

            // 让用户选分类
            var category = await Shell.Current.DisplayActionSheet(
                "选择照片分类", "取消", null, Categories[1..]); // 跳过"全部"
            if (category == null || category == "取消") category = "💰 好吃不贵";

            // 保存文件
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
            await Shell.Current.DisplayAlert("提示", "此设备不支持该功能", "知道了");
        }
        catch (PermissionException)
        {
            await Shell.Current.DisplayAlert("提示", "权限被拒绝，请在设置中开启", "知道了");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("出错了", $"操作失败：{ex.Message}", "好的");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void OnFilter(string category)
    {
        _selectedCategory = category;
        SelectedFilter = category;
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        FilteredPhotos.Clear();
        var items = _selectedCategory == "全部"
            ? _allPhotos
            : _allPhotos.Where(p => p.Category == _selectedCategory);

        foreach (var p in items)
            FilteredPhotos.Add(p);

        IsEmpty = FilteredPhotos.Count == 0;
    }

    private async void OnDeletePhoto(PhotoItem photo)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "删除照片", $"确定要删除这张「{photo.Category}」的照片吗？", "删除", "取消");
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
            await Shell.Current.DisplayAlert("出错了", $"删除失败：{ex.Message}", "好的");
        }
    }

    private async Task ViewPhotoAsync(PhotoItem photo)
    {
        if (!File.Exists(photo.FilePath))
        {
            await Shell.Current.DisplayAlert("提示", "照片文件不存在", "知道了");
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
            await Shell.Current.DisplayAlert("提示", "无法打开照片", "知道了");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
