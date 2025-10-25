namespace SIGEBI.Application.DTOs
{
    public class ReporteDTO
    {
        public int LibroId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int CantidadPrestamos { get; set; }
    }
}
