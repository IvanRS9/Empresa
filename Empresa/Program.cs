using Empresa.Models;
using Empresa;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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

app.MapGet("/api/empleados", async ([FromServices] EmpresaContext dbContext) =>
{
	var empleados = await dbContext.Empleado.Include(e => e.Ciudad).ToListAsync();
	return Results.Ok(dbContext.Empleado);
});

// El método deberá de permitir insertar todos los datos de un nuevo Empleado, incluyendo los id de Ciudad y Departamento
app.MapPost("/api/empleados", async([FromServices] EmpresaContext dbContext, [FromBody] Empleado empleado) =>
{
	empleado.FechaIngreso = DateTime.Now;
	await dbContext.Empleado.AddAsync(empleado);
	await dbContext.SaveChangesAsync();

	return Results.Ok($"El empleado {empleado.Nombre} fue creado correctamente");
});
//app.MapPost("/api/empleados", async ([FromServices] EmpresaContext dbContext, [FromBody] Empleado empleado, EmpleadoDepartamento empDep) =>
//{
//	await dbContext.Empleado.AddAsync(empleado);
//	await dbContext.EmpleadoDepartamento.AddAsync(empDep);
//	await dbContext.SaveChangesAsync();

//	return Results.Ok($"El empleado {empleado.Nombre} fue creado correctamente");
//});

// Ruta por metodo put para Modificar todos los datos de un Empleado específico (solo los datos del Empleado)
app.MapPut("/api/empleados/{id}", async ([FromServices] EmpresaContext dbContext, int id, [FromBody] Empleado empleado) =>
{
	var empleadoModificado = await dbContext.Empleado.FindAsync(id);
	if (empleadoModificado != null)
	{
		empleadoModificado.Nombre = empleado.Nombre;
		empleadoModificado.FechaIngreso = empleado.FechaIngreso;
		empleadoModificado.Puesto = empleado.Puesto;
		empleadoModificado.Sueldo = empleado.Sueldo;
		await dbContext.SaveChangesAsync();
		return Results.Ok($"El empleado {empleado.Nombre} fue modificado correctamente");
	}

	return Results.NotFound($"No se encontró el empleado con el id {id}");
});

// Ruta por metodo delete para Eliminar un Empleado específico
app.MapDelete("/api/empleados/{id}", async([FromServices] EmpresaContext dbContext, int id) =>
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
