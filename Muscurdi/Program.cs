using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace Muscurdi;
class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
    {
        Splat.Locator.CurrentMutable.Register(() => new Views.AddView(), typeof(ReactiveUI.IViewFor<ViewModels.AddViewModel>));
        Splat.Locator.CurrentMutable.Register(() => new Views.ListView(), typeof(ReactiveUI.IViewFor<ViewModels.ListViewModel>));

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    }
}
