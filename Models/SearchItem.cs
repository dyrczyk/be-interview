using System.ComponentModel.DataAnnotations;

namespace SimpleSearchApi.Models;

public class SearchItem
{
  public int Id { get; set; }

  [Required]
  [MaxLength(200)]
  public string Title { get; set; } = string.Empty;

  [MaxLength(1000)]
  public string? Description { get; set; }

  [MaxLength(100)]
  public string Category { get; set; } = "General";

  [MaxLength(500)]
  public string? Tags { get; set; }
}