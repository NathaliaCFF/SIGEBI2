namespace UI2.Models.Prestamos
{
    public class PrestamoCreateModel
    {
        public int UsuarioId { get; set; }
        public List<int> LibrosIds { get; set; } = new();
    }
}
