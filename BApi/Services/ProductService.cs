using BApi.DTOs;
using BApi.Models;
using BApi.Repositories;

namespace BApi.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductGetDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToGetDto).ToList();
    }

    public async Task<ProductGetDto?> GetProductByIdAsync(string id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product != null ? MapToGetDto(product) : null;
    }

    public async Task<ProductGetDto> AddProductAsync(ProductPostDto postDto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = postDto.Name,
            Price = postDto.Price,
            ImageUrl = postDto.ImageUrl,
            Description = postDto.Description
        };

        var createdProduct = await _productRepository.AddAsync(product);
        return MapToGetDto(createdProduct);
    }

    public async Task<ProductGetDto?> UpdateProductAsync(string id, ProductUpdateDto updateDto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return null;

        product.Name = updateDto.Name;
        product.Price = updateDto.Price;
        product.ImageUrl = updateDto.ImageUrl;
        product.Description = updateDto.Description;

        var updated = await _productRepository.UpdateAsync(product);
        return updated ? MapToGetDto(product) : null;
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        return await _productRepository.DeleteAsync(id);
    }

    private static ProductGetDto MapToGetDto(Product product)
    {
        return new ProductGetDto
        {
            Id = product.Id,
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Description = product.Description
        };
    }
}