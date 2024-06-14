using System.Text.Json.Serialization;

namespace Empresa.Models
{
	public class Ciudad
	{
		public int CiudadId { get; set; }
		public string Nombre { get; set; }
		[JsonIgnore]
		public ICollection<Empleado> Empleado { get; set; }
	}
}
