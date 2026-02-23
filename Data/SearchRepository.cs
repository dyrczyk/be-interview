using Dapper;
using SimpleSearchApi.Models;

namespace SimpleSearchApi.Data;

public class SearchRepository : ISearchRepository
{
  private readonly IDatabaseSetup _databaseSetup;

  public SearchRepository(IDatabaseSetup databaseSetup)
  {
    _databaseSetup = databaseSetup;
  }

  public async Task<List<SearchItem>> SearchItemsAsync(string? query)
  {
    using var connection = _databaseSetup.CreateConnection();

    var hasQuery = !string.IsNullOrWhiteSpace(query);
    var itemsQuery = $@"
      SELECT * FROM search_items
      {(hasQuery ? "WHERE title ILIKE @Query" : string.Empty)}
      ";

    var parameters = hasQuery ? new { Query = $"%{query}%" } : null;
    var items = await connection.QueryAsync<SearchItem>(itemsQuery, parameters);
    return items.ToList();
  }
}