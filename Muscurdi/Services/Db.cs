using System;
using Muscurdi.Libs;
using Muscurdi.Models;

namespace Muscurdi.Services;
public class Db
{
    private MasterPassword? _key;
    private LiteDB.ILiteDatabase _db;
    public LiteDB.ILiteCollection<PasswordEntry> Passwords { get; init; }

    public static bool IsDbInitialised()
    {
        var db = new LiteDB.LiteDatabase("Muscurdi.db");
        var keys = db.GetCollection<Key>("keys");
        var key = keys.FindById("master");
        return (key is not null);
    }

    public Db(MasterPassword masterPassword)
    {
        _db = new LiteDB.LiteDatabase("Muscurdi.db");
        var keys = _db.GetCollection<Key>("keys");
        var key = keys.FindById("master");
        if (key is null)
        {
            var newKey = new Key() { Id = "master", Value = Crypto.Encrypt(masterPassword.ToString(), masterPassword) };
            keys.Insert(newKey);
            _key = masterPassword;
        }
        else
        {
            Console.WriteLine(key.Value);
            try
            {

                _key = (Crypto.Decrypt(key.Value, masterPassword) == masterPassword.ToString()) ? masterPassword : null;
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException("Wrong Master Password");
            }
        }

        if (_key is null) throw new InvalidOperationException("Wrong Master Password");

        Passwords = _db.GetCollection<PasswordEntry>("passwordEntries");
    }

    public bool AddPassword(PasswordEntry password)
    {
        Passwords.EnsureIndex(x => x.Name, true);
        password.Password = Crypto.Encrypt(password.Password, _key);
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
