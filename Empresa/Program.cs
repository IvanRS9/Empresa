using Empresa.Models;
using Empresa;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSqlServer<EmpresaContext>(builder.Configuration.GetConnectionString("dbEmpresa"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconexion", async ([FromServices] EmpresaContext dbContext) =>
{
	await dbContext.Database.EnsureCreatedAsync();
	return "Base de datos creada";
});

app.MapGet("/api/empleados/all", async ([FromServices] EmpresaContext dbContext) =>
{
	var empleados = await dbContext.Empleado.Include(e => e.Ciudad).ToListAsync();

	return Results.Ok(dbContext.Empleado);
});

app.MapPost("/api/empleados/add", async([FromServices] EmpresaContext dbContext, [FromBody] Empleado empleado) =>
{
	var empleados = dbContext.Empleado.Count();
	empleado.FechaIngreso = DateTime.Now;
	
	await dbContext.AddAsync(empleado);
	await dbContext.SaveChangesAsync();

	return Results.Ok($"El empleado {empleado.Nombre} fue creado correctamente");
});

app.MapPut("/api/empleados/edit/{id}", async ([FromServices] EmpresaContext dbContext, [FromBody] Empleado empleado, [FromRoute] int id) =>
{
	var empleadoEdit = await dbContext.Empleado.FindAsync(id);

	if (empleadoEdit != null)
	{
		empleadoEdit.Nombre = empleado.Nombre;
		empleadoEdit.Puesto = empleado.Puesto;
		empleadoEdit.Sueldo = empleado.Sueldo;
		empleadoEdit.CiudadId = empleado.CiudadId;

		await dbContext.SaveChangesAsync();

		return Results.Ok($"El empleado {empleado.Nombre} fue modificado correctamente");
	}

	return Results.NotFound($"No se encontró el empleado con el id {id}");
});

app.MapDelete("/api/empleados/del/{id}", async([FromServices] EmpresaContext dbContext, int id) =>
{
	var empleado = await dbContext.Empleado.FindAsync(id);

	if (empleado != null)
	{
		dbContext.Empleado.Remove(empleado);

		await dbContext.SaveChangesAsync();

		return Results.Ok($"El empleado {empleado.Nombre} fue eliminado correctamente");
	}

	return Results.NotFound($"No se encontró el empleado con el id {id}");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
