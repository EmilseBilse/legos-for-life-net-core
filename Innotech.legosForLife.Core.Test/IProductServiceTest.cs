using System.Collections.Generic;
using InnoTech.LegosForLife.Core.IServices;
using InnoTech.LegosForLife.Core.Models;
using Moq;
using Xunit;

namespace InnoTech.LegosForLife.Core.Test
{
    public class IProductServiceTest
    {
        [Fact]
        public void IProductService_IsAvailable()
        {
            var service = new Mock<IProductService>().Object;
            Assert.NotNull(service);
        }

        [Fact]
        public void GetProducts_WithNoParam_ReturnsListOfAllProducts()
        {
            var mock = new Mock<IProductService>();
            var fakeList = new List<Product>();
            mock.Setup(s => s.GetProducts())
                .Returns(fakeList);
            var service = mock.Object;
            Assert.Equal(fakeList, service.GetProducts());
        }

        [Fact]
        public void GetById_GetObjectWithIdOne()
        {
            var mock = new Mock<IProductService>();

            var expected = new Product {Id = 1, Name = "Lego1"};
            mock.Setup(s => s.ReadById(1))
                .Returns(expected);
            var service = mock.Object;
            Assert.Equal(expected, service.ReadById(1));
        }

        [Fact]
        public void UpdateByIdTest()
        {
            int id = 1;
            string newName = "Leeeego1";
            var mock = new Mock<IProductService>();

            var expected = new Product {Id = id, Name = newName};
            mock.Setup(s => s.UpdateById(id, newName))
                .Returns(expected);

            var service = mock.Object;
            Assert.Equal(expected,service.UpdateById(id, newName));
        }

        [Fact]
        public void CreateProduct()
        {
            var mock = new Mock<IProductService>();
            Product product = new Product
            {
                Id = 1,
                Name = "ost"
            };
            mock.Setup(s => s.Create(product)).Returns(true);
            var service = mock.Object;
            Assert.True(service.Create(product));
        }
    }
}