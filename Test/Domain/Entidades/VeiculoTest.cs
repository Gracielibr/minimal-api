using MinimalApi.Dominio.Entidades;


namespace Test.Domain.Entidades;

[TestClass]
public class VeiculoTest
{
    [TestMethod]
    public void TestarGetSetPropriedades()
    {
        // Arrange
        var v = new Veiculo();

        // Act
        v.Id = 1;
        v.Marca = "Sandero";
        v.Ano = 2014;
        
        //Assert
        Assert.AreEqual(1, v.Id);
        Assert.AreEqual("Sandero", v.Marca);    
        Assert.AreEqual(2014, v.Ano);
        
      
    }
}
