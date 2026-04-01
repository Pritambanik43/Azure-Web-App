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
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM Students", conn);
        SqlDataReader reader = cmd.ExecuteReader();

        string html = @"
        <html>
        <head>
            <title>User Dashboard</title>
            <style>
                body { font-family: Arial; padding:20px; background:#f4f4f4; }
                table { border-collapse: collapse; width: 50%; background:white; }
                th, td { border:1px solid #ccc; padding:10px; text-align:left; }
                th { background:#eee; }
                input, button { padding:8px; margin-top:10px; }
            </style>
        </head>
        <body>
            <h1>🚀 User Dashboard</h1>

            <form method='post' action='/add'>
                <input type='text' name='name' placeholder='Enter name' required />
                <button type='submit'>Add User</button>
            </form>

            <h2>Users</h2>
            <table>
                <tr><th>ID</th><th>Name</th></tr>";
        
        while (reader.Read())
        {
            html += $"<tr><td>{reader["Id"]}</td><td>{reader["Name"]}</td></tr>";
        }

        html += @"
            </table>
        </body>
        </html>";

        return Results.Content(html, "text/html");
    }
});

app.Run();
