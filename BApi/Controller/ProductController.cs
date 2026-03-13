using BApi.DTOs;
using BApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BApi.Controller;

public static class ProductController
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("/products", async (ProductService productService) =>
        {
            var products = await productService.GetAllProductsAsync();
            return Results.Ok(products);
        }).WithName("GetAllProducts").WithOpenApi();

        app.MapGet("products/{id}", async (string id, ProductService productService) =>
        {
            var product = await productService.GetProductByIdAsync(id);
            return product != null ? Results.Ok(product) : Results.NotFound();
        }).WithName("GetProductById").WithOpenApi();

        app.MapPost("/products", async ([FromBody] ProductPostDto postDto, ProductService productService) =>
        {
            var product = await productService.AddProductAsync(postDto);
            return Results.Created($"/products/{product.Id}", product);
        }).WithName("AddProduct").WithOpenApi();

        app.MapPut("/products/{id}", async (string id, [FromBody] ProductUpdateDto updateDto, ProductService productService) =>
        {
            var product = await productService.UpdateProductAsync(id, updateDto);
            return product != null ? Results.Ok(product) : Results.NotFound();
        }).WithName("UpdateProduct").WithOpenApi();

        app.MapDelete("/products/{id}", async (string id, ProductService productService) =>
        {
            var deleted = await productService.DeleteProductAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        }).WithName("DeleteProduct").WithOpenApi();
    }

}