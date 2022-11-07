namespace Muscurdi.ViewModels;

using ReactiveUI;
using Avalonia.Threading;
using System.Reactive;
using Muscurdi.Services;
using Muscurdi.Models;

public class AddViewModel : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string UrlPathSegment { get; } = "Add";
    public ReactiveCommand<Unit, IRoutableViewModel> Back { get; }
    private PasswordEntry? _existingRecord = null;
    private string _name = string.Empty;

    public string PageTitle { get; } = "Add New Password";
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }
    private string _username = string.Empty;
    public string Username { get => _username; set => this.RaiseAndSetIfChanged(ref _username, value); }
    private string _password = string.Empty;
    public string Password { get => _password; set => this.RaiseAndSetIfChanged(ref _password, value); }

    private string? _passChar = "*";
    public string? PassChar { get => _passChar; set => this.RaiseAndSetIfChanged(ref _passChar, value); }

    private string _showPassIcon = "fa-eye";
    public string ShowPassIcon { get => _showPassIcon; set => this.RaiseAndSetIfChanged(ref _showPassIcon, value); }


    public ReactiveCommand<Unit, Unit> Add { get; set; }
    public ReactiveCommand<Unit, Unit> ShowPass { get; set; }

    private string? _error = null;
    public string? Error { get => _error; set => this.RaiseAndSetIfChanged(ref _error, value); }

    public AddViewModel(IScreen screen, LiteDB.ObjectId? id = null)
    {
        HostScreen = screen;
        Back = HostScreen.Router.NavigateBack;
        if (id != null)
        {
            _existingRecord = S.Instance.Db.GetById(id);
            Name = _existingRecord.Name;
            Username = _existingRecord.Username;
            Password = _existingRecord.Password;
            PageTitle = "Update Password";
        }


        Add = ReactiveCommand.Create(() =>
        {
            if (_existingRecord != null)
            {
                updatePassword();
                HostScreen.Router.NavigateAndReset.Execute(new ListViewModel(this.HostScreen));
                return;
            }


            if (!addNewPassword())
            {
                Error = $"Can't add {Name}, probably exists already";
                DispatcherTimer.RunOnce(
                    () => Error = null,
                    System.TimeSpan.FromSeconds(3)
                );
            }

            Name = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            HostScreen.Router.NavigateAndReset.Execute(new ListViewModel(this.HostScreen));
        });
        ShowPass = ReactiveCommand.Create(() =>
        {
            PassChar = PassChar == "*" ? null : "*";
            ShowPassIcon = PassChar == null ? "fa-eye-slash" : "fa-eye";
        });
    }

    private void updatePassword()
    {
        _existingRecord.Name = Name;
        _existingRecord.Username = Username;
        _existingRecord.Password = Password;
        S.Instance.Db.UpdatePassword(_existingRecord);
    }

    private bool addNewPassword() => S.Instance.Db.AddPassword(new() { Name = Name, Username = Username, Password = Password });

}