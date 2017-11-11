using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kaia.MultiSelect.DataAccess.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void CanGetAllSuppliers()
        {
            // Arrange
            var sut = new UnitOfWork("System.Data.SQLite", 
                @"Data Source=C:\Users\ken\Source\Repos\Kaia.MultiSelect\Kaia.MultiSelect.Database\MultiSelect.sqlite3;Version=3;");

            // Act
            var results = sut.SupplierRepository.GetAll().ToList();

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);
        }

        [TestMethod]
        public void CanGetSingleSupplier()
        {
            // Arrange
            var sut = new UnitOfWork("System.Data.SQLite",
                @"Data Source=C:\Users\ken\Source\Repos\Kaia.MultiSelect\Kaia.MultiSelect.Database\MultiSelect.sqlite3;Version=3;");
            var supplierId = 4L;

            // Act
            var result = sut.SupplierRepository.Get(supplierId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.SupplierId == supplierId);
        }

        [TestMethod]
        public void CanGetSomeSuppliers()
        {
            // Arrange
            var sut = new UnitOfWork("System.Data.SQLite",
                @"Data Source=C:\Users\ken\Source\Repos\Kaia.MultiSelect\Kaia.MultiSelect.Database\MultiSelect.sqlite3;Version=3;");
            var supplierIds = new long[] { 2L, 3L, 100L };

            // Act
            var result = sut.SupplierRepository.Get(supplierIds)
                .ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
        }
    }
}
