using System.ComponentModel.DataAnnotations;

namespace BApi.DTOs;

public class ProductPostDto
{
    [Required]
    public string Name {get; set;} = "";
    [Required]
    public int Price {get; set;}
    public string ImageUrl {get; set;} = string.Empty;
    public string Description {get; set;} = "";
}