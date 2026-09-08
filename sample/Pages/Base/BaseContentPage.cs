using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace HackerNews;

abstract partial class BaseContentPage<TViewModel> : ContentPage where TViewModel : BaseViewModel
{
	protected BaseContentPage(TViewModel viewModel, string pageTitle)
	{
		Title = pageTitle;
		base.BindingContext = viewModel;

		On<iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Always);
	}

	protected new TViewModel BindingContext => (TViewModel)base.BindingContext;
}