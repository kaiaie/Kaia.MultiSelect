using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kaia.Common.DataAccess.Contract;
using Kaia.Common.DataAccess;
using System.Collections.Generic;
using Kaia.MultiSelect.Domain;

namespace Kaia.MultiSelect.DataAccess.Tests
{
    [TestClass]
    public class QueryHelperTests
    {
        [TestMethod]
        public void GetTableNameWorksForModifier()
        {
            // Arrange
            var sut = new StubModifier();

            // Act
            var result = QueryHelper.Default.GetTableName<StubModifier>();

            // Assert
            Assert.AreEqual("stubs", result);
        }

        [TestMethod]
        public void GettingModifierWorks()
        {
            // Arrange
            var entities = new Supplier[]
            {
                new Supplier(1, "Tom", 10, "Dublin"),
                new Supplier(2, "Pat", 20, "Dublin")
            };

            // Act
            var sut = entities.GetModifier();
            sut.ModificationType = ModificationType.Update;

            // Assert
            Assert.IsNotNull(sut);
            Assert.AreEqual(entities.Length, sut.Ids.Count);
            Assert.IsTrue(sut.SupplierName.IsIndeterminate);
            Assert.IsFalse(sut.City.IsIndeterminate);
            Assert.AreEqual("Dublin", sut.City.Value);
        }

        class StubModifier : IEntityModifier
        {
            public IReadOnlyList<long> Ids
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public ModificationType ModificationType
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
