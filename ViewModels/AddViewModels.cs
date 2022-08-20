namespace Muscurdi.ViewModels;

using ReactiveUI;
using Avalonia.Threading;
using System.Reactive;
using Muscurdi.Services;


public class AddViewModel : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string UrlPathSegment { get; } = "Add";
    public ReactiveCommand<Unit, Unit> Back { get; }

    private string _name = string.Empty;
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }
    private string _password = string.Empty;
    public string Password { get => _password; set => this.RaiseAndSetIfChanged(ref _password, value); }

    private string? _passChar = "*";
    public string? PassChar { get => _passChar; set => this.RaiseAndSetIfChanged(ref _passChar, value); }

    private string _showPassText = "Show";
    public string ShowPassText { get => _showPassText; set => this.RaiseAndSetIfChanged(ref _showPassText, value); }


    public ReactiveCommand<Unit, Unit> Add { get; set; }
    public ReactiveCommand<Unit, Unit> ShowPass { get; set; }

    private string? _error = null;
    public string? Error { get => _error; set => this.RaiseAndSetIfChanged(ref _error, value); }

    public AddViewModel(IScreen screen)
    {
        HostScreen = screen;
        Back = HostScreen.Router.NavigateBack;

        Add = ReactiveCommand.Create(() =>
        {
            var result = Db.Instance.AddPassword(new() { Name = Name, Password = Password });
            if (!result)
            {
                Error = $"Can't add {Name}, probably exists already";
                DispatcherTimer.RunOnce(
                    () => Error = null,
                    System.TimeSpan.FromSeconds(3)
                );
            }
            Name = string.Empty;
            Password = string.Empty;
            HostScreen.Router.NavigateAndReset.Execute(new ListViewModel(this.HostScreen));
        });
        ShowPass = ReactiveCommand.Create(() =>
        {
            PassChar = PassChar == "*" ? null : "*";
            ShowPassText = PassChar == null ? "Hide" : "Show";
        });
    }
}