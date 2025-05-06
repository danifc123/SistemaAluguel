using Microsoft.EntityFrameworkCore;
using SistemaAluguel.Data;
using SistemaAluguel.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapImovelEndpoints();
app.MapInquilinoEndpoint();
app.MapContratoEndpoints();
app.MapPagamentosEndpoint();

app.Run();