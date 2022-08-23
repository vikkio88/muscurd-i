namespace Muscurdi.Views;


using Avalonia.Markup.Xaml;

using Avalonia.ReactiveUI;
using ReactiveUI;

public partial class MainWindow : ReactiveWindow<ViewModels.MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(disposables => {});
        AvaloniaXamlLoader.Load(this);
    }
}