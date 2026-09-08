using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using UIKit;

namespace HackerNews;

// Shell's default ShellNavBarAppearanceTracker applies Shell.TitleColor to the standard Title, but not to the Large Title
sealed class LargeTitleShellRenderer : ShellRenderer
{
	protected override IShellNavBarAppearanceTracker CreateNavBarAppearanceTracker() => new LargeTitleNavBarAppearanceTracker(base.CreateNavBarAppearanceTracker());

	sealed class LargeTitleNavBarAppearanceTracker(IShellNavBarAppearanceTracker defaultTracker) : IShellNavBarAppearanceTracker
	{
		public void SetAppearance(UINavigationController controller, ShellAppearance appearance)
		{
			defaultTracker.SetAppearance(controller, appearance);

			if (appearance.TitleColor is not Color titleColor)
				return;

			var navigationBar = controller.NavigationBar;
			var navigationBarAppearance = navigationBar.StandardAppearance;
			navigationBarAppearance.LargeTitleTextAttributes = new UIStringAttributes { ForegroundColor = titleColor.ToPlatform() };

			navigationBar.StandardAppearance = navigationBarAppearance;
			navigationBar.ScrollEdgeAppearance = navigationBarAppearance;
		}

		public void ResetAppearance(UINavigationController controller) => defaultTracker.ResetAppearance(controller);

		public void UpdateLayout(UINavigationController controller) => defaultTracker.UpdateLayout(controller);

		public void SetHasShadow(UINavigationController controller, bool hasShadow) => defaultTracker.SetHasShadow(controller, hasShadow);

		public void Dispose() => defaultTracker.Dispose();
	}
}