namespace FoodPicker.Helpers;

/// 为页面添加左右滑动切换 Tab 的手势
public static class NavigationHelper
{
    public static void EnableTabSwipe(ContentPage page)
    {
        var swipeLeft = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
        swipeLeft.Swiped += (s, e) => SwitchTab(page, +1);

        var swipeRight = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
        swipeRight.Swiped += (s, e) => SwitchTab(page, -1);

        page.Content?.GestureRecognizers.Add(swipeLeft);
        page.Content?.GestureRecognizers.Add(swipeRight);
    }

    private static void SwitchTab(Page page, int offset)
    {
        var shell = Shell.Current;
        if (shell?.Items.Count == 0) return;

        var tabBar = shell.Items[0];
        var pageType = page.GetType();

        for (int i = 0; i < tabBar.Items.Count; i++)
        {
            // TabBar 的每个 Item 是 ShellSection，里面包着 ShellContent
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
