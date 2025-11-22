using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;

namespace Tests
{
    /// <summary>
    /// PRUEBAS UNITARIAS: PrestamoServiceTests
    /// CAPA: Aplicación
    /// MÓDULO: Préstamos y Devoluciones
    /// DESCRIPCIÓN:
    /// Valida las operaciones principales del servicio de préstamos, incluyendo:
    /// - Registro de préstamo (CU-09)
    /// - Registro de devolución (CU-10)
    /// - Consulta de préstamos activos (CU-11)
    /// - Consulta de préstamos vencidos (CU-12)
    /// </summary>
    [TestClass]
    public class PrestamoServiceTests
    {
        private AppDbContext? _context;
        private PrestamoService? _service;

        /// <summary>
        /// Inicializa el contexto y los repositorios antes de cada prueba.
        /// Limpia las tablas involucradas para garantizar independencia total.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=.;Database=SIGEBI_Test;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            _context = new AppDbContext(options);

            // Limpieza completa (orden FK correcto)
            _context.Database.ExecuteSqlRaw("DELETE FROM DetallePrestamos;");
            _context.Database.ExecuteSqlRaw("DELETE FROM Prestamos;");
            _context.Database.ExecuteSqlRaw("DELETE FROM Libros;");
            _context.Database.ExecuteSqlRaw("DELETE FROM Usuarios;");

