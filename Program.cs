using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 🔴 ADD YOUR CONNECTION STRING HERE
string connectionString = "Server=tcp:sqltstsvr.database.windows.net,1433;Initial Catalog=free-sql-db-8744471;User ID=CloudSA76893420;Password=Pritam@1234;Encrypt=True;";

app.MapGet("/", async () =>
{
    var html = new StringBuilder();
    html.Append("<h1>Employee List</h1><table border='1'><tr><th>ID</th><th>Name</th><th>Role</th></tr>");

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        await conn.OpenAsync();
        var cmd = new SqlCommand("SELECT * FROM Employees", conn);
        var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            html.Append($"<tr><td>{reader["Id"]}</td><td>{reader["Name"]}</td><td>{reader["Role"]}</td></tr>");
        }
    }

    html.Append("</table>");
    return html.ToString();
});

app.Run();
