# Simple Search API - Live Coding Challenge

Welcome to the Round 2 Live Coding Challenge! This is a **starter project** that you'll be enhancing during our interview.

## Quick Start

### Prerequisites
- .NET 8 SDK  
- Docker Desktop (for PostgreSQL)
- Internet access for AI tools

### Run the Application

```bash
dotnet run
```

The application automatically:
- Starts a PostgreSQL test container  
- Creates database schema and sample data
- Launches at **https://localhost:7154/swagger**

**Test it**: Open https://localhost:7154/swagger and try the API endpoints!

### Run Tests

```bash
dotnet test Tests/SimpleSearchApi.Tests/SimpleSearchApi.Tests.csproj
```

## Project Structure

```
Controllers/
├── SearchController.cs        # Search API endpoints
Data/
├── SearchRepository.cs        # Search data access
Models/
├── SearchItem.cs              # Data model
```

## Available Endpoints

**🔍 Search:** `GET /api/search` - Search by title  

## Notes

- **PostgreSQL** via Testcontainers (zero manual setup) 
- **Sample data** included (5 search items)
- Check **QUICK_TEST.md** for copy-paste test commands
