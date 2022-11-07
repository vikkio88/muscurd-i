namespace Muscurdi.Models;
public class PasswordEntry
{
    [LiteDB.BsonId]
    public LiteDB.ObjectId? Id { get; set; }
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}
