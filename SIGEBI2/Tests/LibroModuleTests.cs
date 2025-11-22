using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared;
using SIGEBI.API.Controllers;
using SIGEBI.Application.DTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Shared.Base;
using Xunit;



namespace SIGEBI.Tests
{
    public class LibroModuleTests
    {
        private readonly Mock<ILibroRepository> _libroRepoMock;
        private readonly ILibroService _libroService;

        public LibroModuleTests()
        {
            _libroRepoMock = new Mock<ILibroRepository>();
            _libroService = new LibroService(_libroRepoMock.Object);
        }

        // ===============================================================
        // Tests: LibroService
        // ===============================================================

        [Fact]
        public async Task CrearAsync_Deberia_Crear_Libro_Correctamente()
        {
            var libro = new Libro { Id = 1, Titulo = "Clean Code", ISBN = "123456", Disponible = true };
            _libroRepoMock.Setup(r => r.ExisteISBNAsync(libro.ISBN)).ReturnsAsync(false);
            _libroRepoMock.Setup(r => r.AddAsync(It.IsAny<Libro>()))
                          .ReturnsAsync(OperationResult<Libro>.Ok(libro));

            var result = await _libroService.CrearAsync(libro);

            result.Success.Should().BeTrue();
            result.Data!.Titulo.Should().Be("Clean Code");
        }

        [Fact]
        public async Task CrearAsync_Deberia_Fallar_Si_ISBN_Existe()
        {
            var libro = new Libro { Titulo = "Duplicado", ISBN = "111" };
            _libroRepoMock.Setup(r => r.ExisteISBNAsync(libro.ISBN)).ReturnsAsync(true);

            var result = await _libroService.CrearAsync(libro);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("ya existe");
        }

        [Fact]
        public async Task ActualizarAsync_Deberia_Fallar_Si_Libro_No_Existe()
        {
            _libroRepoMock.Setup(r => r.GetByIdAsync(1))
                          .ReturnsAsync(OperationResult<Libro>.Fail("No encontrado"));

            var result = await _libroService.ActualizarAsync(1, new Libro { Id = 1, Titulo = "Nuevo" });

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Libro no encontrado.");
        }

        [Fact]
        public async Task BuscarAsync_Deberia_Retornar_Resultados()
        {
            var lista = new List<Libro>
            {
                new Libro { Id = 1, Titulo = "El Quijote", Autor = "Cervantes", Disponible = true }
            };

            _libroRepoMock.Setup(r => r.BuscarPorTituloOAutorAsync("Quijote"))
                          .ReturnsAsync(lista);

            var result = await _libroService.BuscarAsync("Quijote");

            result.Success.Should().BeTrue();
            result.Data.Should().ContainSingle(l => l.Titulo == "El Quijote");
        }

        [Fact]
        public async Task EstaDisponibleAsync_Deberia_Retornar_Estado_Correcto()
        {
            _libroRepoMock.Setup(r => r.EstaDisponibleAsync(1)).ReturnsAsync(true);

            var result = await _libroService.EstaDisponibleAsync(1);

            result.Success.Should().BeTrue();
            result.Data.Should().BeTrue();
        }

        [Fact]
        public async Task DesactivarAsync_Deberia_Retornar_Ok()
        {
            _libroRepoMock.Setup(r => r.DeleteAsync(1))
                          .ReturnsAsync(OperationResult.Ok("Desactivado"));

            var result = await _libroService.DesactivarAsync(1);

            result.Success.Should().BeTrue();
            result.Message.Should().Contain("Desactivado");
        }

        [Fact]
        public async Task ObtenerPorIdAsync_Deberia_Retornar_Libro()
        {
            var libro = new Libro { Id = 1, Titulo = "Clean Architecture" };
            _libroRepoMock.Setup(r => r.GetByIdAsync(1))
                          .ReturnsAsync(OperationResult<Libro>.Ok(libro));

            var result = await _libroService.ObtenerPorIdAsync(1);

            result.Success.Should().BeTrue();
            result.Data!.Titulo.Should().Be("Clean Architecture");
        }

        [Fact]
        public async Task ListarAsync_Deberia_Retornar_Todos_Los_Libros()
        {
            var lista = new List<Libro>
            {
                new Libro { Id = 1, Titulo = "Libro A" },
                new Libro { Id = 2, Titulo = "Libro B" }
            };

            _libroRepoMock.Setup(r => r.GetAllAsync())
                          .ReturnsAsync(OperationResult<IEnumerable<Libro>>.Ok(lista));

            var result = await _libroService.ListarAsync();

            result.Success.Should().BeTrue();
            result.Data.Should().HaveCount(2);
        }

