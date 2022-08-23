namespace Muscurdi.Libs;

public class Singleton<T> where T : class, new()
{
    private static volatile T _instance;
    private static readonly object _lock = new object();
    public static T Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            lock (_lock)
            {
                if (_instance == null)
                    _instance = new T();
            }

            return _instance;
        }
    }
}
