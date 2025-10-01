using System;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Shared.Base
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaModificacion { get; set; }
    }
}