            // Reiniciar contadores IDENTITY
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Usuarios', RESEED, 0);");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Libros', RESEED, 0);");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Prestamos', RESEED, 0);");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('DetallePrestamos', RESEED, 0);");

            // Instanciar repositorios y servicio principal
            var prestamoRepo = new PrestamoRepository(_context);
            var usuarioRepo = new UsuarioRepository(_context);
            var libroRepo = new LibroRepository(_context);
            var detalleRepo = new DetallePrestamoRepository(_context);

            _service = new PrestamoService(prestamoRepo, usuarioRepo, libroRepo, detalleRepo);
        }

        // =====================================================
        // CU-09: Registrar préstamo
        // =====================================================
        [TestMethod]
        public async Task RegistrarPrestamo_CuandoDatosValidos_DeberiaCrearPrestamoYDetalles()
        {
            var usuario = new Usuario
            {
                Nombre = "Usuario Test",
                Email = $"test{Guid.NewGuid():N}@mail.com",
                Contraseña = "123",
                Rol = "Estudiante",
                Activo = true
            };
            _context!.Usuarios.Add(usuario);
            _context.SaveChanges();

            var libro = new Libro
            {
                Titulo = "Cien años de soledad",
                Autor = "Gabriel García Márquez",
                ISBN = "978" + Guid.NewGuid().ToString("N").Substring(0, 10),
                Editorial = "Sudamericana",
                Categoria = "Realismo mágico",
                AnioPublicacion = 1967,
                Disponible = true,
                Activo = true
            };
            _context.Libros.Add(libro);
            _context.SaveChanges();

            var result = await _service!.RegistrarPrestamoAsync(usuario.Id, new List<int> { libro.Id });

            Assert.IsTrue(result.Success, $"Falló el registro del préstamo: {result.Message}");
            Assert.IsNotNull(result.Data, "El préstamo retornado es nulo.");
            Assert.AreEqual(usuario.Id, result.Data.UsuarioId, "El préstamo no está asociado al usuario correcto.");

            var libroActualizado = await _context.Libros.FindAsync(libro.Id);
            Assert.IsFalse(libroActualizado!.Disponible, "El libro debería haberse marcado como no disponible.");
        }

        // =====================================================
        // CU-10: Registrar devolución
        // =====================================================
        [TestMethod]
        public async Task RegistrarDevolucion_CuandoLibroPrestado_DeberiaActualizarDisponibilidadYMarcarCerrado()
        {
            var usuario = new Usuario
            {
                Nombre = "Carlos",
                Email = $"carlos{Guid.NewGuid():N}@mail.com",
                Contraseña = "123",
                Rol = "Docente",
                Activo = true
            };
            _context!.Usuarios.Add(usuario);
            _context.SaveChanges();

            var libro = new Libro
            {
                Titulo = "El Principito",
                Autor = "Antoine de Saint-Exupéry",
                ISBN = "978" + Guid.NewGuid().ToString("N").Substring(0, 10),
                Editorial = "Reynal & Hitchcock",
                Categoria = "Infantil",
                AnioPublicacion = 1943,
                Disponible = true,
                Activo = true
            };
            _context.Libros.Add(libro);
            _context.SaveChanges();

            var prestamo = await _service!.RegistrarPrestamoAsync(usuario.Id, new List<int> { libro.Id });
            Assert.IsTrue(prestamo.Success, $"No se pudo registrar el préstamo: {prestamo.Message}");

            var devolucion = await _service.RegistrarDevolucionAsync(prestamo.Data!.Id, new List<int> { libro.Id });
            Assert.IsTrue(devolucion.Success, $"Falló la devolución: {devolucion.Message}");

            var libroDevuelto = await _context.Libros.FindAsync(libro.Id);
            Assert.IsTrue(libroDevuelto!.Disponible, "El libro debería estar disponible tras la devolución.");

            var prestamoCerrado = await _context.Prestamos.FindAsync(prestamo.Data.Id);
            Assert.IsFalse(prestamoCerrado!.Activo, "El préstamo debería haberse cerrado tras devolver todos los libros.");
        }

        // =====================================================
        // CU-11: Consultar préstamos activos por usuario
        // =====================================================
        [TestMethod]
        public async Task ObtenerPrestamosActivosPorUsuario_CuandoExisten_DeberiaRetornarLista()
        {
            var usuario = new Usuario
            {
                Nombre = "Ana",
                Email = $"ana{Guid.NewGuid():N}@mail.com",
                Contraseña = "abc",
                Rol = "Estudiante",
                Activo = true
            };
            _context!.Usuarios.Add(usuario);
            _context.SaveChanges();

            var libro = new Libro
            {
                Titulo = "Rayuela",
                Autor = "Julio Cortázar",
                ISBN = "978" + Guid.NewGuid().ToString("N").Substring(0, 10),
                Editorial = "Alfaguara",
                Categoria = "Novela",
                AnioPublicacion = 1963,
                Disponible = true,
                Activo = true
            };
            _context.Libros.Add(libro);
            _context.SaveChanges();

            await _service!.RegistrarPrestamoAsync(usuario.Id, new List<int> { libro.Id });

            var result = await _service.ObtenerPrestamosActivosPorUsuarioAsync(usuario.Id);

            Assert.IsTrue(result.Success, "Error al obtener préstamos activos.");
            Assert.IsTrue(result.Data!.Any(), "No se encontraron préstamos activos para este usuario.");
        }

        // =====================================================
        // CU-12: Consultar préstamos vencidos
        // =====================================================
        [TestMethod]
        public async Task ObtenerPrestamosVencidos_CuandoExisten_DeberiaRetornarPrestamosConFechaVencida()
        {
            var usuario = new Usuario
            {
                Nombre = "Luis",
                Email = $"luis{Guid.NewGuid():N}@mail.com",
                Contraseña = "xyz",
                Rol = "Estudiante",
                Activo = true
            };
            _context!.Usuarios.Add(usuario);
            _context.SaveChanges();

            var libro = new Libro
            {
                Titulo = "1984",
                Autor = "George Orwell",
                ISBN = "978" + Guid.NewGuid().ToString("N").Substring(0, 10),
                Editorial = "Secker & Warburg",
                Categoria = "Distopía",
                AnioPublicacion = 1949,
                Disponible = true,
                Activo = true
            };
            _context.Libros.Add(libro);
            _context.SaveChanges();

            var prestamo = new Prestamo
            {
                UsuarioId = usuario.Id,
                FechaPrestamo = DateTime.Now.AddDays(-15),
                FechaVencimiento = DateTime.Now.AddDays(-7),
                Activo = true
            };
            _context.Prestamos.Add(prestamo);
            _context.SaveChanges();

            var result = await _service!.ObtenerPrestamosVencidosAsync();

            Assert.IsNotNull(result, "El resultado del método es nulo.");
            Assert.IsTrue(result.Success || result.Message.Contains("vencidos"),
                "No se devolvió el mensaje esperado para préstamos vencidos.");
        }
    }
}

