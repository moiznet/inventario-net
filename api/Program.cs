using Inventario.Api.Services ;
using Inventario.Api.Options;
using Scalar.AspNetCore;
using Inventario.Api.Errores; 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ── FASE 1: registrar servicios ──
builder.Services.AddControllers();  // conecta con los controllers
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// crear documentacio
builder.Services.AddOpenApi(o =>
    o.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddOptions<InventarioOptions>()                       // ← paréntesis VACÍOS
    .Bind(builder.Configuration.GetSection(InventarioOptions.Seccion)) // ← la sección va AQUÍ
    .ValidateDataAnnotations()
    .ValidateOnStart();          
    
builder.Services.AddSingleton<IProductoService, ProductoServices>(); // registra el servicio de productos como singleton    
 

var app = builder.Build();


// ── FASE 2: configurar el pipeline (middleware) ──

var logger = app.Logger;

app.UseExceptionHandler();          // 1. primero: atrapa errores de todo lo que sigue


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // epxone openapi para desarrollo
    app.UseSwaggerUI(o => o.SwaggerEndpoint("/openapi/v1.json", "Inventario API"));
}
app.Use(async (context, next) =>
{
    // ANTES: la petición va de entrada
    var inicio = DateTime.UtcNow;
 
    await next(context);     // pasa a la siguiente pieza

    // DESPUÉS: la respuesta va de salida
    var ms = (DateTime.UtcNow - inicio).TotalMilliseconds;
    logger.LogInformation("{Method} {Path} → {StatusCode}  ({TiempoMs:F0} ms)",context.Request.Method, context.Request.Path,context.Response.StatusCode,ms);
    
});
app.UseHttpsRedirection();   // redirige solicitudes http a https

app.UseAuthorization();     // revisa permisos se usa para jwt

app.MapControllers(); //conecta las rutas con os controllers

app.Run(); // ejecuta el servidor
