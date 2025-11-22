using Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Shared;
using SIGEBI.API.Controllers;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using System.Security.Claims;
using Xunit;

namespace SIGEBI.Tests
{
    public class UsuarioModuleTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly UsuarioService _usuarioService;
        private readonly AuthService _authService;

        public UsuarioModuleTests()
        {
            _usuarioRepoMock = new Mock<IUsuarioRepository>();
            _authServiceMock = new Mock<IAuthService>();
            _configMock = new Mock<IConfiguration>();

            _usuarioService = new UsuarioService(_usuarioRepoMock.Object, _authServiceMock.Object);
            _authService = new AuthService(_configMock.Object);
        }

        // --------------------------------------------------------------
        // UsuarioService Tests
        // --------------------------------------------------------------

        [Fact]
        public async Task CrearAsync_Deberia_Fallar_Si_No_Es_Administrador()
        {
            var usuario = new Usuario { Nombre = "Test", Email = "test@mail.com", Contraseña = "123" };
            var usuarioActual = new Usuario { Rol = "Usuario" };

            var result = await _usuarioService.CrearAsync(usuario, usuarioActual);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("No tiene permisos");
        }

        [Fact]
        public async Task CrearAsync_Deberia_Crear_Usuario_Si_Datos_Son_Validos()
        {
            var usuario = new Usuario { Nombre = "Nuevo", Email = "nuevo@mail.com", Contraseña = "123" };
            var admin = new Usuario { Rol = "Administrador" };

            _usuarioRepoMock.Setup(r => r.ObtenerPorEmailAsync(usuario.Email)).ReturnsAsync((Usuario?)null);
            _usuarioRepoMock.Setup(r => r.AddAsync(It.IsAny<Usuario>()))
                            .ReturnsAsync(OperationResult<Usuario>.Ok(usuario));

            var result = await _usuarioService.CrearAsync(usuario, admin);

            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Contraseña.Should().NotBe("123"); // Debe estar hasheada
        }

