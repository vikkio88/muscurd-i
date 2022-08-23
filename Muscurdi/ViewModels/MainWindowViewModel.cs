using ReactiveUI;
using System.Reactive;

namespace Muscurdi.ViewModels;
public class MainWindowViewModel : ReactiveObject, IScreen
{
    public RoutingState Router { get; } = new RoutingState();
    public ReactiveCommand<Unit, IRoutableViewModel> GoToList { get; }

    public string Password { get; set; } = string.Empty;

    public MainWindowViewModel()
    {
        GoToList = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new ListViewModel(this)));
    }
}
