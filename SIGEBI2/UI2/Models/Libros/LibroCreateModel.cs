namespace UI2.Models.Libros
{
    public class LibroCreateModel
    {
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Editorial { get; set; } = string.Empty;
        public int AnioPublicacion { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }
}
