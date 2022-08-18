namespace Muscurdi.Views;
using Muscurdi.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

public partial class AddView : ReactiveUserControl<AddViewModel>
{
    public AddView()
    {
        this.WhenActivated(disposables => {});
        AvaloniaXamlLoader.Load(this);
    }
}
