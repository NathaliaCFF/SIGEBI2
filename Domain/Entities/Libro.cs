using SIGEBI.Shared.Base;

namespace SIGEBI.Domain.Entities
{
    public class Libro : BaseEntity
    {
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Editorial { get; set; } = string.Empty;
        public int AnioPublicacion { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public bool Disponible { get; set; }
    }

}
