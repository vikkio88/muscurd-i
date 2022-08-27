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
        var decrypted = Crypto.Decrypt(encrypted, masterPassword);
        Assert.Equal(decrypted, text);
    }

    [Fact]
    public void CryptoDecryptReturnsNullIfTryingToDecryptWithWrongMasterPassword()
    {
        var masterPassword1 = MasterPassword.Make("some-some-nope-yeah-1234");
        var masterPassword2 = MasterPassword.Make("some-some-nope-yeah-1235");
        var text = "some text";
        var encrypted = Crypto.Encrypt(text, masterPassword1);
        Assert.NotEqual(text, encrypted);
        var decrypted = Crypto.Decrypt(encrypted, masterPassword1);
        Assert.NotNull(decrypted);
        decrypted = Crypto.Decrypt(encrypted, masterPassword2);
        Assert.Null(decrypted);
    }

    //@TODO if you try to decrypt with a wrong master password this throws, needs testing
}
