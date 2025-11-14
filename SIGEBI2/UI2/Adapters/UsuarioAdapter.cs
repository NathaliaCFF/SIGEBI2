using System.Collections.Generic;
using System.Linq;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities;
using UI2.Models.Common;
using UI2.Models.Usuarios;

namespace UI2.Adapters
{
    public class UsuarioAdapter
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioAdapter(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task<AdapterResult<IList<UsuarioListItemModel>>> ObtenerUsuariosAsync(Usuario usuarioActual)
        {
            var resultado = await _usuarioService.ObtenerTodosAsync(usuarioActual);
            if (!resultado.Success || resultado.Data == null)
            {
                return AdapterResult<IList<UsuarioListItemModel>>.Fail(resultado.Message ?? "No se pudieron cargar los usuarios.");
            }

            var lista = resultado.Data
                .Select(u => new UsuarioListItemModel
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Rol = u.Rol,
                    Activo = u.Activo,
                    FechaCreacion = u.FechaCreacion
                })
                .ToList();

            return AdapterResult<IList<UsuarioListItemModel>>.Ok(lista, resultado.Message ?? "Usuarios cargados correctamente.");
        }

        public async Task<AdapterResult<UsuarioListItemModel>> CrearUsuarioAsync(UsuarioFormModel model, Usuario usuarioActual)
        {
            var entidad = new Usuario
            {
                Nombre = model.Nombre,
                Email = model.Email,
                Contraseña = model.Password,
                Rol = model.Rol,
                Activo = model.Activo
            };

            var resultado = await _usuarioService.CrearAsync(entidad, usuarioActual);
            if (!resultado.Success || resultado.Data == null)
            {
                return AdapterResult<UsuarioListItemModel>.Fail(resultado.Message ?? "No se pudo registrar el usuario.");
            }

            var usuario = resultado.Data;
            return AdapterResult<UsuarioListItemModel>.Ok(new UsuarioListItemModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol,
                Activo = usuario.Activo,
                FechaCreacion = usuario.FechaCreacion
            }, resultado.Message ?? "Usuario registrado correctamente.");
        }

        public async Task<AdapterResult<UsuarioListItemModel>> ActualizarUsuarioAsync(UsuarioFormModel model, Usuario usuarioActual)
        {
            var entidad = new Usuario
            {
                Id = model.Id,
                Nombre = model.Nombre,
                Email = model.Email,
                Rol = model.Rol,
                Activo = model.Activo
            };

            var resultado = await _usuarioService.ActualizarAsync(entidad, usuarioActual);
            if (!resultado.Success || resultado.Data == null)
            {
                return AdapterResult<UsuarioListItemModel>.Fail(resultado.Message ?? "No se pudo actualizar el usuario.");
            }

            var usuario = resultado.Data;
            return AdapterResult<UsuarioListItemModel>.Ok(new UsuarioListItemModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol,
                Activo = usuario.Activo,
                FechaCreacion = usuario.FechaCreacion
            }, resultado.Message ?? "Usuario actualizado correctamente.");
        }

        public async Task<AdapterResult> ActivarUsuarioAsync(int id, Usuario usuarioActual)
        {
            var resultado = await _usuarioService.ActivarAsync(id, usuarioActual);
            return resultado.Success
                ? AdapterResult.Ok(resultado.Message ?? "Usuario activado correctamente.")
                : AdapterResult.Fail(resultado.Message ?? "No se pudo activar el usuario.");
        }

        public async Task<AdapterResult> DesactivarUsuarioAsync(int id, Usuario usuarioActual)
        {
            var resultado = await _usuarioService.DesactivarAsync(id, usuarioActual);
            return resultado.Success
                ? AdapterResult.Ok(resultado.Message ?? "Usuario desactivado correctamente.")
                : AdapterResult.Fail(resultado.Message ?? "No se pudo desactivar el usuario.");
        }
    }
}