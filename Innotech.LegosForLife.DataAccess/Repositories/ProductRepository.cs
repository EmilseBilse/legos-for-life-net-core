using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InnoTech.LegosForLife.Core.Models;
using InnoTech.LegosForLife.DataAccess.Entities;
using InnoTech.LegosForLife.Domain.IRepositories;

namespace InnoTech.LegosForLife.DataAccess.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly MainDbContext _ctx;

        public ProductRepository(MainDbContext ctx)
        {
            if (ctx == null) throw new InvalidDataException("Product Repository Must have a DBContext");
            _ctx = ctx;
        }
        public List<Product> FindAll()
        {
            return _ctx.Products
                .Select(pe => new Product
                {
                    Id = pe.Id,
                    Name = pe.Name
                })
                .ToList();
        }
        
        public bool Create(Product product)
        {
            try
            {

                _ctx.Products.Add(new ProductEntity
                {
                    Id = product.Id,
                    Name = product.Name
                });
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Product ReadById(int id)
        {
            if (FindAll().Capacity < id)
            {
                throw new ArgumentException("Id is too high");
            }
            
            return _ctx.Products
                .Select(pe => new Product
                {
                    Id = pe.Id,
                    Name = pe.Name
                }).FirstOrDefault(pe => pe.Id == id);
        }

        public Product UpdateNameById(int id, string newName)
        {
            ProductEntity entity = _ctx.Products.Update(new ProductEntity
            {
                Id = id,
                Name = newName
            }).Entity;

            return new Product
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}