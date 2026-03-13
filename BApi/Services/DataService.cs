using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using BApi.Models;

namespace BApi.Services;

public class DataService
{
    private readonly string _dataFilePath;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<DataService> _logger;

    public DataService(ILogger<DataService> logger)
    {
        _logger = logger;
        var dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }

        _dataFilePath = Path.Combine(dataDirectory, "products.json");

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }
        public async Task<List<Product>> LoadProductsAsync()
    {
        if (!File.Exists(_dataFilePath))
        {
            return new List<Product>();
        }

        try
        {
            var json = await File.ReadAllTextAsync(_dataFilePath);

            if (String.IsNullOrWhiteSpace(json))
            {
                return new List<Product>();
            }

            var products = JsonSerializer.Deserialize<List<Product>>(json, _jsonOptions);
            return products ?? new List<Product>();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Fejl ved læsning af dataen fra {Path}", _dataFilePath);
            return new List<Product>();
        }
    }

    public async Task SaveProductsAsync(List<Product> products)
    {
        try
        {
            var json = JsonSerializer.Serialize(products, _jsonOptions);
            await File.WriteAllTextAsync(_dataFilePath, json);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved gemning af dataen i {Path}", _dataFilePath);
            throw new Exception("Kunne ikke gemme data", ex);
        }
    }
}