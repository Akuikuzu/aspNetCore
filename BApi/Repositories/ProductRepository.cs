using BApi.Data;
using BApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DbContextBApi _dbContext;

    public ProductRepository(DbContextBApi dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> AddAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Id))
        {
            product.Id = Guid.NewGuid().ToString();
        }

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        var existing = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);

        if (existing == null)
        {
            return false;
        }

        existing.Name = product.Name;
        existing.Price = product.Price;
        existing.ImageUrl = product.ImageUrl;
        existing.Description = product.Description;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var existing = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (existing == null)
        {
            return false;
        }

        _dbContext.Products.Remove(existing);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}