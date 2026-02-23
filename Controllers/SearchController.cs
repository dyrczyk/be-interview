using Microsoft.AspNetCore.Mvc;
using SimpleSearchApi.Data;
using SimpleSearchApi.Models;

namespace SimpleSearchApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
  private readonly ISearchRepository _searchRepository;

  public SearchController(ISearchRepository searchRepository)
  {
    _searchRepository = searchRepository;
  }

  /// <summary>
  /// Search for items by title
  /// </summary>
  [HttpGet]
  public async Task<ActionResult<List<SearchItem>>> Search([FromQuery] string? query)
  {
    var result = await _searchRepository.SearchItemsAsync(query);
    return Ok(result);
  }
}