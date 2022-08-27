using System;
using Muscurdi.Exceptions;
using Muscurdi.Libs;
using Muscurdi.Models;

namespace Muscurdi.Services;
public class Db
{
    private const string MUSCURDI_DB_FILENAME = "Muscurdi.db";
    private const string MASTER_KEY_ID = "MASTER_KEY_ID";
    private MasterPassword? _key;
    private LiteDB.ILiteDatabase _db;
    public LiteDB.ILiteCollection<PasswordEntry> Passwords { get; init; }

    public static bool IsDbInitialised()
    {
        var db = new LiteDB.LiteDatabase(MUSCURDI_DB_FILENAME);
        var keys = db.GetCollection<Key>("keys");
        var key = keys.FindById(MASTER_KEY_ID);
        return (key is not null);
    }

    public Db(MasterPassword masterPassword)
    {
        _db = new LiteDB.LiteDatabase(MUSCURDI_DB_FILENAME);
        var keys = _db.GetCollection<Key>("keys");
        var key = keys.FindById(MASTER_KEY_ID);
        if (key is null)
        {
            var newKey = new Key() { Id = MASTER_KEY_ID, Value = Crypto.Encrypt(masterPassword.ToString(), masterPassword) };
            keys.Insert(newKey);
            _key = masterPassword;
        }
        else
        {
            try
            {
                _key = (Crypto.Decrypt(key.Value, masterPassword) == masterPassword.ToString()) ? masterPassword : null;
            }
            catch (Exception exc)
            {
                throw new InvalidPasswordException("Wrong Master Password");
            }
        }

        if (_key is null) throw new InvalidPasswordException("Wrong Master Password");

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
