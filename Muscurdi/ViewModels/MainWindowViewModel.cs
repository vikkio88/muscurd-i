using ReactiveUI;
using System.Reactive;
using Muscurdi.Services;
using Muscurdi.Models;

namespace Muscurdi.ViewModels;
public class MainWindowViewModel : ReactiveObject, IScreen
{
    public RoutingState Router { get; } = new RoutingState();
    public ReactiveCommand<Unit, Unit> Login { get; }

    public string Password { get; set; } = string.Empty;

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
            catch (System.InvalidOperationException exc)
            {
                // Need to check what happens with wrong strings to create Master password
                System.Console.WriteLine("Invalid Password");
                return;
            }

            // this seems to work fine
            Router.Navigate.Execute(new ListViewModel(this));
        });
    }
}
