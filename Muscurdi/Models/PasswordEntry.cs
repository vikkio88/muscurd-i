namespace Muscurdi.Models;
public class PasswordEntry
{
    [LiteDB.BsonId]
    public LiteDB.ObjectId Id { get; set; }
    public string Name { get; set; }

    //@TODO maybe this needs to do Crypto on set and get
    public string Password { get; set; }
}
