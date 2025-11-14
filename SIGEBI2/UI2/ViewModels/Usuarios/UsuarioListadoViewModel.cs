using System.Collections.Generic;
using System.ComponentModel;
using UI2.Models.Usuarios;

namespace UI2.ViewModels.Usuarios
{
    public class UsuarioListadoViewModel
    {
        public BindingList<UsuarioListItemModel> Usuarios { get; } = new();

        public void CargarUsuarios(IEnumerable<UsuarioListItemModel> usuarios)
        {
            Usuarios.Clear();
            foreach (var usuario in usuarios)
            {
                Usuarios.Add(usuario);
            }
        }
    }
}