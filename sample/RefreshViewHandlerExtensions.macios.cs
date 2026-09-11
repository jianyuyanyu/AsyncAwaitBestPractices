using Microsoft.Maui.Handlers;
using UIKit;

namespace HackerNews;

// Workaround for https://github.com/dotnet/maui/issues/33169
// When the navigation bar displays Large Titles, .NET MAUI assigns the UIRefreshControl to UIScrollView.RefreshControl, which makes UIKit host it inside
// the navigation bar. As soon as the pull-to-refresh gesture settles, UIKit hides the hosted UIRefreshControl (while still reserving its space in the
// navigation bar), so the refresh indicator disappears even though the refresh is still executing.
// Inserting the UIRefreshControl into the UIScrollView instead, like .NET MAUI does when Large Titles are disabled, keeps the indicator visible and
// spinning until RefreshView.IsRefreshing is set to false
static class RefreshViewHandlerExtensions
{
	public static void KeepRefreshControlOutOfLargeTitleNavigationBar(this IPropertyMapper<IRefreshView, IRefreshViewHandler> mapper)
	{
		// .NET MAUI (re)attaches the UIRefreshControl to the UIScrollView in each of these mappings
		mapper.AppendToMapping(nameof(IRefreshView.Content), InsertRefreshControlIntoScrollView);
		mapper.AppendToMapping(nameof(IRefreshView.IsRefreshEnabled), InsertRefreshControlIntoScrollView);
		mapper.AppendToMapping(nameof(IView.IsEnabled), InsertRefreshControlIntoScrollView);
	}

	static void InsertRefreshControlIntoScrollView(IRefreshViewHandler handler, IRefreshView refreshView)
	{
		if (GetScrollView(handler.PlatformView) is not { RefreshControl: UIRefreshControl refreshControl } scrollView)
			return;

		scrollView.RefreshControl = null;
		scrollView.InsertSubview(refreshControl, 0);
	}

	static UIScrollView? GetScrollView(UIView view)
	{
		if (view is UIScrollView scrollView)
			return scrollView;

		foreach (var subview in view.Subviews)
		{
			if (GetScrollView(subview) is UIScrollView nestedScrollView)
				return nestedScrollView;
		}

		return null;
	}
}
