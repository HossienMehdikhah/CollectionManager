using CollectionManager.WinUI.Activations;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI;

namespace CollectionManager.WinUI.Services;

public class ActivationService(ActivationHandler<LaunchActivatedEventArgs> _defaultHandler,
    IEnumerable<IActivationHandler> _activationHandlers) : IActivationService
{
    private UIElement? _shell = null;
    public async Task ActivateAsync(object activationArgs)
    {
        if (App.MainWindow.Content == null)
        {
            _shell = App.GetService<ShellPage>();
            App.MainWindow.Content = _shell ?? new Frame();
        }
        await HandleActivationAsync(activationArgs);
        App.MainWindow.Activate();
    }
    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }
}
