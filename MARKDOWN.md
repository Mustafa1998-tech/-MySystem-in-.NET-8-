# .NET 8 Project Commands Reference

## 1. Project Setup Commands

# .NET 8 Project Commands Reference 

1. Project Setup Commands
# Create New Web API Project
dotnet new webapi -n MySystem


شرح: ينشئ مشروع Web API جديد باسم MySystem.

# Install Required NuGet Packages
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.File
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.10
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.8
dotnet add package MiniProfiler.AspNetCore.Mvc
dotnet add package MiniProfiler.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.10


وظيفة كل حزمة:

Serilog: تسجيل نشاط التطبيق (logging)

Entity Framework Core: التعامل مع قاعدة بيانات SQL Server

JWT Bearer: المصادقة باستخدام JWT

MiniProfiler: مراقبة أداء التطبيق

# Install Global Tools
dotnet tool install --global devskim
dotnet tool install --global dotnet-outdated-tool
dotnet tool install --global dotnet-ef --version 8.0.10


وظيفة:

devskim: فحص الثغرات الأمنية (CVEs) في الكود

dotnet-outdated-tool: التحقق من الحزم القديمة

dotnet-ef: أوامر Entity Framework

Add Dotnet Tools to PATH (PowerShell)
[Environment]::SetEnvironmentVariable("PATH", [Environment]::GetEnvironmentVariable("PATH") + ";C:\Users\ahmed\.dotnet\tools", "User")


وظيفة: تشغيل الأدوات العالمية من أي مكان.

2. # Database Commands
 # Create Initial Migration
dotnet ef migrations add InitialCreate


شرح: إنشاء Migration أولي لإنشاء قاعدة البيانات والجداول.

# Apply Migrations to Database
dotnet ef database update


شرح: تطبيق جميع الـ migrations على قاعدة البيانات.

# Remove Last Migration
dotnet ef migrations remove


شرح: حذف آخر migration قبل تطبيقه.

# Database Connection String (appsettings.json)
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MySystemDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}


شرح: إعداد الاتصال بقاعدة بيانات SQL Server مع تجاوز مشاكل SSL في التطوير.

3. # Application Commands
# Run the Application
dotnet run


وظيفة: تشغيل التطبيق في وضع التطوير (http://localhost:5002
).

# Hot Reload
dotnet watch run


وظيفة: إعادة تشغيل التطبيق تلقائياً عند تعديل الكود.

# Build the Application
dotnet build


وظيفة: بناء المشروع والتحقق من الأخطاء.

# Restore Packages
dotnet restore


وظيفة: استعادة الحزم المحددة في ملف المشروع.

# Publish for Production
dotnet publish -c Release -o ./publish


وظيفة: إنشاء نسخة جاهزة للنشر (release).

4. # Security & Vulnerability Commands
Scan for Security Vulnerabilities
devskim analyze .


وظيفة: تحليل الكود بحثاً عن ثغرات أمنية.

# Check Vulnerable Packages
dotnet list package --vulnerable


وظيفة: التحقق من الحزم التي تحتوي على ثغرات معروفة (CVEs).

# Check Outdated Packages
dotnet outdated


وظيفة: عرض الحزم القديمة التي تحتاج تحديث.

5. # Environment Commands
# Set Development Environment

# PowerShell:

$env:ASPNETCORE_ENVIRONMENT="Development"


Linux/Mac:

export ASPNETCORE_ENVIRONMENT=Development


وظيفة: تحديد البيئة لتغيير إعدادات التطبيق.

6. # API Testing Commands
Test Health Endpoint
curl http://localhost:5002/health

Test Hello Endpoint
curl http://localhost:5002/hello

Test Login Endpoint (PowerShell)
Invoke-WebRequest -Uri "http://localhost:5002/auth/login?username=admin&password=password" -Method POST

Test Protected Users Endpoint
$headers = @{Authorization="Bearer YOUR_JWT_TOKEN"}
Invoke-WebRequest -Uri "http://localhost:5002/api/users" -Headers $headers -Method GET

7. Monitoring & Logging Commands
Access MiniProfiler Dashboard
http://localhost:5002/profiler/results-index

Check Application Logs
Get-Content "logs/log.txt"

8. CI/CD Commands
GitHub Actions Workflow
# يتم تشغيله تلقائياً عند الدفع إلى GitHub

9. Cleanup Commands
Stop Application
Stop-Process -Name "MySystem" -Force

Clean Build Artifacts
dotnet clean

🔹 ملخص سير العمل

إنشاء المشروع وتثبيت الحزم والأدوات العالمية

إعداد قاعدة البيانات وإنشاء migrations

تشغيل التطبيق واختبار API

مراقبة الأداء والتسجيل

فحص الحزم والكود بحثاً عن ثغرات أمنية

النشر والإعدادات للبيئات المختلفة

تنظيف الملفات المؤقتة عند الحاجة