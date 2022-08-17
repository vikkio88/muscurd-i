using ReactiveUI;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reactive;
using Muscurdi.Services;
using Muscurdi.Models;

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

    private ObservableCollection<PasswordEntry> _passwords = new();
    public ObservableCollection<PasswordEntry> Passwords { get => _passwords; set => this.RaiseAndSetIfChanged(ref _passwords, value); }

    public ReactiveCommand<string, Unit> CopyToClipboard { get; set; }
    public ReactiveCommand<LiteDB.ObjectId, Unit> DeletePassword { get; set; }
    public ReactiveCommand<Unit, Unit> Add { get; set; }
    public ReactiveCommand<Unit, Unit> ShowPass { get; set; }

    private string? _searchText = null;
    public string? SearchText { get => _searchText; set => this.RaiseAndSetIfChanged(ref _searchText, value); }
    public ReactiveCommand<Unit, Unit> Search { get; set; }

    public MainWindowViewModel()
    {
        CopyToClipboard = ReactiveCommand.Create((string password) => { Avalonia.Application.Current?.Clipboard?.SetTextAsync(password); });
        DeletePassword = ReactiveCommand.Create((LiteDB.ObjectId id) =>
        {
            if (Db.Instance.Passwords.Delete(id))
            {
                var item = Passwords.First(x => x.Id == new LiteDB.ObjectId(id));
                Passwords.Remove(item);
            }
        });
        Add = ReactiveCommand.Create(() =>
        {
            Db.Instance.AddPassword(new() { Name = Name, Password = Password });
            Name = string.Empty;
            Password = string.Empty;
        });
        ShowPass = ReactiveCommand.Create(() =>
        {
            PassChar = PassChar == "*" ? null : "*";
            ShowPassText = PassChar == null ? "Hide" : "Show";
        });

        var canSearch = this.WhenAnyValue(
            x => x.SearchText,
            x => !string.IsNullOrWhiteSpace(x)
        );
        Search = ReactiveCommand.Create(() =>
        {
            Passwords = new(Db.Instance.Passwords.Find(x => x.Name.Contains(SearchText ?? string.Empty)).ToList());
            System.Console.WriteLine(Passwords.Count);
        }, canSearch);
    }
}
