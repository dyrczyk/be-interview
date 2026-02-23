using SimpleSearchApi.Models;

namespace SimpleSearchApi.Data;

public interface ISearchRepository
{
  Task<List<SearchItem>> SearchItemsAsync(string? query);
}