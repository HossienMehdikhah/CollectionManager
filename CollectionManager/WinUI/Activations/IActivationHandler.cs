namespace CollectionManager.WinUI.Activations;

public interface IActivationHandler
{
    bool CanHandle(object args);

    Task HandleAsync(object args);
}
