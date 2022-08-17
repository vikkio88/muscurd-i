using Muscurdi.Libs;
using Muscurdi.Models;

namespace Muscurdi.Services;
public class Db : Singleton<Db>
{
    private LiteDB.ILiteDatabase _db;
    public LiteDB.ILiteCollection<PasswordEntry> Passwords { get; init; }

    public Db()
    {
        _db = new LiteDB.LiteDatabase("Muscurdi.db");
        Passwords = _db.GetCollection<PasswordEntry>("passwordEntries");
    }

    public bool AddPassword(PasswordEntry password)
    {
        Passwords.EnsureIndex(x => x.Name, true);
        LiteDB.BsonValue? result = null;
        try
        {
            result = Passwords.Insert(password);
        }
        catch (LiteDB.LiteException _)
        {
            result = null;

        }
        return result is not null;
    }

}
