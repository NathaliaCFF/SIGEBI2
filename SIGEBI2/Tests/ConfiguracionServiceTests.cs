using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using System.Threading.Tasks;

namespace Tests
{
    /// <summary>
    /// PRUEBAS UNITARIAS: ConfiguracionServiceTests
    /// CAPA: Aplicación
    /// MÓDULO: Administración / Configuración del Sistema
    /// DESCRIPCIÓN:
    /// Valida las operaciones principales del servicio de configuración:
    /// - Actualizar duración estándar de préstamo (CU-14)
    /// - Obtener configuración actual del sistema
    /// </summary>
    [TestClass]
    public class ConfiguracionServiceTests
    {
        private AppDbContext? _context;
        private ConfiguracionService? _service;

        /// <summary>
        /// Inicializa un entorno limpio antes de cada prueba.
        /// Se elimina la tabla Configuraciones y se reinicia su contador IDENTITY.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=.;Database=SIGEBI_Test;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            _context = new AppDbContext(options);

            // Limpieza de datos previos
            _context.Database.ExecuteSqlRaw("DELETE FROM Configuraciones;");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Configuraciones', RESEED, 0);");

            var repo = new ConfigurationRepository(_context);
            _service = new ConfiguracionService(repo);
        }

        // ============================================================
        // CU-14: Actualizar duración estándar de préstamo
        // ============================================================
        [TestMethod]
        public async Task ActualizarDuracionPrestamoDias_CuandoValorValido_DeberiaActualizarCorrectamente()
        {
            // Arrange — Insertar registro inicial
            var config = new Configuration { DuracionPrestamoDias = 7 };
            _context!.Configuraciones.Add(config);
            _context.SaveChanges();

            // Act — Ejecutar la actualización
            var result = await _service!.ActualizarDuracionPrestamoDiasAsync(14);

            // Assert — Validar resultados
            Assert.IsTrue(result.Success, $"Error al actualizar duración: {result.Message}");
            var actualizada = await _context.Configuraciones.FirstOrDefaultAsync();
            Assert.IsNotNull(actualizada, "No se encontró la configuración actualizada en la base de datos.");
            Assert.AreEqual(14, actualizada!.DuracionPrestamoDias, "La duración no fue actualizada correctamente.");
        }

        // ============================================================
        // CU-14: Validar valor inválido (negativo o cero)
        // ============================================================
        [TestMethod]
        public async Task ActualizarDuracionPrestamoDias_CuandoValorInvalido_DeberiaFallar()
        {
            // Arrange
            var config = new Configuration { DuracionPrestamoDias = 7 };
            _context!.Configuraciones.Add(config);
            _context.SaveChanges();

            // Act
            var result = await _service!.ActualizarDuracionPrestamoDiasAsync(0);

            // Assert
            Assert.IsFalse(result.Success, " El método debería haber fallado con valor 0.");
            Assert.AreEqual("La duración de préstamo debe ser mayor que 0 días.", result.Message);
        }

        // ============================================================
        // CU-14: Obtener configuración actual
        // ============================================================
        [TestMethod]
        public async Task ObtenerConfiguracion_CuandoExisteRegistro_DeberiaRetornarConfiguracionActual()
        {
            // Arrange
            var config = new Configuration { DuracionPrestamoDias = 10 };
            _context!.Configuraciones.Add(config);
            _context.SaveChanges();

            // Act
            var result = await _service!.ObtenerConfiguracionAsync();

            // Assert
            Assert.IsTrue(result.Success, $"No se pudo obtener la configuración: {result.Message}");
            Assert.IsNotNull(result.Data, "No se devolvió ningún registro de configuración.");
            Assert.AreEqual(10, result.Data!.DuracionPrestamoDias, "El valor de duración obtenido no coincide con el registrado.");
        }
    }
}
