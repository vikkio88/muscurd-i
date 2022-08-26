using Muscurdi.Libs;
using Muscurdi.Models;

namespace Muscurdi.Services;

// I hate this shit, will need to do DependencyInjection properly
public class S : Singleton<S>
{
    private Db? _db;

    public Db Db { get => _db; }
    public Db GetDb(MasterPassword masterPassword)
    {
        if (_db is null) _db = new Db(masterPassword);
        return _db;
    }

    public void CloseDb()
    {
        _db = null;
    }
}
