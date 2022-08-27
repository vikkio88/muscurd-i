using Muscurdi.Libs;
using Muscurdi.Models;

namespace Muscurdi.Services;

// I hate this shit, will need to do DependencyInjection properly
public class S : Singleton<S>
{
    private Db? _db;

    public Db? Db
    {
        get
        {
            if (_db is null)
            {
                _db = new();
            }
            return _db;
        }
    }
}
