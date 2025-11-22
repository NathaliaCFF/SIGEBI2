using Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SIGEBI.Application.Services
{
    // ============================================================================
    // SERVICIO: AuthService
    // MÓDULO: Autenticación del Sistema
    // DESCRIPCIÓN: Gestiona la autenticación de usuarios del sistema SIGEBI,
    // generando tokens JWT que contienen la identidad, rol y permisos del usuario
    // autenticado. Este servicio forma parte del flujo principal de acceso al
    // sistema, garantizando seguridad y control de roles.
    // CASOS DE USO RELACIONADOS:
    //   - CU-00: Autenticar Usuario
    // CAPA: Aplicación
    // ============================================================================
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ============================================================================
        // CASO DE USO: CU-00 - Autenticar Usuario
        // DESCRIPCIÓN:
        //   Este método forma parte del flujo del caso de uso "Autenticar Usuario".
        //   Se ejecuta una vez que el sistema valida las credenciales ingresadas por
        //   el usuario. Genera un token JWT que encapsula la identidad, nombre y rol
        //   del usuario, junto con su tiempo de expiración, permitiendo acceder a las
        //   funcionalidades autorizadas según su perfil (Administrador, Docente o
        //   Estudiante).
        //
        // FLUJO RELACIONADO:
        //   - Paso 4: El sistema valida las credenciales contra la base de datos.
        //   - Paso 5: El sistema asigna el rol correspondiente.
        //   - Paso 6: El sistema genera y devuelve el token JWT.
        // ============================================================================
        public AuthResponseDTO GenerarToken(Usuario usuario)
        {
            // CU-00: Obtiene la configuración de los parámetros JWT desde appsettings.json
            var jwtSettings = _configuration.GetSection("Jwt");
            var keyValue = jwtSettings["Key"]
                ?? throw new InvalidOperationException("La clave JWT no está configurada.");

            // CU-00: Crea la clave simétrica que se utilizará para firmar el token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));

            // CU-00: Define los claims del usuario (email, nombre, rol y JTI único)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // CU-00: Define las credenciales de firma (HMAC-SHA256)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // CU-00: Construye el token JWT con los parámetros configurados
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            // CU-00: Retorna el DTO con el token, la expiración, nombre y rol del usuario autenticado
            return new AuthResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = token.ValidTo,
                NombreUsuario = usuario.Nombre,
                Rol = usuario.Rol ?? "Usuario"
            };
        }
    }
}