        // ===============================================================
        // Tests: LibroController
        // ===============================================================

        [Fact]
        public async Task Crear_Deberia_Retornar_Ok_Si_Exitoso()
        {
            var dto = new LibroDTO { Id = 1, Titulo = "Clean Code", ISBN = "123" };
            var libro = new Libro { Id = 1, Titulo = "Clean Code", ISBN = "123" };

            var mockService = new Mock<ILibroService>();
            mockService.Setup(s => s.CrearAsync(It.IsAny<Libro>()))
                       .ReturnsAsync(ServiceResult<Libro>.Ok(libro));

            var controller = new LibroController(mockService.Object);
            var result = await controller.Crear(dto) as OkObjectResult;

            result.Should().NotBeNull();
            var data = result!.Value as LibroDTO;
            data!.Titulo.Should().Be("Clean Code");
        }

        [Fact]
        public async Task Actualizar_Deberia_Retornar_NotFound_Si_No_Existe()
        {
            var mockService = new Mock<ILibroService>();
            mockService.Setup(s => s.ActualizarAsync(1, It.IsAny<Libro>()))
                       .ReturnsAsync(ServiceResult<Libro>.Fail("No encontrado"));

            var controller = new LibroController(mockService.Object);
            var result = await controller.Actualizar(1, new LibroDTO()) as NotFoundObjectResult;

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Buscar_Deberia_Retornar_Lista_De_Libros()
        {
            var lista = new List<Libro> { new Libro { Id = 1, Titulo = "1984", Autor = "Orwell" } };
            var mockService = new Mock<ILibroService>();
            mockService.Setup(s => s.BuscarAsync("1984"))
                       .ReturnsAsync(ServiceResult<IEnumerable<Libro>>.Ok(lista));

            var controller = new LibroController(mockService.Object);
            var result = await controller.Buscar("1984") as OkObjectResult;

            result.Should().NotBeNull();
            (result!.Value as List<LibroDTO>)!.Should().ContainSingle(l => l.Titulo == "1984");
        }

        [Fact]
        public async Task ConsultarDisponibilidad_Deberia_Retornar_True()
        {
            var mockService = new Mock<ILibroService>();
            mockService.Setup(s => s.EstaDisponibleAsync(1))
                       .ReturnsAsync(ServiceResult<bool>.Ok(true));

            var controller = new LibroController(mockService.Object);
            var result = await controller.ConsultarDisponibilidad(1) as OkObjectResult;

            result.Should().NotBeNull();
            (result!.Value as ServiceResult<bool>)!.Data.Should().BeTrue();
        }

        [Fact]
        public async Task Desactivar_Deberia_Retornar_NotFound_Si_Falla()
        {
            var mockService = new Mock<ILibroService>();
            mockService.Setup(s => s.DesactivarAsync(1))
                       .ReturnsAsync(ServiceResult.Fail("No encontrado"));

            var controller = new LibroController(mockService.Object);
            var result = await controller.Desactivar(1) as NotFoundObjectResult;

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task ObtenerPorId_Deberia_Retornar_Libro_Si_Existe()
        {
            var libro = new Libro { Id = 1, Titulo = "Cien Años de Soledad" };
            var mockService = new Mock<ILibroService>();
            mockService.Setup(s => s.ObtenerPorIdAsync(1))
                       .ReturnsAsync(ServiceResult<Libro>.Ok(libro));

            var controller = new LibroController(mockService.Object);
            var result = await controller.ObtenerPorId(1) as OkObjectResult;

            result.Should().NotBeNull();
            (result!.Value as LibroDTO)!.Titulo.Should().Be("Cien Años de Soledad");
        }

        [Fact]
        public async Task Listar_Deberia_Retornar_Todos_Los_Libros()
        {
            var lista = new List<Libro>
            {
                new Libro { Id = 1, Titulo = "Libro 1" },
                new Libro { Id = 2, Titulo = "Libro 2" }
            };

            var mockService = new Mock<ILibroService>();
            mockService.Setup(s => s.ListarAsync())
                       .ReturnsAsync(ServiceResult<IEnumerable<Libro>>.Ok(lista));

            var controller = new LibroController(mockService.Object);
            var result = await controller.Listar() as OkObjectResult;

            result.Should().NotBeNull();
            (result!.Value as List<LibroDTO>)!.Should().HaveCount(2);
        }
    }
}
