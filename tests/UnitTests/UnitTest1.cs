namespace UnitTests;

public class UnitTest1
{
    [Fact]
    public void Sample_Test_Only()
    {
        var soma = 1 + 1;
        
        Assert.Equal(2, soma);
    }
}