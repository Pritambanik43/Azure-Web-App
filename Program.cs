using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 👉 IMPORTANT: Paste your connection string here
string connectionString = "Server=tcp:sqltstsvr.database.windows.net,1433;Initial Catalog=free-sql-db-8744471;User ID=CloudSA76893420;Password=Pritam@1234;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

app.MapGet("/", () =>
{
    string result = "Connected successfully! 🎉\n";

    try
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            result += "Database connection OK ✅";
        }
    }
    catch (Exception ex)
    {
        result += "Error: " + ex.Message;
    }

    return result;
});

app.Run();
