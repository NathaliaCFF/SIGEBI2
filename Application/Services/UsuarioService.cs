using Application.DTOs;
using Shared;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Application.Services
{
    // ============================================================================
    // SERVICIO: UsuarioService
    // MÓDULO: Gestión de Usuarios / Autenticación
    // DESCRIPCIÓN: Contiene la lógica de negocio y validaciones correspondientes
    // a la administración de usuarios y autenticación del sistema SIGEBI.
    // CASOS DE USO RELACIONADOS:
    //   - CU-00: Autenticar usuario
    //   - CU-05: Registrar usuario
    //   - CU-06: Editar usuario
    //   - CU-07: Activar usuario
    //   - CU-08: Desactivar usuario
    // CAPA: Aplicación
    // ============================================================================
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuthService _authService;

        public UsuarioService(IUsuarioRepository usuarioRepository, IAuthService authService)
        {
            _usuarioRepository = usuarioRepository;
            _authService = authService;
        }

        // ============================================================================
        // CASO DE USO: CU-05 - Registrar usuario
        // DESCRIPCIÓN: Permite al Administrador registrar un nuevo usuario,
        // validando campos obligatorios, duplicidad de correo y aplicando hash
        // a la contraseña antes de guardarlo en la base de datos.
        // ============================================================================
        public async Task<OperationResult<Usuario>> CrearAsync(Usuario usuario, Usuario usuarioActual)
        {
            if (usuarioActual.Rol != "Administrador")
                return OperationResult<Usuario>.Fail("No tiene permisos para crear usuarios.");

            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                return OperationResult<Usuario>.Fail("El nombre del usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                return OperationResult<Usuario>.Fail("El correo electrónico es obligatorio.");

            // CU-05: Verificar duplicidad de correo
            var existente = await _usuarioRepository.ObtenerPorEmailAsync(usuario.Email);
            if (existente != null)
                return OperationResult<Usuario>.Fail("Ya existe un usuario registrado con ese correo electrónico.");

            // CU-05: Aplicar hash a la contraseña y registrar
            usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);
            usuario.FechaCreacion = DateTime.UtcNow;

            return await _usuarioRepository.AddAsync(usuario);
        }

        // ============================================================================
        // CASO DE USO: CU-06 - Editar usuario
        // DESCRIPCIÓN: Permite al Administrador modificar los datos de un usuario
        // existente, validando el rol y la existencia previa del registro.
        // ============================================================================
        public async Task<OperationResult<Usuario>> ActualizarAsync(Usuario usuario, Usuario usuarioActual)
        {
            if (usuarioActual.Rol != "Administrador")
                return OperationResult<Usuario>.Fail("No tiene permisos para modificar usuarios.");

            if (usuario.Id <= 0)
                return OperationResult<Usuario>.Fail("Debe especificar un ID válido.");

            // CU-06: Validar existencia previa del usuario
            var existente = await _usuarioRepository.GetByIdAsync(usuario.Id);
            if (existente.Data == null)
                return OperationResult<Usuario>.Fail("El usuario no existe.");

            usuario.FechaModificacion = DateTime.UtcNow;
            return await _usuarioRepository.UpdateAsync(usuario);
        }

        // ============================================================================
        // MÉTODO AUXILIAR (Eliminación lógica o física según configuración)
        // *No corresponde directamente a un caso de uso descrito, pero apoya la gestión.
        // ============================================================================
        public async Task<OperationResult> EliminarAsync(int id, Usuario usuarioActual)
        {
            if (usuarioActual.Rol != "Administrador")
                return OperationResult.Fail("No tiene permisos para eliminar usuarios.");

            if (id <= 0)
                return OperationResult.Fail("Debe indicar un ID válido.");

            var existente = await _usuarioRepository.GetByIdAsync(id);
            if (existente.Data == null)
                return OperationResult.Fail("El usuario no existe o ya fue eliminado.");

            return await _usuarioRepository.DeleteAsync(id);
        }

        // ============================================================================
        // MÉTODO AUXILIAR: ObtenerTodosAsync
        // DESCRIPCIÓN: Recupera todos los usuarios registrados, filtrando según rol.
        // *Apoya varios casos de uso de visualización (CU-06, CU-07, CU-08).
        // ============================================================================
        public async Task<OperationResult<IEnumerable<Usuario>>> ObtenerTodosAsync(Usuario usuarioActual)
        {
            var result = await _usuarioRepository.GetAllAsync();

            if (result.Data == null || !result.Data.Any())
                return OperationResult<IEnumerable<Usuario>>.Fail("No se encontraron usuarios registrados.");

            if (usuarioActual.Rol != "Administrador")
            {
                var activos = result.Data.Where(u => u.Activo);
                return OperationResult<IEnumerable<Usuario>>.Ok(activos, "Usuarios activos obtenidos correctamente.");
            }

            return OperationResult<IEnumerable<Usuario>>.Ok(result.Data, "Usuarios obtenidos correctamente.");
        }

        // ============================================================================
        // CASO DE USO: CU-00 - Autenticar usuario
        // DESCRIPCIÓN: Permite validar las credenciales de inicio de sesión del usuario,
        // verificando su estado activo e integrando el servicio de autenticación JWT.
        // ============================================================================
        public async Task<OperationResult<AuthResponseDTO>> AutenticarAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return OperationResult<AuthResponseDTO>.Fail("Debe ingresar las credenciales.");

            var usuario = await _usuarioRepository.ObtenerPorEmailAsync(email);

            // 🟡 TEMPORAL: mostrar el hash actual para depurar
            Console.WriteLine("---- DEBUG LOGIN ----");
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Contraseña ingresada: {password}");
            Console.WriteLine($"Hash almacenado: {usuario.Contraseña}");
            Console.WriteLine($"Longitud del hash: {usuario.Contraseña?.Length}");
            Console.WriteLine("----------------------");

            // CU-00: Validar existencia y estado activo
            if (usuario == null || !usuario.Activo)
                return OperationResult<AuthResponseDTO>.Fail("Credenciales inválidas o usuario inactivo.");

            // CU-00: Validar contraseña con hash
            bool validPassword = BCrypt.Net.BCrypt.Verify(password, usuario.Contraseña);
            if (!validPassword)
                return OperationResult<AuthResponseDTO>.Fail("Contraseña incorrecta.");

            // CU-00: Generar token JWT
            var tokenResponse = _authService.GenerarToken(usuario);
            return OperationResult<AuthResponseDTO>.Ok(tokenResponse, "Autenticación exitosa.");
        }

        // ============================================================================
        // CASO DE USO: CU-07 - Activar usuario
        // DESCRIPCIÓN: Permite al Administrador reactivar una cuenta previamente
        // desactivada, habilitando al usuario para realizar préstamos.
        // ============================================================================
        public async Task<OperationResult> ActivarAsync(int id, Usuario usuarioActual)
        {
            if (usuarioActual.Rol != "Administrador")
                return OperationResult.Fail("No tiene permisos para activar usuarios.");

            var result = await _usuarioRepository.ActivarUsuarioAsync(id);
            return result
                ? OperationResult.Ok("Usuario activado correctamente.")
                : OperationResult.Fail("No se pudo activar el usuario.");
        }

        // ============================================================================
        // CASO DE USO: CU-08 - Desactivar usuario
        // DESCRIPCIÓN: Permite al Administrador deshabilitar temporalmente un usuario,
        // impidiendo que realice nuevos préstamos hasta ser reactivado.
        // ============================================================================
        public async Task<OperationResult> DesactivarAsync(int id, Usuario usuarioActual)
        {
            if (usuarioActual.Rol != "Administrador")
                return OperationResult.Fail("No tiene permisos para desactivar usuarios.");

            var result = await _usuarioRepository.DesactivarUsuarioAsync(id);
            return result
                ? OperationResult.Ok("Usuario desactivado correctamente.")
                : OperationResult.Fail("No se pudo desactivar el usuario.");
        }
    }
}
