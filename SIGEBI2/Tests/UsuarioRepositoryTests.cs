using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using SIGEBI.Application.Services;
using SIGEBI.Application.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Tests
{
    [TestClass]
    public class UsuarioRepositoryTests
    {
        private AppDbContext? _context;
        private UsuarioRepository? _repository;
        private UsuarioService? _service;
        private IAuthService? _authService;

        // Usuario administrador de prueba (para pasar como usuarioActual)
        private Usuario _usuarioActual = new Usuario
        {
            Nombre = "Administrador de prueba",
            Email = "admin@sigebi.com",
            Rol = "Administrador",
            Activo = true
        };

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=.;Database=SIGEBI_Test;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            _context = new AppDbContext(options);
            _repository = new UsuarioRepository(_context);

            var inMemoryConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new List<KeyValuePair<string, string?>>
                {
                    new("Jwt:Key", "ClavePrueba123456789"),
                    new("Jwt:Issuer", "SIGEBI"),
                    new("Jwt:Audience", "SIGEBIUsers"),
                    new("Jwt:ExpireMinutes", "60")
                })
                .Build();


            _authService = new AuthService(inMemoryConfig);
            _service = new UsuarioService(_repository, _authService);

            // Inicialización de datos
            if (!_context.Usuarios.Any())
            {
                var usuarioActivo = new Usuario
                {
                    Nombre = "UsuarioActivo",
                    Email = "activo@test.com",
                    Contraseña = "1234",
                    Rol = "Admin",
                    Activo = true
                };

                var usuarioInactivo = new Usuario
                {
                    Nombre = "UsuarioInactivo",
                    Email = "inactivo@test.com",
                    Contraseña = "1234",
                    Rol = "Empleado",
                    Activo = false
                };

                _context.Usuarios.AddRange(usuarioActivo, usuarioInactivo);
                _context.SaveChanges();
            }
            else
            {
                if (!_context.Usuarios.Any(u => !u.Activo))
                {
                    var nuevoInactivo = new Usuario
                    {
                        Nombre = "AutoInactivo",
                        Email = "auto_inactivo@test.com",
                        Contraseña = "1234",
                        Rol = "Prueba",
                        Activo = false
                    };

                    _context.Usuarios.Add(nuevoInactivo);
                    _context.SaveChanges();
                }
            }
        }

        // CREATE
        [TestMethod]
        public async Task CrearUsuario_DeberiaGuardarCorrectamente()
        {
            var usuario = new Usuario
            {
                Nombre = "Usuario_" + System.Guid.NewGuid(),
                Email = "nuevo_" + System.Guid.NewGuid() + "@sigebi.com",
                Contraseña = "1234",
                Rol = "Bibliotecario"
            };

            var result = await _service!.CrearAsync(usuario, _usuarioActual);
            Assert.IsTrue(result.Success, result.Message);
            Assert.IsNotNull(result.Data);
        }

        // READ
        [TestMethod]
        public async Task ObtenerUsuarios_DeberiaRetornarLista()
        {
            var result = await _service!.ObtenerTodosAsync(_usuarioActual);
            Assert.IsTrue(result.Success, result.Message);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data!.Any(), "No se encontraron usuarios registrados.");
        }

        // UPDATE
        [TestMethod]
        public async Task ActualizarUsuario_DeberiaModificarDatos()
        {
            var usuario = await _context!.Usuarios.FirstOrDefaultAsync();
            if (usuario == null)
                Assert.Inconclusive("No hay usuarios en la base de datos para actualizar.");

            usuario!.Nombre = "Usuario Modificado";
            var result = await _service!.ActualizarAsync(usuario, _usuarioActual);
            Assert.IsTrue(result.Success, result.Message);
        }

        // DESACTIVAR
        [TestMethod]
        public async Task DesactivarUsuario_DeberiaCambiarEstado()
        {
            var usuario = await _context!.Usuarios.FirstOrDefaultAsync(u => u.Activo);
            if (usuario == null)
                Assert.Inconclusive("No hay usuarios activos para desactivar.");

            var result = await _service!.DesactivarAsync(usuario.Id, _usuarioActual);
            Assert.IsTrue(result.Success, result.Message);
        }

        // ACTIVAR
        [TestMethod]
        public async Task ActivarUsuario_DeberiaCambiarEstado()
        {
            var usuario = await _context!.Usuarios.FirstOrDefaultAsync(u => !u.Activo);
            if (usuario == null)
                Assert.Inconclusive("No hay usuarios inactivos para activar.");

            var result = await _service!.ActivarAsync(usuario.Id, _usuarioActual);
            Assert.IsTrue(result.Success, result.Message);
        }

        // DELETE
        [TestMethod]
        public async Task EliminarUsuario_DeberiaEliminarCorrectamente()
        {
            var usuario = await _context!.Usuarios
                .OrderByDescending(u => u.Id)
                .FirstOrDefaultAsync();

            if (usuario == null)
                Assert.Inconclusive("No hay usuarios disponibles para eliminar.");

            var result = await _service!.EliminarAsync(usuario.Id, _usuarioActual);
            Assert.IsTrue(result.Success, result.Message);
        }
    }
}

