using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using SimpleSearchApi.Data;
using SimpleSearchApi.Models;
using Xunit;

namespace SimpleSearchApi.Tests.Data;

public sealed class SearchRepositoryTests : IAsyncLifetime
{
  private readonly DatabaseSetup _databaseSetup;
  private readonly SearchRepository _repository;

  public SearchRepositoryTests()
  {
    _databaseSetup = new DatabaseSetup(NullLogger<DatabaseSetup>.Instance);
    _repository = new SearchRepository(_databaseSetup);
  }

  public Task InitializeAsync()
  {
    return _databaseSetup.InitializeDatabaseAsync();
  }

  public async Task DisposeAsync()
  {
    await _databaseSetup.DisposeAsync();
  }

  [Fact]
  public async Task SearchItemsAsync_Filters_By_Title()
  {
    var results = await _repository.SearchItemsAsync("api");

    results.Should().NotBeEmpty();
    results.Should().OnlyContain(item =>
      item.Title.Contains("api", StringComparison.OrdinalIgnoreCase));
  }
}
