using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kaia.Common.DataAccess.Contract;
using Kaia.Common.DataAccess;
using System.Collections.Generic;
using Kaia.MultiSelect.Domain;
using System.Linq;

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

        [TestMethod]
        public void GettingUpdateQueryWorks()
        {
            // Arrange
            var entities = new Supplier[]
            {
                new Supplier(1, "Tom", 10, "Dublin"),
                new Supplier(2, "Pat", 20, "London"),
                new Supplier(3, "Harry", 30, "London")
            };
            var modifier = entities.Where(s => s.City == "London").GetModifier();
            modifier.City.Value = "Berlin";
            modifier.ModificationType = ModificationType.Update;

            // Act
            var sut = QueryHelper.Default.GetUpdateQuery(modifier);

            // Assert
            Assert.IsNotNull(sut);
            Assert.IsFalse(string.IsNullOrEmpty(sut.Sql));
            Assert.IsNotNull(sut.Parameters);
        }

        [TestMethod]
        public void GettingDuplicateQueryWorks()
        {
            // Arrange
            var entities = new Supplier[]
            {
                new Supplier(1, "Tom", 10, "Dublin"),
                new Supplier(2, "Pat", 20, "London"),
                new Supplier(3, "Harry", 30, "London")
            };
            var modifier = entities.Where(s => s.City == "London").GetModifier();
            modifier.City.Value = "Berlin";
            modifier.ModificationType = ModificationType.Duplicate;

            // Act
            var sut = QueryHelper.Default.GetDuplicateQuery(modifier);

            // Assert
            Assert.IsNotNull(sut);
            Assert.IsFalse(string.IsNullOrEmpty(sut.Sql));
            Assert.IsNotNull(sut.Parameters);
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
