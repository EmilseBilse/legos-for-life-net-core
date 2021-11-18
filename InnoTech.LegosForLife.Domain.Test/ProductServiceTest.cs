using System.Collections.Generic;
using System.IO;
using InnoTech.LegosForLife.Core.IServices;
using InnoTech.LegosForLife.Core.Models;
using InnoTech.LegosForLife.Domain.IRepositories;
using InnoTech.LegosForLife.Domain.Services;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace InnoTech.LegosForLife.Domain.Test
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _mock;
        private readonly ProductService _service;
        private readonly List<Product> _expected;

        public ProductServiceTest()
        {
            _mock = new Mock<IProductRepository>();
            _service = new ProductService(_mock.Object);
            _expected = new List<Product>
            {
                new Product { Id = 1, Name = "Lego1" },
                new Product { Id = 2, Name = "Lego2" }
            };
        }
        
        [Fact]
        public void ProductService_IsIProductService()
        {
            Assert.True(_service is IProductService);
        }
        
        
        [Fact]
        public void ProductService_WithNullProductRepository_ThrowsInvalidDataException()
        {
            Assert.Throws<InvalidDataException>(
                () => new ProductService(null)
                );

        }
        
        [Fact]
        public void ProductService_WithNullProductRepository_ThrowsExceptionWithMessage()
        {
            var exception = Assert.Throws<InvalidDataException>(
                () => new ProductService(null)
            );
            Assert.Equal("ProductRepository Cannot Be Null",exception.Message);
        }
        
        
        [Fact]
        public void GetProducts_CallsProductRepositoriesFindAll_ExactlyOnce()
        {
            _service.GetProducts();
            _mock.Verify(r => r.FindAll(), Times.Once);
        }
        
        [Fact]
        public void GetProducts_NoFilter_ReturnsListOfAllProducts()
        {
            _mock.Setup(r => r.FindAll())
                .Returns(_expected);
            var actual = _service.GetProducts();
            Assert.Equal(_expected, actual);
        }

        [Fact]
        public void CreateProduct_ValidProduct_ReturnsTrue()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Ost"
            };
            List<Product> products = new List<Product>();
            products.Add(product);
            _mock.Setup(r => r.FindAll())
                .Returns(products);

            //Act
            bool productCreated = _service.Create(product);
           
            //Assert
            Assert.Equal(products, _service.GetProducts());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ReadByIdTest(int id)
        {
            _mock.Setup(r => r.ReadById(id))
                .Returns(new Product {Id = id, Name = "Lego1"});

            var actual = JsonConvert.SerializeObject(_service.ReadById(id));
            var expected = JsonConvert.SerializeObject(new Product {Id = id, Name = "Lego1"});
            Assert.Equal(expected,actual);
        }

        [Fact]
        public void UpdateProductTest()
        {
            int id = 1;
            string newName = "Leeeego33";
            _mock.Setup(r => r.UpdateNameById(id, newName))
                .Returns(new Product {Id = id, Name = newName});

            string actual = JsonConvert.SerializeObject(_service.UpdateNameById(id, newName));
            string expected = JsonConvert.SerializeObject(new Product {Id = id, Name = newName});
            
            Assert.Equal(expected,actual);
        }

        [Fact]
        public void DeleteProduct()
        { 
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
            _mock.Setup(s => s.FindAll())
                .Returns(products);

            _service.Create(product);
            _service.Create(new Product
            {
                Id = 2,
                Name = "Brød"
            });
            _service.Create(new Product
            {
                Id = 3,
                Name = "Vin"
            });

            _service.Delete(mockProductId);

            Assert.Equal(products, _service.GetProducts());
        }
    }
}