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
    [TestClass]
    public class DetallePrestamoRepositoryTests
    {
        private AppDbContext? _context;
        private DetallePrestamoRepository? _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=.;Database=SIGEBI_Test;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            _context = new AppDbContext(options);

            // Limpiar tablas relacionadas antes de iniciar
            _context.Database.ExecuteSqlRaw("DELETE FROM DetallePrestamos;");
            _context.Database.ExecuteSqlRaw("DELETE FROM Prestamos;");
            _context.Database.ExecuteSqlRaw("DELETE FROM Libros;");
            _context.Database.ExecuteSqlRaw("DELETE FROM Usuarios;");

            // Reiniciar identidad
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Usuarios', RESEED, 0);");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Libros', RESEED, 0);");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Prestamos', RESEED, 0);");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('DetallePrestamos', RESEED, 0);");

            _repository = new DetallePrestamoRepository(_context);
        }

        // ============================================================
        // Test 1: ObtenerPorPrestamoAsync()
        // ============================================================
        [TestMethod]
        public async Task ObtenerPorPrestamo_DeberiaRetornarDetallesCorrectos()
        {
            // Crear datos base
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

            // Ejecutar método del repositorio
            var result = await _repository!.ObtenerPorPrestamoAsync(prestamo.Id);

            // Validaciones
            Assert.IsNotNull(result, "No se devolvió ningún resultado.");
            Assert.IsTrue(result.Any(), "La lista de detalles está vacía.");
            Assert.AreEqual(prestamo.Id, result.First().PrestamoId, "El detalle no pertenece al préstamo correcto.");
        }

        // ============================================================
        // Test 2: RegistrarDevolucionAsync()
        // ============================================================
        [TestMethod]
        public async Task RegistrarDevolucion_DeberiaActualizarDetalleComoDevuelto()
        {
            // Crear datos base
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

            // Ejecutar método del repositorio
            await _repository!.RegistrarDevolucionAsync(detalle.Id);

            // Verificar cambios
            var actualizado = await _context.DetallePrestamos.FindAsync(detalle.Id);
            Assert.IsNotNull(actualizado, "No se encontró el detalle actualizado.");
            Assert.IsTrue(actualizado!.Devuelto, "El campo 'Devuelto' no fue actualizado.");
            Assert.IsNotNull(actualizado.FechaDevolucion, "No se asignó la fecha de devolución.");
        }
    }
}
