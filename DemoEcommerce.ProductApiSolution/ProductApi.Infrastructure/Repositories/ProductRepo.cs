using Ecommerce.SherdLibrary.Logs;
using Ecommerce.SherdLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.Repositories
{

    // Implementing the IProduct interface for Product repository
    // This class will handle CRUD operations for Product entities
    //so we need to have a dbcontext here to interact with the database

    public class ProductRepo(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product Entity)
        {
            try
            {
                //quick check if product already exists
                var getProduct = context.Products
                    .FirstOrDefault(p => p.Name == Entity.Name);
                if (getProduct != null && !string.IsNullOrEmpty(getProduct.Name)) 
                    return new Response { flag = false, message = $"Product : {getProduct.Name} already exists." };

                //if not exists, proceed to add
                var result = await context.Products.AddAsync(Entity);
                if (result == null)
                    return new Response { flag = false, message = "Failed to create product." };

                await context.SaveChangesAsync(); // Save changes to the database
                return new Response { flag = true, message = $"Product : {getProduct.Name} created successfully." };

            }
            catch (Exception ex)
            {
                // Handle exception and return a failure response
                // You can log the exception here if needed
                LogsExceptions.LogException(ex);
                
                return new Response { flag = false, message = "Error Occurred while Creating Product" }; //local error message
            }
        }

        public async Task<Response> UpdateAsync(Product Entity)
        {
            try
            {
                var getProduct = await context.Products.FindAsync(Entity.Id);
                if (getProduct == null)
                    return new Response { flag = false, message = $"Product : {getProduct.Name} not found." };
                //context.Entry(getProduct).State = EntityState.Detached;// Detach the entity to avoid tracking issues
                // Update the product properties
                getProduct.Name = Entity.Name;
                getProduct.Price = Entity.Price;
                getProduct.Quantity = Entity.Quantity;
                context.Products.Update(getProduct);
                await context.SaveChangesAsync();
                return new Response { flag = true, message = $"Product : {getProduct.Name} updated successfully." };

            }
            catch (Exception ex)
            {
                // Handle exception and return a failure response
                // You can log the exception here if needed
                LogsExceptions.LogException(ex);

                return new Response { flag = false, message = "Error Occurred while Updating Product" }; //local error message
            }
        }

        public async Task<Response> DeleteAsync(Product Entity)
        {
            try
            {
                var product = await context.Products.FindAsync(Entity.Id);
                if (product == null)
                {
                    return new Response { flag = false, message = "Product not found." };
                }
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return new Response { flag = true, message = "Product deleted successfully." };

            }
            catch (Exception ex)
            {
                // Handle exception and return a failure response
                // You can log the exception here if needed
                LogsExceptions.LogException(ex);

                return new Response { flag = false, message = "Error Occurred while Deleting Product" }; //local error message
            }

        }

        public async Task<IEnumerable<Product>> GetAllAsync(Product Entity)
        {
            try
            {
                var products = await context.Products.AsNoTracking().ToListAsync();
                if (products == null)
                    throw new Exception("No products found");
                return products;

            }
            catch (Exception ex)
            {
                // Handle exception and return a failure response
                // You can log the exception here if needed
                LogsExceptions.LogException(ex);

                throw new Exception("Error Occurred while Retrieving Products"); //local error message
            }
        }
       

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var result = await context.Products.FindAsync(id);
                if (result == null)
                    throw new Exception("Product not found");
                return result;

            }
            catch (Exception ex)
            {
                // Handle exception and return a failure response
                // You can log the exception here if needed
                LogsExceptions.LogException(ex);
                throw new Exception("Error Occurred while Retrieving Product"); //local error message
            }


        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> Predicate)
        {
            try
            {
                var result = await context.Products.Where(Predicate).FirstOrDefaultAsync();
                if (result == null)
                    throw new Exception(
                        "Product not found");
                return result;
            }
            catch (Exception ex)
            {
                // Handle exception and return a failure response
                // You can log the exception here if needed
                LogsExceptions.LogException(ex);
                throw new Exception("Error Occurred while Retrieving Product"); //local error message
            }
        }

    }
}
