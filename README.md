# MySystem (.NET 8)

مشروع تدريبي متكامل  run & test بالخطوات.

Endpoints:
- GET /hello
- GET /health
- POST /auth/token (body: {"userName":"admin","password":"password"})
- GET /users (requires Bearer token)
- POST /users (requires Bearer token)

شغل:
dotnet restore
dotnet build
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
