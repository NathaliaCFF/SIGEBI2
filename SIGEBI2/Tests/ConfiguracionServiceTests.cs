using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class ConfiguracionServiceTests
    {
        private AppDbContext? _context;
        private ConfiguracionService? _service;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=.;Database=SIGEBI_Test;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            _context = new AppDbContext(options);

            // Limpiar datos previos
            _context.Database.ExecuteSqlRaw("DELETE FROM Configuraciones;");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Configuraciones', RESEED, 0);");

            var repo = new ConfigurationRepository(_context);
            _service = new ConfiguracionService(repo);
        }

        // CU-14: Configurar duración estándar de préstamos
        [TestMethod]
        public async Task ActualizarDuracionPrestamoDias_DeberiaActualizarCorrectamente()
        {
            // Insertar registro inicial
            var config = new Configuration { DuracionPrestamoDias = 7 };
            _context!.Configuraciones.Add(config);
            _context.SaveChanges();

            // Ejecutar actualización
            var result = await _service!.ActualizarDuracionPrestamoDiasAsync(14);

            Assert.IsTrue(result.Success, $"Error al actualizar: {result.Message}");
            var actualizada = await _context.Configuraciones.FirstOrDefaultAsync();
            Assert.AreEqual(14, actualizada!.DuracionPrestamoDias);
        }

        [TestMethod]
        public async Task ActualizarDuracionPrestamoDias_DeberiaFallarConValorInvalido()
        {
            var config = new Configuration { DuracionPrestamoDias = 7 };
            _context!.Configuraciones.Add(config);
            _context.SaveChanges();

            var result = await _service!.ActualizarDuracionPrestamoDiasAsync(0);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("El valor ingresado debe ser mayor que cero.", result.Message);
        }

        [TestMethod]
        public async Task ObtenerConfiguracion_DeberiaRetornarConfiguracionActual()
        {
            var config = new Configuration { DuracionPrestamoDias = 10 };
            _context!.Configuraciones.Add(config);
            _context.SaveChanges();

            var result = await _service!.ObtenerConfiguracionAsync();

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(10, result.Data!.DuracionPrestamoDias);
        }
    }
}

