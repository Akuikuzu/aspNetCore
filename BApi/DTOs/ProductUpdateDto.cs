using System.ComponentModel.DataAnnotations;

namespace BApi.DTOs;

public class ProductUpdateDto
{
    [Required]
    public string Name { get; set; } = "";
    [Required]
    public int Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = "";
}