namespace Muscurdi.Tests.Libs;
using Muscurdi.Libs;
using Muscurdi.Models;

public class CryptoTests
{
    [Fact]
    public void CryptoDoesItsThingTest()
    {
        var masterPassword = MasterPassword.Make("some-some-nope-yeah-1234");
        var text = "some text";
        var encrypted = Crypto.Encrypt(text, masterPassword);
        Assert.NotEqual(text, encrypted);
        var decrytpted = Crypto.Decrypt(encrypted, masterPassword);
        Assert.Equal(decrytpted, text);
    }
}
