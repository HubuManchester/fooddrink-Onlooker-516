namespace FoodPicker.Helpers;

/// 左右滑动切换 Tab 的共用方法
public static class NavigationHelper
{
    public static void SwitchToTab(Page page, int offset)
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
