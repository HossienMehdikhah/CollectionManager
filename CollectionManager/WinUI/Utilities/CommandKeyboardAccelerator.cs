using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;
using System.Windows.Input;
namespace CollectionManager.WinUI.Utilities;

public class CommandKeyboardAccelerator : KeyboardAccelerator
{
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command),
        typeof(ICommand),
        typeof(CommandKeyboardAccelerator),
        new PropertyMetadata(null));
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }


    public static readonly DependencyProperty CommandParameterProperty =
       DependencyProperty.Register(
           nameof(CommandParameter),
           typeof(object),
           typeof(CommandKeyboardAccelerator),
           new PropertyMetadata(null));
    public object CommandParameter
    {
        get { return GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }

   
   


    public CommandKeyboardAccelerator() => Invoked += OnAcceleratorInvoked;
    private void OnAcceleratorInvoked(KeyboardAccelerator sender,
        KeyboardAcceleratorInvokedEventArgs args)
    {
        Command.Execute(CommandParameter);
    }
}
