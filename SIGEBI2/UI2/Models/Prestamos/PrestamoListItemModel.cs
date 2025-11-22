namespace UI2.Models.Prestamos
{
    public class PrestamoListItemModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool Activo { get; set; }
        public List<PrestamoDetalleItemModel> Detalles { get; set; } = new();
    }
}
