using System.Collections.Generic;
using InnoTech.LegosForLife.Core.Models;

namespace InnoTech.LegosForLife.Core.IServices
{
    public interface IProductService
    {
        List<Product> GetProducts();
        public Product ReadById(int i);


        bool Create(Product product);
        Product UpdateById(int id, string newName);
    }
    
    
}