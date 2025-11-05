using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using Application.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    /// <summary>
    /// PRUEBAS UNITARIAS: ReporteServiceTests
    /// CAPA: Aplicación
    /// MÓDULO: Reportes
    /// DESCRIPCIÓN:
    /// Valida la generación del reporte de libros más prestados (CU-13),
    /// verificando el orden, conteo y manejo de escenarios sin datos.
    /// </summary>
    [TestClass]
    public class ReporteServiceTests
    {
        private AppDbContext? _context;
        private ReporteService? _service;

        /// <summary>
        /// Configuración inicial antes de cada prueba:
        /// limpia todas las tablas relacionadas con préstamos y reinicia sus identidades.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=.;Database=SIGEBI_Test;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            _context = new AppDbContext(options);

            // Limpieza y reinicio de identidad
            _context.Database.ExecuteSqlRaw("DELETE FROM DetallePrestamos;");
            _context.Database.ExecuteSqlRaw("DELETE FROM Prestamos;");
            _context.Database.ExecuteSqlRaw("DELETE FROM Libros;");
            _context.Database.ExecuteSqlRaw("DELETE FROM Usuarios;");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Usuarios', RESEED, 0);");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Libros', RESEED, 0);");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Prestamos', RESEED, 0);");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('DetallePrestamos', RESEED, 0);");

            var repo = new ReporteRepository(_context);
            _service = new ReporteService(repo);
        }

        // ============================================================
        // CU-13: Generar reporte de libros más prestados
        // ============================================================
        [TestMethod]
        public async Task ObtenerLibrosMasPrestados_CuandoExistenPrestamos_DeberiaRetornarListaOrdenada()
        {
            // ---------- Arrange ----------
            // Crear usuario base
            var usuario = new Usuario
            {
                Nombre = "Usuario Reporte",
                Email = $"reporte{Guid.NewGuid():N}@mail.com",
                Contraseña = "123",
                Rol = "Estudiante",
                Activo = true
            };
            _context!.Usuarios.Add(usuario);
            _context.SaveChanges();

            // Crear libros
            var libroA = new Libro
            {
                Titulo = "Libro A",
                Autor = "Autor A",
                ISBN = "978" + Guid.NewGuid().ToString("N").Substring(0, 10),
                AnioPublicacion = 2020,
                Disponible = true,
                Activo = true
            };
            var libroB = new Libro
            {
                Titulo = "Libro B",
                Autor = "Autor B",
                ISBN = "978" + Guid.NewGuid().ToString("N").Substring(0, 10),
                AnioPublicacion = 2021,
                Disponible = true,
                Activo = true
            };
            _context.Libros.AddRange(libroA, libroB);
            _context.SaveChanges();

            // Registrar tres préstamos para Libro A
            for (int i = 0; i < 3; i++)
            {
                var prestamo = new Prestamo
                {
                    UsuarioId = usuario.Id,
                    FechaPrestamo = DateTime.Now.AddDays(-i),
                    FechaVencimiento = DateTime.Now.AddDays(7),
                    Activo = true
                };
                _context.Prestamos.Add(prestamo);
                _context.SaveChanges();

                var detalle = new DetallePrestamo
                {
                    PrestamoId = prestamo.Id,
                    LibroId = libroA.Id,
                    Devuelto = false,
                    Activo = true,
                    FechaCreacion = DateTime.Now
                };
                _context.DetallePrestamos.Add(detalle);
            }

            // Un préstamo para Libro B
            var prestamoB = new Prestamo
            {
                UsuarioId = usuario.Id,
                FechaPrestamo = DateTime.Now.AddDays(-1),
                FechaVencimiento = DateTime.Now.AddDays(7),
                Activo = true
            };
            _context.Prestamos.Add(prestamoB);
            _context.SaveChanges();

            var detalleB = new DetallePrestamo
            {
                PrestamoId = prestamoB.Id,
                LibroId = libroB.Id,
                Devuelto = false,
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            _context.DetallePrestamos.Add(detalleB);
            _context.SaveChanges();

            // ---------- Act ----------
            var result = await _service!.ObtenerLibrosMasPrestadosAsync();

            // ---------- Assert ----------
            Assert.IsTrue(result.Success, $"El reporte no se generó correctamente: {result.Message}");
            Assert.IsNotNull(result.Data, "El resultado devuelto es nulo.");
            Assert.IsTrue(result.Data!.Any(), "La lista de libros más prestados está vacía.");

            var top = result.Data!.First();
            Assert.AreEqual("Libro A", top.Titulo, "El libro más prestado no coincide con el esperado.");
            Assert.AreEqual(3, top.CantidadPrestamos, "El número de préstamos reportado no es correcto.");
        }

        // ============================================================
        // CU-13: Escenario sin datos — reporte vacío
        // ============================================================
        [TestMethod]
        public async Task ObtenerLibrosMasPrestados_CuandoNoExistenPrestamos_DeberiaRetornarFail()
        {
            // ---------- Act ----------
            var result = await _service!.ObtenerLibrosMasPrestadosAsync();

            // ---------- Assert ----------
            Assert.IsFalse(result.Success, "El método debería devolver Fail cuando no hay préstamos.");
            Assert.AreEqual("No hay préstamos registrados para generar el reporte.", result.Message);
        }
    }
}
