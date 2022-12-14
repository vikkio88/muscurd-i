namespace Muscurdi.ViewModels;
using ReactiveUI;
using System.Reactive;
using System.Linq;
using System.Collections.ObjectModel;

using Muscurdi.Services;
using Muscurdi.Models;
using Avalonia.Threading;

public class ListViewModel : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen { get; }

    public string UrlPathSegment { get; } = "List";
    public ReactiveCommand<Unit, IRoutableViewModel> GoToAdd { get; }

    public ReactiveCommand<string, Unit> CopyToClipboard { get; set; }
    public ReactiveCommand<LiteDB.ObjectId, Unit> DeletePassword { get; set; }
    public ReactiveCommand<LiteDB.ObjectId, IRoutableViewModel> UpdateEntry { get; set; }
    private ObservableCollection<PasswordEntry> _passwords = new();
    public ObservableCollection<PasswordEntry> Passwords { get => _passwords; set => this.RaiseAndSetIfChanged(ref _passwords, value); }

    private string? _searchText = null;
    public string? SearchText { get => _searchText; set => this.RaiseAndSetIfChanged(ref _searchText, value); }

    private string? _searchStatus = null;
    public string? SearchStatus { get => _searchStatus; set => this.RaiseAndSetIfChanged(ref _searchStatus, value); }

    private string? _message = null;
    public string? Message { get => _message; set => this.RaiseAndSetIfChanged(ref _message, value); }
    public ReactiveCommand<Unit, Unit> Search { get; set; }
    public ReactiveCommand<Unit, Unit> ShowAll { get; set; }

    public ListViewModel(IScreen screen)
    {
        HostScreen = screen;
        GoToAdd = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new AddViewModel(this.HostScreen)));
        SearchStatus = $"{S.Instance.Db.Count()} Passwords Saved.";

        CopyToClipboard = ReactiveCommand.Create((string text) =>
        {
            Avalonia.Application.Current?.Clipboard?.SetTextAsync(text);
            Message = "Copied to Clipboard!";
            DispatcherTimer.RunOnce(
                () => Message = null,
                System.TimeSpan.FromSeconds(3)
            );
        });

        UpdateEntry = ReactiveCommand.CreateFromObservable((LiteDB.ObjectId id) => HostScreen.Router.Navigate.Execute(new AddViewModel(this.HostScreen, id)));
        DeletePassword = ReactiveCommand.Create((LiteDB.ObjectId id) =>
        {
            if (S.Instance.Db.Passwords.Delete(id))
            {
                var item = Passwords.First(x => x.Id == new LiteDB.ObjectId(id));
                Passwords.Remove(item);
            }
        });

        var canSearch = this.WhenAnyValue(
            x => x.SearchText,
            x => !string.IsNullOrWhiteSpace(x)
        );
        Search = ReactiveCommand.Create(() =>
        {
            Passwords = new(S.Instance.Db.FindPasswordsByName(SearchText ?? string.Empty));
            if (Passwords.Count == 0)
            {
                SearchStatus = $"No Results for \"{SearchText}\"";
            }
            else
            {
                SearchStatus = null;
            }
        }, canSearch);

        ShowAll = ReactiveCommand.Create(() =>
        {
            Passwords = new(S.Instance.Db.GetAll());
            if (Passwords.Count == 0)
            {
                SearchStatus = "No Passwords Stored yet...";
            }
            else
            {
                SearchStatus = null;
            }
        });

    }
}