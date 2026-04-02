using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic; // ADD THIS
using Microsoft.Data.SqlClient;   // ✅ FIXED
using System;                     // ✅ FIXED

var builder = WebApplication.CreateBuilder(args);

// ✅ ADD THIS LINE HERE (after builder, before app)
string connectionString = "Server=tcp:sqltstsvr.database.windows.net,1433;Initial Catalog=free-sql-db-8744471;Persist Security Info=False;User ID=CloudSA76893420;Password=Pritam@0926;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
var app = builder.Build();

app.MapGet("/", () =>
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        try
{
    conn.Open();
}
catch (Exception ex)
{
    return Results.Content("DB ERROR: " + ex.Message);
}

        SqlCommand cmd = new SqlCommand("SELECT StudentID, Name, Course FROM Students", conn);
        SqlDataReader reader = cmd.ExecuteReader();

        string html = @"
<html>
<head>
    <title> Welcome To Student Dashboard</title>

    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css' rel='stylesheet'>

</head>
<body class='bg-light'>

<div class='container mt-5'>

<h1 class='mb-4'>🚀 Welcome To Student Dashboard</h1>

<form method='post' action='/add' class='row g-2 mb-4'>
    <div class='col-md-3'>
        <input type='text' name='name' class='form-control' placeholder='Name' required />
    </div>
    <div class='col-md-2'>
        <input type='number' name='age' class='form-control' placeholder='Age' required />
    </div>
    <div class='col-md-3'>
        <input type='text' name='course' class='form-control' placeholder='Course' required />
    </div>
    <div class='col-md-2'>
        <button type='submit' class='btn btn-success'>Add</button>
    </div>
</form>

<input type='text' id='search' class='form-control mb-3' placeholder='Search...' onkeyup='filterTable()' />

<table class='table table-bordered table-striped' id='studentTable'>
<thead class='table-dark'>
<tr>
    <th>ID</th>
    <th>Name</th>
    <th>Course</th>
    <th>Actions</th>
</tr>
</thead>
<tbody>";
        
        while (reader.Read())
{
    html += $@"
<tr>
    <td>{reader["StudentID"]}</td>
    <td>{reader["Name"]}</td>
    <td>{reader["Course"]}</td>
    <td>
        <a href='/delete/{reader["StudentID"]}' 
   class='btn btn-danger btn-sm'
   onclick='return confirm(""Are you sure?"")'>
   Delete
</a>
    </td>
</tr>";
}

        html += @"
</tbody>
</table>
</div>

<script>
function filterTable() {
    let input = document.getElementById('search').value.toLowerCase();
    let rows = document.querySelectorAll('#studentTable tbody tr');

    rows.forEach(row => {
        let text = row.innerText.toLowerCase();
        row.style.display = text.includes(input) ? '' : 'none';
    });
}
</script>

</body>
</html>";

        return Results.Content(html, "text/html; charset=utf-8");
    }
});

app.MapPost("/add", async (HttpRequest request) =>
{
    var form = await request.ReadFormAsync();

    string name = form["name"];
    int age = int.Parse(form["age"]);
    string course = form["course"];

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();

        SqlCommand cmd = new SqlCommand(
            "INSERT INTO Students (Name, Age, Course) VALUES (@name, @age, @course)", conn);

        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@age", age);
        cmd.Parameters.AddWithValue("@course", course);

        cmd.ExecuteNonQuery();
    }

    return Results.Redirect("/");
});
app.MapGet("/delete/{id}", (int id) =>
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();

        SqlCommand cmd = new SqlCommand(
            "DELETE FROM Students WHERE StudentID=@id", conn);

        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();
    }

    return Results.Redirect("/");
});

app.Run();
