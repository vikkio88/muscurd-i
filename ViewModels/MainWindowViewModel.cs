using ReactiveUI;
using System.Reactive;
using Muscurdi.Services;

namespace Muscurdi.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Muscurd-i";
    private string _name = string.Empty;
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }
    private string _password = string.Empty;
    public string Password { get => _password; set => this.RaiseAndSetIfChanged(ref _password, value); }

    private string? _passChar = "*";
    public string? PassChar { get => _passChar; set => this.RaiseAndSetIfChanged(ref _passChar, value); }

    private string _showPassText = "Show";
    public string ShowPassText { get => _showPassText; set => this.RaiseAndSetIfChanged(ref _showPassText, value); }

    private int _count = 0;
    public int Count { get => _count; set => this.RaiseAndSetIfChanged(ref _count, value); }

    public ReactiveCommand<Unit, Unit> CopyToClipboard { get; set; }
    public ReactiveCommand<Unit, Unit> Add { get; set; }
    public ReactiveCommand<Unit, Unit> ShowPass { get; set; }

    public MainWindowViewModel()
    {
        Count = Db.Instance.Passwords.Count();
        CopyToClipboard = ReactiveCommand.Create(() => { Avalonia.Application.Current?.Clipboard?.SetTextAsync("Yoyoy"); });
        Add = ReactiveCommand.Create(() =>
        {
            Db.Instance.AddPassword(new() { Name = Name, Password = Password });
            Count = Db.Instance.Passwords.Count();
            Name = string.Empty;
            Password = string.Empty;
        });
        ShowPass = ReactiveCommand.Create(() =>
        {
            PassChar = PassChar == "*" ? null : "*";
            ShowPassText = PassChar == null ? "Hide" : "Show";
        });
    }
}
