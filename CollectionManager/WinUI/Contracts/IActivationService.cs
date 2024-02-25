namespace CollectionManager.WinUI.Contracts;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
