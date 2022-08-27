using ReactiveUI;
using System.Reactive;
using Muscurdi.Services;
using Muscurdi.Models;
using Avalonia.Threading;

namespace Muscurdi.ViewModels;
public class MainWindowViewModel : ReactiveObject, IScreen
{
    public RoutingState Router { get; } = new RoutingState();
    public ReactiveCommand<Unit, Unit> Login { get; }

    public string _password = string.Empty;
    public string Password { get => _password; set => this.RaiseAndSetIfChanged(ref _password, value); }
    public string? _error = null;
    public string? Error { get => _error; set => this.RaiseAndSetIfChanged(ref _error, value); }

    public MainWindowViewModel()
    {
        if (!Db.IsDbInitialised())
        {

            // @TODO: in case this happens need to showcase a password generator
            var mp = Libs.MasterPasswordHelper.Generate();
            System.Console.WriteLine(mp.ToMemorable());
            S.Instance.GetDb(mp);
            S.Instance.CloseDb();
        }

        Login = ReactiveCommand.Create(() =>
        {
            try
            {
                S.Instance.GetDb(MasterPassword.Make(Password));
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
