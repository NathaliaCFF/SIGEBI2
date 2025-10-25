using Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SIGEBI.Application.Services
{
    // ============================================================================
    // SERVICIO: AuthService
    // MÓDULO: Autenticación
    // DESCRIPCIÓN: Implementa la generación de tokens JWT (JSON Web Token) para
    // los usuarios autenticados del sistema SIGEBI, garantizando la seguridad de
    // las sesiones y el control de acceso por roles.
    // CASOS DE USO RELACIONADOS:
    //   - CU-00: Autenticar usuario
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
        // CASO DE USO: CU-00 - Generar token JWT
        // DESCRIPCIÓN: Crea un token de autenticación JWT con la información del
        // usuario (correo, nombre y rol). Este token se utiliza para validar el acceso
        // del usuario a las funcionalidades del sistema según su perfil.
        // ============================================================================
        public AuthResponseDTO GenerarToken(Usuario usuario)
        {
            // CU-00: Obtener parámetros de configuración del token desde appsettings.json
            var jwtSettings = _configuration.GetSection("Jwt");
            var keyValue = jwtSettings["Key"] ?? throw new InvalidOperationException("La clave JWT no está configurada.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));


            // CU-00: Definir los claims (información contenida en el token)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim("rol", usuario.Rol ?? "Usuario"),
                new Claim("nombre", usuario.Nombre),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // CU-00: Crear credenciales de firma con algoritmo seguro HMAC SHA256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // CU-00: Generar el token JWT con tiempo de expiración y emisor configurado
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            // CU-00: Retornar la respuesta con los datos del token y usuario autenticado
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
