using System.ComponentModel;
using UI2.Models.Usuarios;

namespace UI2.ViewModels.Usuarios
{
    public class UsuarioListadoViewModel
    {
        private readonly BindingList<UsuarioListItemModel> _usuariosFiltrados = new();
        private readonly List<UsuarioListItemModel> _usuariosOriginales = new();
        private string _filtroActual = string.Empty;

        public BindingList<UsuarioListItemModel> Usuarios => _usuariosFiltrados;

        public void CargarUsuarios(IEnumerable<UsuarioListItemModel> usuarios)
        {
            _usuariosOriginales.Clear();
            _usuariosOriginales.AddRange(usuarios);
            AplicarFiltro(_filtroActual);
        }

        public void AgregarUsuario(UsuarioListItemModel usuario)
        {
            _usuariosOriginales.Add(usuario);
            AplicarFiltro(_filtroActual);
        }

        public void ActualizarUsuario(UsuarioListItemModel usuarioActualizado)
        {
            var indice = _usuariosOriginales.FindIndex(u => u.Id == usuarioActualizado.Id);
            if (indice >= 0)
            {
                _usuariosOriginales[indice] = usuarioActualizado;
            }

            AplicarFiltro(_filtroActual);
        }

        public void ActualizarEstado(int usuarioId, bool activo)
        {
            var usuario = _usuariosOriginales.FirstOrDefault(u => u.Id == usuarioId);
            if (usuario != null)
            {
                usuario.Activo = activo;
            }

            AplicarFiltro(_filtroActual);
        }

        public void AplicarFiltro(string filtro)
        {
            _filtroActual = filtro ?? string.Empty;
            var texto = _filtroActual.Trim();

            IEnumerable<UsuarioListItemModel> usuarios = _usuariosOriginales;
            if (!string.IsNullOrWhiteSpace(texto))
            {
                var tokens = texto.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var token in tokens)
                {
                    usuarios = FiltrarPorToken(usuarios, token);
                }
            }

            _usuariosFiltrados.RaiseListChangedEvents = false;
            _usuariosFiltrados.Clear();
            foreach (var usuario in usuarios)
            {
                _usuariosFiltrados.Add(usuario);
            }

            _usuariosFiltrados.RaiseListChangedEvents = true;
            _usuariosFiltrados.ResetBindings();
        }

        private static IEnumerable<UsuarioListItemModel> FiltrarPorToken(
            IEnumerable<UsuarioListItemModel> usuarios,
            string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return usuarios;
            }

            token = token.Trim();
            if (EsTokenActivo(token))
            {
                return usuarios.Where(u => u.Activo);
            }

            if (EsTokenInactivo(token))
            {
                return usuarios.Where(u => !u.Activo);
            }

            return usuarios.Where(u =>
                (!string.IsNullOrEmpty(u.Nombre) && u.Nombre.Contains(token, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(token, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(u.Rol) && u.Rol.Contains(token, StringComparison.OrdinalIgnoreCase)));
        }

        private static bool EsTokenActivo(string token)
        {
            return token.Equals("activo", StringComparison.OrdinalIgnoreCase) ||
                   token.Equals("activos", StringComparison.OrdinalIgnoreCase) ||
                   token.Equals("habilitado", StringComparison.OrdinalIgnoreCase) ||
                   token.Equals("habilitados", StringComparison.OrdinalIgnoreCase);
        }

        private static bool EsTokenInactivo(string token)
        {
            return token.Equals("inactivo", StringComparison.OrdinalIgnoreCase) ||
                   token.Equals("inactivos", StringComparison.OrdinalIgnoreCase) ||
                   token.Equals("deshabilitado", StringComparison.OrdinalIgnoreCase) ||
                   token.Equals("deshabilitados", StringComparison.OrdinalIgnoreCase);
        }
    }
}
