using System.Text.Json.Serialization;

namespace Empresa.Models
{
	public class Departamento
	{
		public int DepartamentoId { get; set; }
		public string Nombre { get; set; }
		public string Descripcion { get; set; }
		[JsonIgnore]
		public virtual ICollection<EmpleadoDepartamento> EmpleadoDepartamento { get; set; }
	}
}
