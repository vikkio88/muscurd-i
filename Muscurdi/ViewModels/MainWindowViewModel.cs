using ReactiveUI;
using System.Reactive;
using Muscurdi.Services;
using Muscurdi.Models;
using Avalonia.Threading;

namespace Muscurdi.ViewModels;
public class MainWindowViewModel : ReactiveObject, IScreen
{
    public RoutingState Router { get; } = new RoutingState();
    public ReactiveCommand<Unit, Unit> GeneratePassword { get; }
    public ReactiveCommand<Unit, Unit> Login { get; }

    public bool _isDbInitialised = false;
    public bool IsDbInitialised { get => _isDbInitialised; set => this.RaiseAndSetIfChanged(ref _isDbInitialised, value); }

    public string _password = string.Empty;
    public string Password
    {
        get => _password; set
        {
            this.RaiseAndSetIfChanged(ref _password, value);
        }
    }
    public string? _error = null;
    public string? Error { get => _error; set => this.RaiseAndSetIfChanged(ref _error, value); }

    public MainWindowViewModel()
    {
        IsDbInitialised = Db.IsDbInitialised();
        if (!IsDbInitialised)
        {
            var mp = Libs.MasterPasswordHelper.Generate();
            Password = mp.ToMemorable();
        }

        GeneratePassword = ReactiveCommand.Create(() =>
        {
            var mp = Libs.MasterPasswordHelper.Generate();
            Password = mp.ToMemorable();
        });


        Login = ReactiveCommand.Create(() =>
        {
            try
            {
                S.Instance.Db?.Init(MasterPassword.Make(Password));
            }
            catch (System.Exception exc) when (exc is Exceptions.MasterPasswordException || exc is Exceptions.InvalidPasswordException)
            {
                reportLoginError(exc.Message);
                return;
            }

            Router.Navigate.Execute(new ListViewModel(this));
        });
    }

    private void reportLoginError(string message)
    {
        Error = message;
        Password = string.Empty;
        DispatcherTimer.RunOnce(
           () => Error = null,
           System.TimeSpan.FromSeconds(3)
       );
    }
}
