using System.Collections.Generic;
using System.ComponentModel;
using UI2.Models.Libros;

namespace UI2.ViewModels.Libros
{
    public class LibroListadoViewModel
    {
        public LibroFiltroModel Filtro { get; } = new();
        public BindingList<LibroListItemModel> Libros { get; } = new();

        public void CargarLibros(IEnumerable<LibroListItemModel> libros)
        {
            Libros.Clear();
            foreach (var libro in libros)
            {
                Libros.Add(libro);
            }
        }
    }
}