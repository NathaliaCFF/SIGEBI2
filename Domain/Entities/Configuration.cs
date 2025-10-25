using SIGEBI.Shared.Base;

namespace SIGEBI.Domain.Entities
{
    public class Configuration : BaseEntity
    {
        public int DuracionPrestamoDias { get; set; } = 7; 
    }

}
