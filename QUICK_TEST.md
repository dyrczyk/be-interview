# Quick Test Commands - Simple API Testing

## 🚀 Easiest Way - Swagger UI

1. **Run application**: `dotnet run`
2. **Open browser**: https://localhost:7154/swagger
3. **Click on endpoint** → **Try it out** → **Execute**

**💡 Tip**: You can test everything directly in the browser without command line!

---

## 📋 Copy-Paste Commands

### PowerShell (Windows)
```powershell
# Basic search
Invoke-RestMethod "https://localhost:7154/api/search" | ConvertTo-Json

# Search with parameters (should return multiple results)
Invoke-RestMethod "https://localhost:7154/api/search?query=api" | ConvertTo-Json
```
