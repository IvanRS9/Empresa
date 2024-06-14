using System.Text.Json.Serialization;

namespace Empresa.Models
{
	public class EmpleadoDepartamento
	{
		public int EmpleadoId { get; set; }
		public int DepartamentoId { get; set; }

		public Empleado Empleado { get; set; }
		public Departamento Departamento { get; set; }
	}
}
