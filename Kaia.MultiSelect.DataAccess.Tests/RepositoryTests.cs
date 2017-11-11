using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kaia.MultiSelect.Domain;

namespace Kaia.MultiSelect.DataAccess.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        protected UnitOfWork unitOfWork;

        [TestInitialize]
        public void SetUp()
        {
            unitOfWork = new UnitOfWork("System.Data.SQLite",
                @"Data Source=C:\Users\ken\Source\Repos\Kaia.MultiSelect\Kaia.MultiSelect.Database\MultiSelect.sqlite3;Version=3;");
        }


        [TestCleanup]
        public void TearDown()
        {
            unitOfWork.Dispose();
        }


        [TestMethod]
        public void CanGetAllSuppliers()
        {
            // Arrange
            var sut = unitOfWork;

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
            var sut = unitOfWork;
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
            var sut = unitOfWork;
            var supplierIds = new long[] { 2L, 3L, 100L };

            // Act
            var result = sut.SupplierRepository.Get(supplierIds)
                .ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
        }


        [TestMethod]
        public void CanInsert()
        {
            // Arrange
            var sut = unitOfWork;
            var newSupplier = new NewSupplier
            {
                SupplierName = "Test Supplier",
                Status = 10,
                City = "Dublin"
            };

            //Act
            var result = sut.SupplierRepository.Create(newSupplier);

            // Assert
            Assert.IsTrue(result > 0);
        }
    }
}
