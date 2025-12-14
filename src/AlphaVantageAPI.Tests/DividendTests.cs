using Tudormobile.AlphaVantage.Entities;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class DividendTests
{
    [TestMethod]
    public void Constructor_SetsPropertiesToDefault()
    {
        // Act
        var dividend = new Dividend();

        // Assert
        Assert.AreEqual(default, dividend.DeclarationDate);
        Assert.AreEqual(default, dividend.ExDividendDate);
        Assert.AreEqual(default, dividend.RecordDate);
        Assert.AreEqual(default, dividend.PaymentDate);
        Assert.AreEqual(0m, dividend.Amount);
    }

    [TestMethod]
    public void SetProperties_WithValidValues_StoresCorrectly()
    {
        // Arrange
        var declarationDate = new DateOnly(2023, 8, 10);
        var exDividendDate = new DateOnly(2023, 8, 11);
        var recordDate = new DateOnly(2023, 8, 14);
        var paymentDate = new DateOnly(2023, 9, 1);
        var amount = 0.23m;

        // Act
        var dividend = new Dividend
        {
            DeclarationDate = declarationDate,
            ExDividendDate = exDividendDate,
            RecordDate = recordDate,
            PaymentDate = paymentDate,
            Amount = amount
        };

        // Assert
        Assert.AreEqual(declarationDate, dividend.DeclarationDate);
        Assert.AreEqual(exDividendDate, dividend.ExDividendDate);
        Assert.AreEqual(recordDate, dividend.RecordDate);
        Assert.AreEqual(paymentDate, dividend.PaymentDate);
        Assert.AreEqual(amount, dividend.Amount);
    }

    [TestMethod]
    public void Amount_WithPreciseDecimal_RetainsPrecision()
    {
        // Arrange
        var dividend = new Dividend();
        var preciseAmount = 1.234567m;

        // Act
        dividend.Amount = preciseAmount;

        // Assert
        Assert.AreEqual(preciseAmount, dividend.Amount);
    }

    [TestMethod]
    public void Amount_WithZero_StoresCorrectly()
    {
        // Arrange
        var dividend = new Dividend
        {
            // Act
            Amount = 0m
        };

        // Assert
        Assert.AreEqual(0m, dividend.Amount);
    }

    [TestMethod]
    public void Amount_WithNegativeValue_StoresCorrectly()
    {
        // Arrange
        var dividend = new Dividend();
        var negativeAmount = -0.50m;

        // Act
        dividend.Amount = negativeAmount;

        // Assert
        Assert.AreEqual(negativeAmount, dividend.Amount);
    }

    [TestMethod]
    public void DateProperties_WithMinValue_StoresCorrectly()
    {
        // Arrange
        var dividend = new Dividend
        {
            // Act
            DeclarationDate = DateOnly.MinValue,
            ExDividendDate = DateOnly.MinValue,
            RecordDate = DateOnly.MinValue,
            PaymentDate = DateOnly.MinValue
        };

        // Assert
        Assert.AreEqual(DateOnly.MinValue, dividend.DeclarationDate);
        Assert.AreEqual(DateOnly.MinValue, dividend.ExDividendDate);
        Assert.AreEqual(DateOnly.MinValue, dividend.RecordDate);
        Assert.AreEqual(DateOnly.MinValue, dividend.PaymentDate);
    }

    [TestMethod]
    public void DateProperties_WithMaxValue_StoresCorrectly()
    {
        // Arrange
        var dividend = new Dividend
        {
            // Act
            DeclarationDate = DateOnly.MaxValue,
            ExDividendDate = DateOnly.MaxValue,
            RecordDate = DateOnly.MaxValue,
            PaymentDate = DateOnly.MaxValue
        };

        // Assert
        Assert.AreEqual(DateOnly.MaxValue, dividend.DeclarationDate);
        Assert.AreEqual(DateOnly.MaxValue, dividend.ExDividendDate);
        Assert.AreEqual(DateOnly.MaxValue, dividend.RecordDate);
        Assert.AreEqual(DateOnly.MaxValue, dividend.PaymentDate);
    }

    [TestMethod]
    public void Dividend_ImplementsIEntity()
    {
        // Arrange
        var dividend = new Dividend();

        // Act & Assert
        Assert.IsInstanceOfType<IEntity>(dividend);
    }

    [TestMethod]
    public void TwoDividends_WithSameValues_AreIndependent()
    {
        // Arrange
        var dividend1 = new Dividend
        {
            DeclarationDate = new DateOnly(2023, 8, 10),
            ExDividendDate = new DateOnly(2023, 8, 11),
            RecordDate = new DateOnly(2023, 8, 14),
            PaymentDate = new DateOnly(2023, 9, 1),
            Amount = 0.23m
        };

        var dividend2 = new Dividend
        {
            DeclarationDate = new DateOnly(2023, 8, 10),
            ExDividendDate = new DateOnly(2023, 8, 11),
            RecordDate = new DateOnly(2023, 8, 14),
            PaymentDate = new DateOnly(2023, 9, 1),
            Amount = 0.23m
        };

        // Act
        dividend1.Amount = 0.50m;

        // Assert
        Assert.AreEqual(0.50m, dividend1.Amount);
        Assert.AreEqual(0.23m, dividend2.Amount);
    }

    [TestMethod]
    public void Amount_WithLargeValue_StoresCorrectly()
    {
        // Arrange
        var dividend = new Dividend();
        var largeAmount = 999999.99m;

        // Act
        dividend.Amount = largeAmount;

        // Assert
        Assert.AreEqual(largeAmount, dividend.Amount);
    }

    [TestMethod]
    public void DateProperties_CanBeModifiedIndependently()
    {
        // Arrange
        var dividend = new Dividend
        {
            DeclarationDate = new DateOnly(2023, 1, 1),
            ExDividendDate = new DateOnly(2023, 1, 2),
            RecordDate = new DateOnly(2023, 1, 3),
            PaymentDate = new DateOnly(2023, 1, 4)
        };

        // Act
        dividend.DeclarationDate = new DateOnly(2024, 1, 1);

        // Assert
        Assert.AreEqual(new DateOnly(2024, 1, 1), dividend.DeclarationDate);
        Assert.AreEqual(new DateOnly(2023, 1, 2), dividend.ExDividendDate);
        Assert.AreEqual(new DateOnly(2023, 1, 3), dividend.RecordDate);
        Assert.AreEqual(new DateOnly(2023, 1, 4), dividend.PaymentDate);
    }

    [TestMethod]
    public void SetProperties_WithTypicalDividendData_WorksCorrectly()
    {
        // Arrange & Act
        var dividend = new Dividend
        {
            DeclarationDate = new DateOnly(2025, 11, 15),
            ExDividendDate = new DateOnly(2025, 11, 28),
            RecordDate = new DateOnly(2025, 11, 29),
            PaymentDate = new DateOnly(2025, 12, 15),
            Amount = 0.55m
        };

        // Assert
        Assert.AreEqual(new DateOnly(2025, 11, 15), dividend.DeclarationDate);
        Assert.AreEqual(new DateOnly(2025, 11, 28), dividend.ExDividendDate);
        Assert.AreEqual(new DateOnly(2025, 11, 29), dividend.RecordDate);
        Assert.AreEqual(new DateOnly(2025, 12, 15), dividend.PaymentDate);
        Assert.AreEqual(0.55m, dividend.Amount);
    }
}
