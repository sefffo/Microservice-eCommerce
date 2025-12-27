using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTO_s.Conversions
{
    public static class ProductConversion
    {
        public static ProductDto ToProductDto(this Product product)

            => new ProductDto(
                product.Id,
                product.Name ?? string.Empty,
                product.Price,
                product.Quantity
            );

        public static Product ToProductEntity(this ProductDto productDto)

            => new Product()
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Price = productDto.Price,
                Quantity = productDto.Quantity
            };


        public static IEnumerable<ProductDto> ToProductDtoList(this IEnumerable<Product> products)
            => products.Select(p => p.ToProductDto()).ToList();


        public static IEnumerable<Product> ToProductList(this IEnumerable<ProductDto> productsDto)
             => productsDto.Select(p => p.ToProductEntity()).ToList();

        // Method to convert a Product entity and a collection of Product entities to their corresponding DTOs
        //public static (ProductDto?, IEnumerable<ProductDto>?) FromEntity(Product product, IEnumerable<Product> products)
        //{

        //}

    }
}
