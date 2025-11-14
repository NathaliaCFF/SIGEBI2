using System.Collections.Generic;
using System.ComponentModel;
using UI2.Models.Prestamos;

namespace UI2.ViewModels.Prestamos
{
    public class PrestamoListadoViewModel
    {
        public BindingList<PrestamoListItemModel> Prestamos { get; } = new();
        public int UsuarioIdConsulta { get; set; }

        public void CargarPrestamos(IEnumerable<PrestamoListItemModel> prestamos)
        {
            Prestamos.Clear();
            foreach (var prestamo in prestamos)
            {
                Prestamos.Add(prestamo);
            }
        }
    }
}