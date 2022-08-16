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

    public bool AddPassword(PasswordEntry password){
        var result = Passwords.Insert(password);
        return result is not null;
    }

}
