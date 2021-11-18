using System;
using System.Collections.Generic;
using System.IO;
using InnoTech.LegosForLife.Core.IServices;
using InnoTech.LegosForLife.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnoTech.LegosForLife.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController: ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new InvalidDataException("ProductService Cannot Be Null");
        }
        
        [HttpGet]
        public ActionResult<List<Product>> GetAll()
        {
            return _productService.GetProducts();
        }

        [HttpPost]
        public ActionResult<Product> Create([FromBody] Product product)
        {
            var createdProduct = _productService.Create(product);
            return Created($"https://localhost/api/products/{createdProduct}",createdProduct);
        }

        [HttpDelete]
        public void Remove(int id)
        {
             _productService.Delete(id);
            
        }

        [HttpPut]
        public ActionResult<Product> Update(int id, string newName)
        {
            var updatedProduct = _productService.UpdateById(id, newName);
            return Created($"https://localhost/api/products/{updatedProduct}", updatedProduct);
        }
    }
}