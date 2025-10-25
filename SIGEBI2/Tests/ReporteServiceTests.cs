using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class ReporteServiceTests
    {
        private AppDbContext? _context;
        private ReporteService? _service;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=.;Database=SIGEBI_Test;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            _context = new AppDbContext(options);

            // Limpiar tablas relacionadas
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

        // CU-15: Generar reporte de libros más prestados
        [TestMethod]
        public async Task ObtenerLibrosMasPrestados_DeberiaRetornarListaDeLibrosOrdenada()
        {
            // Crear usuario
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
            var libro1 = new Libro { Titulo = "Libro A", Autor = "Autor A", ISBN = "978" + Guid.NewGuid().ToString("N").Substring(0, 10), Disponible = true, Activo = true, AnioPublicacion = 2020 };
            var libro2 = new Libro { Titulo = "Libro B", Autor = "Autor B", ISBN = "978" + Guid.NewGuid().ToString("N").Substring(0, 10), Disponible = true, Activo = true, AnioPublicacion = 2021 };
            _context.Libros.AddRange(libro1, libro2);
            _context.SaveChanges();

            // Crear préstamos con más detalles para Libro A
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
                    LibroId = libro1.Id,
                    Devuelto = false,
                    Activo = true,
                    FechaCreacion = DateTime.Now
                };
                _context.DetallePrestamos.Add(detalle);
            }

            // Un solo préstamo para Libro B
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
                LibroId = libro2.Id,
                Devuelto = false,
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            _context.DetallePrestamos.Add(detalleB);
            _context.SaveChanges();

            // Ejecutar el reporte
            var result = await _service!.ObtenerLibrosMasPrestadosAsync();

            Assert.IsTrue(result.Success, "El reporte no se generó correctamente.");
            Assert.IsTrue(result.Data!.Any(), "La lista de libros está vacía.");

            var top = result.Data!.First();
            Assert.AreEqual("Libro A", top.Titulo, "El libro más prestado no es el esperado.");
            Assert.AreEqual(3, top.CantidadPrestamos, "La cantidad de préstamos no es correcta.");
        }

        [TestMethod]
        public async Task ObtenerLibrosMasPrestados_DeberiaRetornarFailSiNoHayDatos()
        {
            var result = await _service!.ObtenerLibrosMasPrestadosAsync();
            Assert.IsFalse(result.Success);
            Assert.AreEqual("No hay préstamos registrados para generar el reporte.", result.Message);
        }
    }
}