        [Fact]
        public async Task ActualizarAsync_Deberia_Fallar_Si_No_Existe()
        {
            var admin = new Usuario { Rol = "Administrador" };
            _usuarioRepoMock.Setup(r => r.GetByIdAsync(1))
                            .ReturnsAsync(OperationResult<Usuario>.Fail("No encontrado"));

            var usuario = new Usuario { Id = 1, Nombre = "Editado" };

            var result = await _usuarioService.ActualizarAsync(usuario, admin);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task ObtenerTodosAsync_Deberia_Retornar_Solo_Activos_Si_No_Es_Admin()
        {
            var lista = new List<Usuario>
            {
                new Usuario { Id = 1, Activo = true },
                new Usuario { Id = 2, Activo = false }
            };

            var user = new Usuario { Rol = "Usuario" };

            _usuarioRepoMock.Setup(r => r.GetAllAsync())
                            .ReturnsAsync(OperationResult<IEnumerable<Usuario>>.Ok(lista));

            var result = await _usuarioService.ObtenerTodosAsync(user);

            result.Success.Should().BeTrue();
            result.Data.Should().OnlyContain(u => u.Activo);
        }

        [Fact]
        public async Task AutenticarAsync_Deberia_Fallar_Si_Password_Incorrecta()
        {
            var usuario = new Usuario
            {
                Email = "test@mail.com",
                Contraseña = BCrypt.Net.BCrypt.HashPassword("abc"),
                Activo = true
            };

            _usuarioRepoMock.Setup(r => r.ObtenerPorEmailAsync(usuario.Email))
                            .ReturnsAsync(usuario);

            var result = await _usuarioService.AutenticarAsync(usuario.Email, "wrong");

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Contraseña incorrecta");
        }

        [Fact]
        public async Task AutenticarAsync_Deberia_Generar_Token_Si_Credenciales_Son_Correctas()
        {
            var usuario = new Usuario
            {
                Email = "ok@mail.com",
                Contraseña = BCrypt.Net.BCrypt.HashPassword("123"),
                Activo = true,
                Rol = "Administrador",
                Nombre = "Admin"
            };

            _usuarioRepoMock.Setup(r => r.ObtenerPorEmailAsync(usuario.Email))
                            .ReturnsAsync(usuario);
            _authServiceMock.Setup(a => a.GenerarToken(usuario))
                            .Returns(new AuthResponseDTO { Token = "jwt-token" });

            var result = await _usuarioService.AutenticarAsync(usuario.Email, "123");

            result.Success.Should().BeTrue();
            result.Data!.Token.Should().Be("jwt-token");
        }

        [Fact]
        public async Task ActivarAsync_Deberia_Fallar_Si_No_Es_Admin()
        {
            var usuario = new Usuario { Rol = "Usuario" };
            var result = await _usuarioService.ActivarAsync(1, usuario);

            result.Success.Should().BeFalse();
        }

        // --------------------------------------------------------------
        // AuthService Tests
        // --------------------------------------------------------------

        [Fact]
        public void GenerarToken_Deberia_Devolver_Token_Valido()
        {
            var usuario = new Usuario { Email = "a@b.com", Nombre = "Test", Rol = "Administrador" };

            var jwtSection = new Mock<IConfigurationSection>();
            jwtSection.Setup(s => s["Key"]).Returns("ClaveSuperSeguraParaJWT_Test_1234567890!!");
            jwtSection.Setup(s => s["Issuer"]).Returns("SIGEBI");
            jwtSection.Setup(s => s["Audience"]).Returns("SIGEBI-Users");
            jwtSection.Setup(s => s["ExpireMinutes"]).Returns("30");

            _configMock.Setup(c => c.GetSection("Jwt")).Returns(jwtSection.Object);

            var service = new AuthService(_configMock.Object);
            var token = service.GenerarToken(usuario);

            token.Token.Should().NotBeNullOrEmpty();
            token.NombreUsuario.Should().Be(usuario.Nombre);
        }


        // --------------------------------------------------------------
        // UsuarioController Tests
        // --------------------------------------------------------------

        [Fact]
        public async Task ObtenerTodos_Deberia_Retornar_Usuarios_Si_Es_Admin()
        {
            var mockService = new Mock<IUsuarioService>();
            mockService.Setup(s => s.ObtenerTodosAsync(It.IsAny<Usuario>()))
                       .ReturnsAsync(OperationResult<IEnumerable<Usuario>>.Ok(new List<Usuario>
                       {
                           new Usuario { Id = 1, Nombre = "Juan" }
                       }));

            var controller = CrearController(mockService.Object, "Administrador");

            var result = await controller.ObtenerTodos() as OkObjectResult;

            result.Should().NotBeNull();
            var data = result!.Value as OperationResult<IEnumerable<Usuario>>;
            data!.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Crear_Deberia_Retornar_Ok_Si_Usuario_Creado()
        {
            var usuario = new Usuario { Nombre = "Nuevo", Email = "nuevo@correo.com" };

            var mockService = new Mock<IUsuarioService>();
            mockService.Setup(s => s.CrearAsync(It.IsAny<Usuario>(), It.IsAny<Usuario>()))
                       .ReturnsAsync(OperationResult<Usuario>.Ok(usuario));

            var controller = CrearController(mockService.Object, "Administrador");
            var result = await controller.Crear(usuario) as OkObjectResult;

            result.Should().NotBeNull();
            (result!.Value as OperationResult<Usuario>)!.Data!.Nombre.Should().Be("Nuevo");
        }

        // --------------------------------------------------------------
        // Helpers
        // --------------------------------------------------------------
        private UsuarioController CrearController(IUsuarioService service, string rol)
        {
            var controller = new UsuarioController(service);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "Admin"),
                new Claim(ClaimTypes.NameIdentifier, "admin@mail.com"),
                new Claim(ClaimTypes.Role, rol)
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return controller;
        }
    }
}
