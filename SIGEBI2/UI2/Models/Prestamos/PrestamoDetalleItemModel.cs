namespace UI2.Models.Prestamos
{
    public class PrestamoDetalleItemModel
    {
        public int LibroId { get; set; }
        public string TituloLibro { get; set; } = string.Empty;
        public bool Devuelto { get; set; }
        public int DetalleId { get; set; }

        public DateTime? FechaDevolucion { get; set; }
    }
}