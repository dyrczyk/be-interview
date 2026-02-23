using System.Data;
using Npgsql;
using Testcontainers.PostgreSql;

namespace SimpleSearchApi.Data;

public interface IDatabaseSetup
{
  Task InitializeDatabaseAsync();
  IDbConnection CreateConnection();
}

public class DatabaseSetup : IDatabaseSetup, IAsyncDisposable
{
  private readonly ILogger<DatabaseSetup> _logger;
  private readonly PostgreSqlContainer _container;
  private string? _connectionString;

  public DatabaseSetup(ILogger<DatabaseSetup> logger)
  {
    _logger = logger;

    // Create PostgreSQL test container
    _container = new PostgreSqlBuilder()
      .WithImage("postgres:15")
      .WithDatabase("SimpleSearchDb")
      .WithUsername("postgres")
      .WithPassword("password123")
      .WithCleanUp(true)
      .Build();
  }

  public IDbConnection CreateConnection()
  {
    if (string.IsNullOrEmpty(_connectionString))
    {
      throw new InvalidOperationException("Database not initialized. Call InitializeDatabaseAsync first.");
    }

    return new NpgsqlConnection(_connectionString);
  }

  public async Task InitializeDatabaseAsync()
  {
    _logger.LogInformation("Starting PostgreSQL test container...");

    // Start the container
    await _container.StartAsync();

    // Get connection string
    _connectionString = _container.GetConnectionString();

    _logger.LogInformation("PostgreSQL container started successfully");

    // Initialize schema and data
    await InitializeSchemaAsync();
  }

  private async Task InitializeSchemaAsync()
  {
    using var connection = new NpgsqlConnection(_connectionString);
    await connection.OpenAsync();

    // Create table
    var createTableSql = @"
      CREATE TABLE IF NOT EXISTS search_items (
        id SERIAL PRIMARY KEY,
        title VARCHAR(200) NOT NULL,
        description VARCHAR(1000),
        category VARCHAR(100) NOT NULL,
        tags VARCHAR(500)
      );
      ";

    using var command = new NpgsqlCommand(createTableSql, connection);
    await command.ExecuteNonQueryAsync();

    // Insert sample data (only if empty)
    var checkSql = @"SELECT COUNT(*) FROM search_items";
    using var checkCommand = new NpgsqlCommand(checkSql, connection);
    var count = (long)(await checkCommand.ExecuteScalarAsync() ?? 0);

    if (count == 0)
    {
      var insertSql = @"
        INSERT INTO search_items (title, description, category, tags) VALUES
        ('Getting Started with API Development', 'A comprehensive guide to building REST APIs with ASP.NET Core', 'Development', 'api,dotnet,tutorial'),
        ('API Database Design Best Practices', 'Learn how to design efficient and scalable database schemas', 'Database', 'database,design,postgresql'),
        ('API Data Access with Entity Framework', 'Object-relational mapping made easy with EF Core', 'Development', 'orm,entityframework,database'),
        ('API Query Optimization with SQL', 'Master complex queries, indexes, and performance optimization', 'Database', 'sql,performance,optimization'),
        ('API Microservices Architecture', 'Building scalable distributed systems with .NET', 'Architecture', 'microservices,dotnet,distributed');
        ";

      using var insertCommand = new NpgsqlCommand(insertSql, connection);
      await insertCommand.ExecuteNonQueryAsync();
    }

    _logger.LogInformation("PostgreSQL database initialized with sample data");
  }

  public async ValueTask DisposeAsync()
  {
    if (_container != null)
    {
      await _container.DisposeAsync();
      _logger.LogInformation("PostgreSQL test container disposed");
    }
  }
}