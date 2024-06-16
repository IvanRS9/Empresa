namespace Empresa.Models
{
	// DTO (Data Transfer Object) para examen o para ganar un punto por si pregunta el profe
	public class EmpleadoDTO
	{
		public string Nombre { get; set; }
		public DateTime FechaIngreso { get; set; }
		public string Puesto { get; set; }
		public double Sueldo { get; set; }
		public int CiudadId { get; set; }
		public int DepartamentoId { get; set; }
	}
}
