using System.ComponentModel.DataAnnotations;
using Tasko.Web.Utility;

namespace Tasko.Web.Models;

public class ProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string ImageLocalPath { get; set; } = string.Empty;
    [Range(1, 100)]
    public int Count { get; set; } = 1;
    [MaxFileSize(1)]
    [AllowedExtensions([".jpg", ".png"])]
    public IFormFile? Image { get; set; }
}
