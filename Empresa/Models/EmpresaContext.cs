using Microsoft.EntityFrameworkCore;
using Empresa.Models;
using static Empresa.Models.Empleado;

namespace Empresa.Models
{
	public class EmpresaContext : DbContext
	{
		public EmpresaContext(DbContextOptions<EmpresaContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Genera seed data para la tabla Ciudad en una lista
			List<Ciudad> ciudadInit = new List<Ciudad>();
			ciudadInit.Add(new Ciudad { CiudadId = 1, Nombre = "Guadalajara" });
			ciudadInit.Add(new Ciudad { CiudadId = 2, Nombre = "Zapopan" });
			ciudadInit.Add(new Ciudad { CiudadId = 3, Nombre = "Tlaquepaque" });
			ciudadInit.Add(new Ciudad { CiudadId = 4, Nombre = "Tonala" });

			// Genera el modelo de la tabla Ciudad
			modelBuilder.Entity<Ciudad>(ciudad =>
			{
				ciudad.HasKey(c => c.CiudadId);
				ciudad.Property(c => c.Nombre).HasMaxLength(50).IsRequired();
				ciudad.HasData(ciudadInit);
			});

			// Genera el seed data para la tabla Departamento en una lista
			List<Departamento> departamentoInit = new List<Departamento>();
			departamentoInit.Add(new Departamento { DepartamentoId = 1, Nombre = "Recursos Humanos", Descripcion = "Departamento de Recursos Humanos" });
			departamentoInit.Add(new Departamento { DepartamentoId = 2, Nombre = "Ventas", Descripcion = "Departamento de Ventas" });
			departamentoInit.Add(new Departamento { DepartamentoId = 3, Nombre = "Compras", Descripcion = "Departamento de Compras" });

			// Genera el modelo de la tabla Departamento
			modelBuilder.Entity<Departamento>(departamento =>
			{
				departamento.HasKey(d => d.DepartamentoId);
				departamento.Property(d => d.Nombre).HasMaxLength(50).IsRequired();
				departamento.Property(d => d.Descripcion).HasMaxLength(100).IsRequired();
				departamento.HasData(departamentoInit);
			});

			// Genera el seed data para la tabla Empleado en una lista
			List<Empleado> empleadoInit = new List<Empleado>();
			empleadoInit.Add(new Empleado { EmpleadoId = 1, Nombre = "Juan Perez", FechaIngreso = DateTime.Now, Puesto = "Gerente", Sueldo = 10000, CiudadId = 1 });
			empleadoInit.Add(new Empleado { EmpleadoId = 2, Nombre = "Maria Lopez", FechaIngreso = DateTime.Now, Puesto = "Vendedor", Sueldo = 8000, CiudadId = 2 });
			empleadoInit.Add(new Empleado { EmpleadoId = 3, Nombre = "Pedro Ramirez", FechaIngreso = DateTime.Now, Puesto = "Comprador", Sueldo = 9000, CiudadId = 3 });

			// Genera el modelo de la tabla Empleado
			modelBuilder.Entity<Empleado>(empleado =>
			{
				empleado.HasKey(e => e.EmpleadoId);
				empleado.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
				empleado.Property(e => e.FechaIngreso).IsRequired();
				empleado.Property(e => e.Puesto).HasMaxLength(50).IsRequired();
				empleado.Property(e => e.Sueldo).IsRequired();
				empleado.HasOne(e => e.Ciudad).WithMany(c => c.Empleado).HasForeignKey(e => e.CiudadId);
				
				empleado.HasData(empleadoInit);
			});

			// Genera el seed data para la tabla EmpleadoDepartamento en una lista
			List<EmpleadoDepartamento> empleadoDepartamentoInit = new List<EmpleadoDepartamento>();
			empleadoDepartamentoInit.Add(new EmpleadoDepartamento { EmpleadoId = 1, DepartamentoId = 1 });
			empleadoDepartamentoInit.Add(new EmpleadoDepartamento { EmpleadoId = 2, DepartamentoId = 2 });
			empleadoDepartamentoInit.Add(new EmpleadoDepartamento { EmpleadoId = 3, DepartamentoId = 3 });

			// Genera el modelo de la tabla EmpleadoDepartamento
			modelBuilder.Entity<EmpleadoDepartamento>(empleadoDepartamento =>
			{
				empleadoDepartamento.HasKey(ed => new { ed.EmpleadoId, ed.DepartamentoId });
				empleadoDepartamento.HasOne(ed => ed.Departamento).WithMany(d => d.EmpleadoDepartamento).HasForeignKey(ed => ed.DepartamentoId);
				empleadoDepartamento.HasOne(ed => ed.Empleado).WithMany(e => e.EmpleadoDepartamento).HasForeignKey(ed => ed.EmpleadoId);

				empleadoDepartamento.HasData(empleadoDepartamentoInit);
			});
		}
		
		public DbSet<Ciudad> Ciudad { get; set; }
		public DbSet<Departamento> Departamento { get; set; }
		public DbSet<Empleado> Empleado { get; set; }
		public DbSet<EmpleadoDepartamento> EmpleadoDepartamento { get; set; }
	}
}
