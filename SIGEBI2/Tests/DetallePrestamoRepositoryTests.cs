using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    /// <summary>
    /// PRUEBAS UNITARIAS: DetallePrestamoRepositoryTests
    /// CAPA: Persistencia
    /// MÓDULO: Préstamos y Devoluciones
    /// DESCRIPCIÓN:
    /// Valida las operaciones principales del repositorio de DetallePrestamo:
    /// - Obtener detalles por préstamo (CU-10, CU-11)
    /// - Registrar devolución (CU-10)
    /// </summary>
    [TestClass]
    public class DetallePrestamoRepositoryTests
    {
        private AppDbContext? _context;
        private DetallePrestamoRepository? _repository;

        /// <summary>
        /// Configuración inicial antes de cada prueba.
        /// Se limpia la base de datos y se reinician las identidades.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=.;Database=SIGEBI_Test;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            _context = new AppDbContext(options);

            // Limpieza completa (orden de FK)
            _context.Database.ExecuteSqlRaw("DELETE FROM DetallePrestamos;");
            _context.Database.ExecuteSqlRaw("DELETE FROM Prestamos;");
            _context.Database.ExecuteSqlRaw("DELETE FROM Libros;");
            _context.Database.ExecuteSqlRaw("DELETE FROM Usuarios;");

            // Reinicio de contadores IDENTITY
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Usuarios', RESEED, 0);");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Libros', RESEED, 0);");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Prestamos', RESEED, 0);");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('DetallePrestamos', RESEED, 0);");

            _repository = new DetallePrestamoRepository(_context);
        }

        // ============================================================
        // TEST 1 — CU-10 / CU-11: Obtener detalles de préstamo
        // ============================================================
        [TestMethod]
        public async Task ObtenerPorPrestamo_CuandoExistePrestamo_DeberiaRetornarDetallesCorrectos()
        {
            // Arrange — Crear datos base
            var usuario = new Usuario
            {
                Nombre = "Usuario de prueba",
                Email = $"test{Guid.NewGuid():N}@mail.com",
                Contraseña = "123",
                Rol = "Estudiante",
                Activo = true
            };
            _context!.Usuarios.Add(usuario);
            _context.SaveChanges();

            var libro = new Libro
            {
                Titulo = "Libro de prueba",
                Autor = "Autor Prueba",
                ISBN = "978" + Guid.NewGuid().ToString("N").Substring(0, 10),
                Editorial = "Editorial A",
                Categoria = "Ficción",
                AnioPublicacion = 2024,
                Disponible = true,
                Activo = true
            };
            _context.Libros.Add(libro);
            _context.SaveChanges();

            var prestamo = new Prestamo
            {
                UsuarioId = usuario.Id,
                FechaPrestamo = DateTime.Now,
                FechaVencimiento = DateTime.Now.AddDays(7),
                Activo = true
            };
            _context.Prestamos.Add(prestamo);
            _context.SaveChanges();

            var detalle = new DetallePrestamo
            {
                PrestamoId = prestamo.Id,
                LibroId = libro.Id,
                Devuelto = false,
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            _context.DetallePrestamos.Add(detalle);
            _context.SaveChanges();

            // Act — Ejecutar método del repositorio
            var result = await _repository!.ObtenerPorPrestamoAsync(prestamo.Id);

            // Assert — Validar resultados
            Assert.IsNotNull(result, "El método devolvió un resultado nulo.");
            Assert.IsTrue(result.Any(), "No se devolvieron registros de detalle.");
            Assert.AreEqual(prestamo.Id, result.First().PrestamoId, "El detalle no pertenece al préstamo esperado.");
        }

        // ============================================================
        // TEST 2 — CU-10: Registrar devolución
        // ============================================================
        [TestMethod]
        public async Task RegistrarDevolucion_CuandoDetalleExistente_DeberiaMarcarComoDevuelto()
        {
            // Arrange — Crear datos base
            var usuario = new Usuario
            {
                Nombre = "Usuario devolución",
                Email = $"user{Guid.NewGuid():N}@mail.com",
                Contraseña = "abc",
                Rol = "Docente",
                Activo = true
            };
            _context!.Usuarios.Add(usuario);
            _context.SaveChanges();

            var libro = new Libro
            {
                Titulo = "Libro devolución",
                Autor = "Autor Test",
                ISBN = "978" + Guid.NewGuid().ToString("N").Substring(0, 10),
                Editorial = "Alfaguara",
                Categoria = "Ficción",
                AnioPublicacion = 2024,
                Disponible = true,
                Activo = true
            };
            _context.Libros.Add(libro);
            _context.SaveChanges();

            var prestamo = new Prestamo
            {
                UsuarioId = usuario.Id,
                FechaPrestamo = DateTime.Now,
                FechaVencimiento = DateTime.Now.AddDays(7),
                Activo = true
            };
            _context.Prestamos.Add(prestamo);
            _context.SaveChanges();

            var detalle = new DetallePrestamo
            {
                PrestamoId = prestamo.Id,
                LibroId = libro.Id,
                Devuelto = false,
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            _context.DetallePrestamos.Add(detalle);
            _context.SaveChanges();

            // Act — Registrar devolución en el repositorio
            await _repository!.RegistrarDevolucionAsync(detalle.Id);

            // Assert — Validar que el registro fue actualizado
            var actualizado = await _context.DetallePrestamos.FindAsync(detalle.Id);
            Assert.IsNotNull(actualizado, "No se encontró el detalle tras la devolución.");
            Assert.IsTrue(actualizado!.Devuelto, "El campo 'Devuelto' no fue marcado como verdadero.");
            Assert.IsNotNull(actualizado.FechaDevolucion, "No se asignó la fecha de devolución correctamente.");
        }
    }
}
