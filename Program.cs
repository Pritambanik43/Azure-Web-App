using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.SqlClient;   // ✅ FIXED
using System;                     // ✅ FIXED

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 🔥 Your connection string (PASTE YOURS HERE)
string connectionString = "Server=tcp:sqltstsvr.database.windows.net,1433;Initial Catalog=free-sql-db-8744471;Persist Security Info=False;User ID=CloudSA76893420;Password=Pritam@0926;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

app.MapGet("/", () =>
{
    try
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            return "✅ Connected to Azure SQL Database!";
        }
    }
    catch (Exception ex)
    {
        return "❌ DB Connection Failed: " + ex.Message;
    }
});

app.Run();
