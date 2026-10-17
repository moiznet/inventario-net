using Inventario.Api.Services ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ── FASE 1: registrar servicios ──
builder.Services.AddControllers();  // conecta con los controllers
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();      // crear documentacio

builder.Services.AddSingleton<IProductoService, ProductoServices>(); // registra el servicio de productos como singleton    

var app = builder.Build();
// ── FASE 2: configurar el pipeline (middleware) ──
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // epxone openapi para desarrollo
}

app.UseHttpsRedirection();   // redirige solicitudes http a https

app.UseAuthorization();     // revisa permisos se usa para jwt

app.MapControllers(); //conecta las rutas con os controllers

app.Run(); // ejecuta el servidor
