using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EntityFrameworkCore.Testing.Moq;
using InnoTech.LegosForLife.Core.Models;
using InnoTech.LegosForLife.DataAccess.Entities;
using InnoTech.LegosForLife.DataAccess.Repositories;
using InnoTech.LegosForLife.Domain.IRepositories;
using Newtonsoft.Json;
using Xunit;

namespace InnoTech.LegosForLife.DataAccess.Test.Repositories
{
    public class ProductRepositoryTest
    {
        [Fact]
        public void ProductRepository_IsIProductRepository()
        {
            var fakeContext = Create.MockedDbContextFor<MainDbContext>();
            var repository = new ProductRepository(fakeContext);
            Assert.IsAssignableFrom<IProductRepository>(repository);
        }
        
        [Fact]
        public void ProductRepository_WithNullDBContext_ThrowsInvalidDataException()
        {
            Assert.Throws<InvalidDataException>(() => new ProductRepository(null));
        }
        
        [Fact]
        public void ProductRepository_WithNullDBContext_ThrowsExceptionWithMessage()
        {
            var exception = Assert
                .Throws<InvalidDataException>(() => new ProductRepository(null));
            Assert.Equal("Product Repository Must have a DBContext", exception.Message);
        }
        
        [Fact]
        public void FindAll_GetAllProductsEntitiesInDBContext_AsAListOfProducts()
        {
            //Arrange
            var fakeContext = Create.MockedDbContextFor<MainDbContext>();
            var repository = new ProductRepository(fakeContext);
            var list = new List<ProductEntity>
            {
                new ProductEntity { Id = 1, Name = "Lego1" },
                new ProductEntity { Id = 2, Name = "Lego2" },
                new ProductEntity { Id = 3, Name = "Lego3" }
            };
            fakeContext.Set<ProductEntity>().AddRange(list);
            fakeContext.SaveChanges();
            
            var expectedList = list
                .Select(pe => new Product
                {
                    Id = pe.Id,
                    Name = pe.Name
                })
                .ToList();
            
            //Act
            var actualResult = repository.FindAll();
            
            //Assert
            Assert.Equal(expectedList, actualResult, new Comparer());
        }

        [Fact]
        public void Create_ValidProduct()
        {
           //Arrange
           var fakeContext = Create.MockedDbContextFor<MainDbContext>();
           var repository = new ProductRepository(fakeContext);
           var product = new Product
           {
               Id = 1,
               Name = "Ost"
           };
           List<Product> products = new List<Product>();
           products.Add(product);
           

           //Act
           bool productCreated = repository.Create(product);
           
           //Assert
           Assert.Equal(products, repository.FindAll(), new Comparer());
        }

        [Theory]
        [InlineData(1,"Lego1")]
        [InlineData(3,"Lego3")]
        public void GetByIdTest(int id, string name)
        {
            var fakeContext = Create.MockedDbContextFor<MainDbContext>();
            var repository = new ProductRepository(fakeContext);
            var list = new List<ProductEntity>
            {
                new ProductEntity { Id = 1, Name = "Lego1" },
                new ProductEntity { Id = 2, Name = "Lego2" },
                new ProductEntity { Id = 3, Name = "Lego3" }
            };
            fakeContext.Set<ProductEntity>().AddRange(list);
            fakeContext.SaveChanges();

            string expected = JsonConvert.SerializeObject(new Product {Id = id, Name = name});

            string actual = JsonConvert.SerializeObject(repository.ReadById(id));
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(1, "LEEEEGOOO1")]
        [InlineData(2, "HEJSA LEGO 2")]
        public void UpdateNameByIdTest(int id, string newName)
        {
            var fakeContext = Create.MockedDbContextFor<MainDbContext>();
            var repository = new ProductRepository(fakeContext);
            var list = new List<ProductEntity>
            {
                new ProductEntity {Id = 1, Name = "Lego1"},
                new ProductEntity {Id = 2, Name = "Lego2"},
                new ProductEntity {Id = 3, Name = "Lego3"}
            };
            fakeContext.Set<ProductEntity>().AddRange(list);
            fakeContext.SaveChanges();

            string expected = JsonConvert.SerializeObject(new Product {Id = id, Name = newName});

            string actual = JsonConvert.SerializeObject(repository.UpdateNameById(id, newName));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Delete_ValidProduct()
        {
            var fakeContext = Create.MockedDbContextFor<MainDbContext>();
            var repository = new ProductRepository(fakeContext);
            int mockProductId = 1;
            Product product = new Product
            {
                Id = mockProductId,
                Name = "Ost"
            };
            repository.Create(product);
            
            Assert.True(repository.Delete(mockProductId));
            
        }
        
    }


    public class Comparer: IEqualityComparer<Product>
    {
        public bool Equals(Product x, Product y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Id == y.Id && x.Name == y.Name;
        }

        public int GetHashCode(Product obj)
        {
            return HashCode.Combine(obj.Id, obj.Name);
        }

    }
}