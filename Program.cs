
using AuthApi.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Adiciona o contexto do banco de dados com SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=users.db"));

// Adiciona os controllers (Web API)
builder.Services.AddControllers();

var app = builder.Build();

// Usa os endpoints de controller
app.MapControllers();

app.Run();
