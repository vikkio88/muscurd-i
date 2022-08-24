namespace Muscurdi.Tests.Models;
using Muscurdi.Models;
public class MasterPasswordTests
{
    [Fact]
    public void MasterPasswordMakingHappyPath()
    {
        var password = MasterPassword.Make("nope-yeah-nope-yeah-1234");
        Assert.Equal("nope-yeah-nope-nope-yeah-nope-yeah-1234", $"{password}");
    }
    
    [Fact]
    public void MasterPasswordRejectsShortPasswords()
    {
        var result = Assert.Throws<System.ArgumentException>(() => MasterPassword.Make("nope-yeah-1234"));
        Assert.Contains("memorable", result.Message);
    }
    
    [Fact]
    public void MasterPasswordRejectsShortWord()
    {
        var result = Assert.Throws<System.ArgumentException>(() => MasterPassword.Make("nope-yeah-yeah-mammam-1234"));
        Assert.Contains("word", result.Message);
    }
    
    [Fact]
    public void MasterPasswordRejectsInvalidAppendix()
    {
        var result = Assert.Throws<System.ArgumentException>(() => MasterPassword.Make("nope-yeah-yeah-mamm-juve"));
        Assert.Contains("appendix", result.Message);
    }

    [Fact]
    public void MasterPasswordReturnsMemorable()
    {
        var password = MasterPassword.Make("nope-yeah-yeah-mamm-9999");
        Assert.Equal("nope-yeah-yeah-mamm-9999", password.ToMemorable());
    }
}
