using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml;
namespace CollectionManager.WinUI.Activations;

public class DefaultActivationHandler(INavigationService _navigationService) : ActivationHandler<LaunchActivatedEventArgs>
{
    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(typeof(MainpageViewModel).FullName!, args.Arguments);
        await Task.CompletedTask;
    }
}
