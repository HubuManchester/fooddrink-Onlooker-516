namespace FoodPicker.Helpers;

/// 左右滑动切换 Tab —— 使用 Pan 手势避免与 ScrollView 冲突
public static class NavigationHelper
{
    private static readonly Dictionary<Type, PanGestureRecognizer> _panCache = new();

    public static void EnableSwipe(ContentPage page)
    {
        if (page.Content is not View root) return;

        // 避免重复添加
        if (_panCache.ContainsKey(page.GetType())) return;

        var pan = new PanGestureRecognizer();
        pan.PanUpdated += (s, e) =>
        {
            if (e.StatusType == GestureStatus.Completed &&
                Math.Abs(e.TotalX) > 80 &&
                Math.Abs(e.TotalX) > Math.Abs(e.TotalY) * 1.5)  // 横向为主
            {
                SwitchToTab(page, e.TotalX < 0 ? +1 : -1);
            }
        };

        root.GestureRecognizers.Add(pan);
        _panCache[page.GetType()] = pan;
    }

    private static void SwitchToTab(ContentPage page, int offset)
    {
        var shell = Shell.Current;
        if (shell?.Items.Count == 0) return;

        var tabBar = shell.Items[0];
        var pageType = page.GetType();

        for (int i = 0; i < tabBar.Items.Count; i++)
        {
            if (tabBar.Items[i] is ShellSection section &&
                section.Items.Count > 0 &&
                section.Items[0].Content?.GetType() == pageType)
            {
                var newIdx = (i + offset + tabBar.Items.Count) % tabBar.Items.Count;
                tabBar.CurrentItem = tabBar.Items[newIdx];
                return;
            }
        }
    }
}
