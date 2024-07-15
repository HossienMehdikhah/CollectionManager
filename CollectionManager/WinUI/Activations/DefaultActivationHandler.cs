using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Pages;
using Microsoft.UI.Xaml;
namespace CollectionManager.WinUI.Activations;

public class DefaultActivationHandler(INavigationService _navigationService) 
    : ActivationHandler<LaunchActivatedEventArgs>
{
    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(typeof(FeedPage).FullName!, args.Arguments);
        await Task.CompletedTask;
    }
}
