using Avalonia;
using Avalonia.ReactiveUI;
using System;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;

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
            .WithIcons(container => container.Register<FontAwesomeIconProvider>())
            .LogToTrace()
            .UseReactiveUI();
    }
}
