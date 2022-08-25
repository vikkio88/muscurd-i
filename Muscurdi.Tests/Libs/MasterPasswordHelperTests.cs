namespace Muscurdi.Tests.Libs;
using Muscurdi.Libs;
using Muscurdi.Models;
public class MasterPasswordHelperTests
{
    [Fact]
    public void MasterPasswordHelperDoesItsThingTest()
    {
        var password = MasterPasswordHelper.Generate();
        var other = MasterPassword.Make(password.ToMemorable());
        Assert.Equal(other.ToMemorable(), password.ToMemorable());
    }
}
