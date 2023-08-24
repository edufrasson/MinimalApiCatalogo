using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Variável de conexão
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Acesso ao banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

/* Definição dos EndPoints */
app.MapGet("/", () => "Catálogo de Produtos - 2023");

// Tabela Categoria

app.MapPost("/categorias", async (Categoria c, AppDbContext db) =>
{
    db.Categorias.Add(c);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{c.Id}", c);
});

app.MapGet("/categorias", async(AppDbContext db) => await db.Categorias.ToListAsync());

app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Categorias.FindAsync(id)
        is Categoria c ? Results.Ok(c) : Results.NotFound();
});

app.MapPut("/categorias/{id:int}", async (int id, Categoria c, AppDbContext db) =>
{
    if(id != c.Id)
    {
        return Results.BadRequest();
    }

    var categoriaDB = await db.Categorias.FindAsync(id);

    if (categoriaDB is null) return Results.NotFound();

    categoriaDB.Nome = c.Nome;
    categoriaDB.Descricao = c.Descricao;

    await db.SaveChangesAsync();
    return Results.Ok(categoriaDB);
});

app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    var categoria = await db.Categorias.FindAsync(id);
    if (categoria is null)
        return Results.NotFound();

    db.Categorias.Remove(categoria);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.Run();

