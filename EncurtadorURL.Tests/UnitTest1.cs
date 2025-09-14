namespace EncurtadorURL.Tests;

public class UnitTest1
{
    public static int Add(int a, int b) =>  a + b;
    
    
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(1, 3, 4)]
    [InlineData(-1, -1, -2)]
    public void SumWithValuesBiggerZeroValuesThenReturnsValueValid(int a, int b, int resultadoEsperado)
    {
        // Act
        var resultadoReal = Add(a, b);
        // Assert
        Assert.Equal(resultadoEsperado, resultadoReal);
    }
}


