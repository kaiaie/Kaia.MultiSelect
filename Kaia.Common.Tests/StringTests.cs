using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kaia.Common;

namespace Kaia.Common.Tests
{
    [TestClass]
    public class StringTests
    {
        [TestMethod]
        public void InitialCapitalizationWorks()
        {
            // Arrange
            var sut = "man";

            // Act
            var result = sut.ToInitialCap();

            // Assert
            Assert.AreEqual("Man", result);
        }


        [TestMethod]
        public void PascalCaseWorks()
        {
            // Arrange
            var sut = "supplier_id";

            // Act
            var result = sut.ToPascalCase();

            // Assert
            Assert.AreEqual("SupplierId", result);
        }


        [TestMethod]
        public void SnakeCaseWorks()
        {
            // Arrange
            var sut = "LongIdentifierIsLong";

            // Act
            var result = sut.ToSnakeCaseLower();

            // Assert
            Assert.AreEqual("long_identifier_is_long", result);
        }
    }
}
