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

app.MapPost("/api/empleados/add", async ([FromServices] EmpresaContext dbContext, [FromBody] EmpleadoDTO empleadoDTO) =>
{
	var empleado = new Empleado
	{
		Nombre = empleadoDTO.Nombre,
		Puesto = empleadoDTO.Puesto,
		Sueldo = empleadoDTO.Sueldo,
		CiudadId = empleadoDTO.CiudadId
	};

	await dbContext.Empleado.AddAsync(empleado);
	await dbContext.SaveChangesAsync();

	var empleadoDepartamento = new EmpleadoDepartamento
	{
		EmpleadoId = empleado.EmpleadoId,
		DepartamentoId = empleadoDTO.DepartamentoId
	};

	await dbContext.EmpleadoDepartamento.AddAsync(empleadoDepartamento);
	await dbContext.SaveChangesAsync();

	return Results.Ok($"El empleado {empleado.Nombre} fue agregado correctamente");
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

app.MapGet("/api/departamentos/all", async([FromServices] EmpresaContext dbContext) =>
{
	var departamentos = await dbContext.Departamento.ToListAsync();

	return Results.Ok(departamentos);
});

app.MapGet("/api/departamentos/{id}", async([FromServices] EmpresaContext dbContext, int id) =>
{
	var departamento = await dbContext.Departamento.FindAsync(id);

	if (departamento != null)
	{
		return Results.Ok(departamento);
	}

	return Results.NotFound($"No se encontró el departamento con el id {id}");
});

app.MapPost("/api/departamentos/add", async([FromServices] EmpresaContext dbContext, [FromBody] Departamento departamento) =>
{
	await dbContext.Departamento.AddAsync(departamento);
	await dbContext.SaveChangesAsync();

	return Results.Ok($"El departamento {departamento.Nombre} fue agregado correctamente");
});

app.MapPut("/api/departamentos/edit/{id}", async([FromServices] EmpresaContext dbContext, [FromBody] Departamento departamento, [FromRoute] int id) =>
{
	var departamentoEdit = await dbContext.Departamento.FindAsync(id);

	if (departamentoEdit != null)
	{
		departamentoEdit.Nombre = departamento.Nombre;
		departamentoEdit.Descripcion = departamento.Descripcion;

		await dbContext.SaveChangesAsync();

		return Results.Ok($"El departamento {departamento.Nombre} fue modificado correctamente");
	}

	return Results.NotFound($"No se encontró el departamento con el id {id}");
});

app.MapDelete("/api/departamentos/del/{id}", async([FromServices] EmpresaContext dbContext, int id) =>
{
	var departamento = await dbContext.Departamento.FindAsync(id);

	if (departamento != null)
	{
		dbContext.Departamento.Remove(departamento);

		await dbContext.SaveChangesAsync();

		return Results.Ok($"El departamento {departamento.Nombre} fue eliminado correctamente");
	}

	return Results.NotFound($"No se encontró el departamento con el id {id}");
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
