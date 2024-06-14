namespace Empresa.Models
{
	public class Departamento
	{
		public int DepartamentoId { get; set; }
		public string Nombre { get; set; }
		public string Descripcion { get; set; }
		public ICollection<EmpleadoDepartamento> EmpleadoDepartamento { get; set; }
	}
}
