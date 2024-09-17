using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ImovelContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Imoveis API", Version = "v1" });
});



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Imoveis API v1"));
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
});


app.MapGet("/imoveis", async (ImovelContext db) =>
{
    var imoveis = await db.Imoveis.ToListAsync();
    return Results.Ok(imoveis);
})
.WithName("GetImoveis")
.WithOpenApi();


app.MapGet("/imoveis/{id}", async (int id, ImovelContext db) =>
    await db.Imoveis.FindAsync(id)
        is Imovel imovel
            ? Results.Ok(imovel)
            : Results.NotFound())
.WithName("GetImovel")
.WithOpenApi();


app.MapPost("/imoveis", async (Imovel imovel, ImovelContext db) =>
{
    db.Imoveis.Add(imovel);
    await db.SaveChangesAsync();
    return Results.Created($"/imoveis/{imovel.Id}", imovel);
})
.WithName("CreateImovel")
.WithOpenApi();


app.MapPut("/imoveis/{id}", async (int id, Imovel inputImovel, ImovelContext db) =>
{
    var imovel = await db.Imoveis.FindAsync(id);

    if (imovel is null) return Results.NotFound();

    imovel.Descricao = inputImovel.Descricao;
    imovel.DataCompra = inputImovel.DataCompra;
    imovel.Endereco = inputImovel.Endereco;

    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("UpdateImovel")
.WithOpenApi();


app.MapDelete("/imoveis/{id}", async (int id, ImovelContext db) =>
{
    if (await db.Imoveis.FindAsync(id) is Imovel imovel)
    {
        db.Imoveis.Remove(imovel);
        await db.SaveChangesAsync();
        return Results.Ok(imovel);
    }

    return Results.NotFound();
})
.WithName("DeleteImovel")
.WithOpenApi();


app.MapPost("/imoveis/{imovelId}/comodos", async (int imovelId, Comodo comodo, ImovelContext db) =>
{
    var imovel = await db.Imoveis.FindAsync(imovelId);
    if (imovel == null) return Results.NotFound("Imóvel não encontrado.");

    comodo.ImovelId = imovelId;
    db.Comodos.Add(comodo);
    await db.SaveChangesAsync();

    return Results.Created($"/imoveis/{imovelId}/comodos/{comodo.Id}", comodo);
})
.WithName("AddComodo")
.WithOpenApi();


app.MapGet("/imoveis/{imovelId}/comodos", async (int imovelId, ImovelContext db) =>
{
    var comodos = await db.Comodos.Where(c => c.ImovelId == imovelId).ToListAsync();
    return Results.Ok(comodos); 
})
.WithName("GetComodos")
.WithOpenApi();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ImovelContext>();
    context.Database.EnsureCreated();
}


app.Run();