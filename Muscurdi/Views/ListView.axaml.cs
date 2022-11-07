namespace Muscurdi.Views;
using Muscurdi.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

public partial class ListView : ReactiveUserControl<ListViewModel>
{
    public ListView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}
