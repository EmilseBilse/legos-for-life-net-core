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
            
            
            var product = new Product
            {
                Id = 1,
                Name = "Ost"
            };
            List<Product> products = new List<Product>();
            products.Add(product);
            
            mock.Setup(s => s.GetProducts())
                .Returns(products);
            var service = mock.Object;
            mock.Setup(s => s.GetProducts())
                .Returns(products);

            //Act
            bool productCreated = service.Create(product);
           
            //Assert
            Assert.Equal(products, service.GetProducts());
        }

        [Fact]
        public void DeleteProduct()
        {
            var mock = new Mock<IProductService>();
            
            int mockProductId = 1;
            Product product = new Product
            {
                Id = mockProductId,
                Name = "Ost"
            };
            
            List<Product> products = new List<Product>
            {
                new() {Id = 2, Name = "Brød"},
                new() { Id = 3, Name = "Vin"},
                
            };
            mock.Setup(s => s.GetProducts())
                .Returns(products);
            var service = mock.Object;
            
            service.Create(product);
            service.Create(new Product
            {
                Id = 2,
                Name = "Brød"
            });
            service.Create(new Product
            {
                Id = 3,
                Name = "Vin"
            });

            service.Delete(mockProductId);

            Assert.Equal(products, service.GetProducts());
        }
    }
}