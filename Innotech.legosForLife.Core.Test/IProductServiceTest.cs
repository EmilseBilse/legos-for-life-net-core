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