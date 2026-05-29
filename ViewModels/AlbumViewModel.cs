using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FoodPicker.ViewModels;

public class AlbumViewModel : INotifyPropertyChanged
{
    public AlbumViewModel()
    {
        Photos = new ObservableCollection<string>();
        TakePhotoCommand = new Command(async () => await TakePhotoAsync());
        DeletePhotoCommand = new Command<string>(OnDeletePhoto);
        ViewPhotoCommand = new Command<string>(async (path) => await ViewPhotoAsync(path));
    }

    public ObservableCollection<string> Photos { get; }

    private bool _isEmpty = true;
    public bool IsEmpty
    {
        get => _isEmpty;
        set { _isEmpty = value; OnPropertyChanged(); }
    }

    private bool _isTakingPhoto;
    public bool IsTakingPhoto
    {
        get => _isTakingPhoto;
        set { _isTakingPhoto = value; OnPropertyChanged(); }
    }

    public ICommand TakePhotoCommand { get; }
    public ICommand DeletePhotoCommand { get; }
    public ICommand ViewPhotoCommand { get; }

    private async Task TakePhotoAsync()
    {
        if (IsTakingPhoto) return;

        try
        {
            IsTakingPhoto = true;

            // 检查相机权限
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (status != PermissionStatus.Granted)
            {
                await Shell.Current.DisplayAlert("提示", "需要相机权限才能拍照", "知道了");
                return;
            }

            // 拍照
            var photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo == null) return;  // 用户取消

            // 保存到 app 的本地目录
            var appDir = FileSystem.AppDataDirectory;
            var fileName = $"food_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
            var filePath = Path.Combine(appDir, fileName);

            using (var sourceStream = await photo.OpenReadAsync())
            using (var destStream = File.OpenWrite(filePath))
            {
                await sourceStream.CopyToAsync(destStream);
            }

            Photos.Add(filePath);
            IsEmpty = Photos.Count == 0;
        }
        catch (FeatureNotSupportedException)
        {
            await Shell.Current.DisplayAlert("提示", "此设备不支持拍照功能", "知道了");
        }
        catch (PermissionException)
        {
            await Shell.Current.DisplayAlert("提示", "相机权限被拒绝，请在设置中开启", "知道了");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("出错了", $"拍照失败：{ex.Message}", "好的");
        }
        finally
        {
            IsTakingPhoto = false;
        }
    }

    private async void OnDeletePhoto(string filePath)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "删除照片", "确定要删除这张美食照片吗？", "删除", "取消");

        if (!confirm) return;

        try
        {
            // 删除文件
            if (File.Exists(filePath))
                File.Delete(filePath);

            Photos.Remove(filePath);
            IsEmpty = Photos.Count == 0;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("出错了", $"删除失败：{ex.Message}", "好的");
        }
    }

    private async Task ViewPhotoAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            await Shell.Current.DisplayAlert("提示", "照片文件不存在", "知道了");
            return;
        }

        try
        {
            // 使用系统默认图片查看器打开照片
            await Launcher.Default.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("提示", "无法打开照片", "知道了");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
