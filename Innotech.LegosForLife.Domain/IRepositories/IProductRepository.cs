using System.Collections.Generic;
using InnoTech.LegosForLife.Core.Models;

namespace InnoTech.LegosForLife.Domain.IRepositories
{
    public interface IProductRepository
    {
        List<Product> FindAll();

        bool Create(Product product);
        Product ReadById(int id);
        bool Delete(int product);
    }
}