using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Muscurdi.ViewModels;
using Muscurdi.Views;

namespace Muscurdi;
public partial class App : Application
{
    static FileStream? _lockFile;
    public override void Initialize()
    {
        var dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        Directory.CreateDirectory(dir);
        try
        {
            _lockFile = File.Open(Path.Combine(dir, ".lock"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            _lockFile.Lock(0, 0);
        }
        catch
        {
            Console.WriteLine("An Instance of Muscurd-I is already running.");
            Environment.Exit(0);
        }
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}