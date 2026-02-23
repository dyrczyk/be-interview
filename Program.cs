using SimpleSearchApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register database setup as singleton
builder.Services.AddSingleton<IDatabaseSetup, DatabaseSetup>();

// Register repositories and services
builder.Services.AddScoped<ISearchRepository, SearchRepository>();

var app = builder.Build();

// Initialize database before starting the application
using (var scope = app.Services.CreateScope())
{
  var databaseSetup = scope.ServiceProvider.GetRequiredService<IDatabaseSetup>();
  await databaseSetup.InitializeDatabaseAsync();
  Console.WriteLine("✅ PostgreSQL container started and database initialized");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Ensure proper database container cleanup on shutdown
app.Lifetime.ApplicationStopping.Register(() =>
{
  var databaseSetup = app.Services.GetService<IDatabaseSetup>();
  if (databaseSetup is IAsyncDisposable asyncDisposable)
  {
    _ = Task.Run(async () => await asyncDisposable.DisposeAsync());
  }
});

app.Run();