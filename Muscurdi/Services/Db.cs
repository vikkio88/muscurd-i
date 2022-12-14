using System;
using System.Collections.Generic;
using System.Linq;
using Muscurdi.Exceptions;
using Muscurdi.Libs;
using Muscurdi.Models;

namespace Muscurdi.Services;
public class Db
{
    private const string MUSCURDI_DB_FILENAME = "Muscurdi.db";
    private const string MASTER_KEY_ID = "MASTER_KEY_ID";
    private const string KEY_COLLECTION = "keys";
    private const string PASSWORD_ENTRIES_COLLECTION = "passwordEntries";
    private MasterPassword? _key;
    private LiteDB.ILiteDatabase _db;
    public LiteDB.ILiteCollection<PasswordEntry>? Passwords { get; set; } = null;

    public static bool IsDbInitialised()
    {
        var db = new LiteDB.LiteDatabase(MUSCURDI_DB_FILENAME);
        var keys = db.GetCollection<Key>(KEY_COLLECTION);
        var key = keys.FindById(MASTER_KEY_ID);
        return (key is not null);
    }

    public Db()
    {
        _db = new LiteDB.LiteDatabase(MUSCURDI_DB_FILENAME);
    }

    public void Init(MasterPassword masterPassword)
    {
        var keys = _db.GetCollection<Key>(KEY_COLLECTION);
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
            catch (Exception)
            {
                throw new InvalidPasswordException("Wrong Master Password");
            }
        }

        if (_key is null) throw new InvalidPasswordException("Wrong Master Password");

        Passwords = _db.GetCollection<PasswordEntry>(PASSWORD_ENTRIES_COLLECTION);
    }

    public int Count() => Passwords.Count();

    public PasswordEntry? GetById(LiteDB.ObjectId id)
    {
        var password = Passwords.FindById(id);
        password.Password = Crypto.Decrypt(password.Password, _key);
        return password;
    }

    public List<PasswordEntry>? GetAll() => Passwords.FindAll().Select(p =>
        {
            p.Password = Crypto.Decrypt(p.Password, _key) ?? "ERROR-DECRYPTING";
            return p;
        }).ToList();


    public void UpdatePassword(PasswordEntry password)
    {
        password.Password = Crypto.Encrypt(password.Password, _key);
        Passwords.Update(password);
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
        catch (LiteDB.LiteException)
        {
            result = null;

        }
        return result is not null;
    }

    public List<PasswordEntry>? FindPasswordsByName(string query)
    {
        return Passwords?.Find(x => x.Name.Contains(query))
        .Select(p =>
        {
            p.Password = Crypto.Decrypt(p.Password, _key) ?? "ERROR-DECRYPTING";
            return p;
        }).ToList();
    }

}
