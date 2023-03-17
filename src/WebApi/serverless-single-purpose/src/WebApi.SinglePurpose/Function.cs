using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using AWS.Lambda.Powertools.Logging;
using AWS.Lambda.Powertools.Tracing;
using Microsoft.EntityFrameworkCore;
using WebApi.Shared.Products;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace WebApi.SinglePurpose
{
    public class Function
    {
        private readonly DataContext _context;

        public Function(DataContext context)
        {
            _context = context;
        }
        
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Get, "/")]
        public async Task<List<Product>> ListProducts()
        {
            Logger.LogDebug("Received request to list products");

            var products = await this._context.Products.ToListAsync();
            
            Tracing.AddAnnotation("product.count", products.Count);

            return products;
        }
        
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Post, "/")]
        public async Task<Product> CreateProduct([FromBody] ProductDTO product)
        {
            Logger.LogDebug("Received request to create product");
            
            var newProduct = new Product()
            {
                Name = product.Name,
                Price = product.Price
            };
            
            Logger.LogDebug("Committing to database");
        
            await this._context.Products.AddAsync(newProduct);
            await this._context.SaveChangesAsync();

            Logger.LogDebug("Saved");

            return newProduct;
        }
        
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Get, "/{id}")]
        public async Task<Product> GetProduct(int id)
        {
            Logger.LogDebug($"Attempting to get product with id {id}");
            
            var existingProduct = await this._context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                Logger.LogWarning("Product not found");
                
                return null;
            }
            
            return existingProduct;
        }
        
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Put, "/{id}")]
        public async Task<Product> UpdateProduct(int id, [FromBody] ProductDTO product)
        {
            Logger.LogDebug($"Attempting to update product with id {id}");
            
            var existingProduct = await this._context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                Logger.LogWarning("Product not found");
                
                return null;
            }
            
            Logger.LogDebug("Product found, updating");

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;

            await this._context.SaveChangesAsync();
            
            return existingProduct;
        }
        
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Delete, "/{id}")]
        public async Task<Product> DeleteProduct(int id)
        {
            Logger.LogDebug($"Attempting to delete product with id {id}");
            
            var existingProduct = await this._context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                Logger.LogWarning("Product not found");
                
                return null;
            }
            
            Logger.LogDebug("Deleting product");

            this._context.Products.Remove(existingProduct);

            await this._context.SaveChangesAsync();
            
            return existingProduct;
        }
        
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Get, "/tools/migrate")]
        public async Task<string> ApplyDbMigrations()
        {
            await this._context.Database.MigrateAsync();

            return "OK";
        }
    }
}
