using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EssentialTools.Models;
using System.Linq;
using Moq;


namespace EssentialTools.Tests
{
    [TestClass]
    public class UnitTest2
    {
        private Product[] products = {
            new Product {Name = "Каяк", Category = "Водные виды спорта", Price = 275M},
            new Product {Name = "Спасательный жилет", Category = "Водные виды спорта", Price = 48.95M},
            new Product {Name = "Мяч", Category = "Футбол", Price = 19.50M},
            new Product {Name = "Угловой флажок", Category = "Футбол", Price = 34.95M}
        };
        [TestMethod]
        public void TesSum_Products_Correctly()
        {
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>()))
                .Returns<decimal>(total => total);
            var target = new LinqValueCalculator(mock.Object);
            var result = target.ValueProducts(products);
            Assert.AreEqual(products.Sum(e =>e.Price), result);
           
        }
        private Product[] createProduct(decimal value)
        {
            return new[] { new Product { Price = value } };
        }
        [TestMethod]
        public void Pass_Throught_Variable_Discounts()
        {
            // Arrange
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>()))
                .Returns<decimal>(total => total);
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v == 0)))
                .Returns<decimal>(total => total);
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v > 100)))
                .Returns<decimal>(total => total*0.9M);
            mock.Setup(m => m.ApplyDiscount(It.IsInRange<decimal>(10, 100,
                Range.Inclusive))).Returns<decimal>(total => total-5);
            var target = new LinqValueCalculator(mock.Object);
            // Act
            decimal FiveDollarDiscount = target.ValueProducts(createProduct(5));
            decimal TenDollarDiscount = target.ValueProducts(createProduct(10));
            decimal FiftyDollarDiscount = target.ValueProducts(createProduct(50));
            decimal HundredDollarDiscount = target.ValueProducts(createProduct(100));
            decimal FiveHundredDollarDiscount = target.ValueProducts(createProduct(500));
            // Assert
            Assert.AreEqual(5, FiveDollarDiscount, "$5 lost");
            Assert.AreEqual(5, TenDollarDiscount, "$10 lost");
            Assert.AreEqual(45, FiftyDollarDiscount, "$50 lost");
            Assert.AreEqual(95, HundredDollarDiscount, "$100 lost");
            Assert.AreEqual(450, FiveHundredDollarDiscount, "$500 lost");
            target.ValueProducts(createProduct(0));

        }
    }
}
