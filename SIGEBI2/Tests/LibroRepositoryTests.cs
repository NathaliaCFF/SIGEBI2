using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class LibroRepositoryTests
    {
        private AppDbContext? _context;
        private LibroRepository? _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=.;Database=SIGEBI_Test;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;


            _context = new AppDbContext(options);
            _repository = new LibroRepository(_context);

            
            _context.Database.ExecuteSqlRaw("DELETE FROM Libros;");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Libros', RESEED, 0);");
        }

        // CU-01 Registrar libro
        [TestMethod]
        public async Task CrearLibro_DeberiaAgregarLibro()
        {
            var libro = new Libro
            {
                Titulo = "Cien años de soledad",
                Autor = "Gabriel García Márquez",
                ISBN = "9780060883287",
                Editorial = "Editorial Sudamericana",
                Categoria = "Realismo mágico",
                AnioPublicacion = 1967,
                Disponible = true,
                Activo = true
            };

            try
            {
                var result = await _repository!.AddAsync(libro);

                Assert.IsTrue(result.Success, $"❌ La operación de agregado falló. {result.Message}");
                Assert.IsNotNull(result.Data, "El resultado del agregado es nulo.");
                Assert.AreEqual("Cien años de soledad", result.Data!.Titulo);
            }
            catch (DbUpdateException dbEx)
            {
                // Captura directa de errores SQL desde EF Core
                var sqlEx = dbEx.InnerException ?? dbEx;
                Assert.Fail($"❌ Error SQL: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                var inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;

                Assert.Fail($"❌ Error general: {inner.Message}");
            }

        }



        // CU-02 Editar libro
        [TestMethod]
        public async Task ActualizarLibro_DeberiaModificarCampos()
        {
            var libro = new Libro
            {
                Titulo = "El principito",
                Autor = "Antoine de Saint-Exupéry",
                ISBN = "9780156012195",
                Editorial = "Reynal & Hitchcock",
                Categoria = "Infantil",
                AnioPublicacion = 1943,
                Disponible = true
            };

            var creado = await _repository!.AddAsync(libro);
            Assert.IsTrue(creado.Success, $"Error al crear libro inicial. {creado.Message}");

            creado.Data!.Titulo = "El Principito (Edición Revisada)";
            var actualizado = await _repository.UpdateAsync(creado.Data);

            Assert.IsTrue(actualizado.Success, $"La actualización falló. {actualizado.Message}");
            Assert.AreEqual("El Principito (Edición Revisada)", actualizado.Data!.Titulo);
        }

        // CU-03 Buscar libro
        [TestMethod]
        public async Task BuscarLibro_DeberiaRetornarCoincidencias()
        {
            var libro = new Libro
            {
                Titulo = "Don Quijote de la Mancha",
                Autor = "Miguel de Cervantes",
                ISBN = "9788491050295",
                Editorial = "Real Academia Española",
                Categoria = "Clásico",
                AnioPublicacion = 1605,
                Disponible = true
            };

            var creado = await _repository!.AddAsync(libro);
            Assert.IsTrue(creado.Success, $"Error al insertar libro de prueba. {creado.Message}");

            var resultados = await _repository.BuscarPorTituloOAutorAsync("Quijote");

            Assert.IsTrue(resultados.Any(), "No se encontraron coincidencias en la búsqueda.");
        }

        // CU-04 Consultar disponibilidad
        [TestMethod]
        public async Task EstaDisponible_DeberiaRetornarTruePorDefecto()
        {
            var libro = new Libro
            {
                Titulo = "Rayuela",
                Autor = "Julio Cortázar",
                ISBN = "9788497592201",
                Editorial = "Alfaguara",
                Categoria = "Novela",
                AnioPublicacion = 1963,
                Disponible = true
            };

            var creado = await _repository!.AddAsync(libro);
            Assert.IsTrue(creado.Success, $"Error al crear libro para verificar disponibilidad. {creado.Message}");

            var disponible = await _repository.EstaDisponibleAsync(creado.Data!.Id);

            Assert.IsTrue(disponible, "El libro debería estar disponible por defecto.");
        }

        // CU-04 (extensión) Cambiar disponibilidad
        [TestMethod]
        public async Task CambiarDisponibilidad_DeberiaActualizarEstado()
        {
            var libro = new Libro
            {
                Titulo = "Fahrenheit 451",
                Autor = "Ray Bradbury",
                ISBN = "9788497594250",
                Editorial = "Minotauro",
                Categoria = "Ciencia Ficción",
                AnioPublicacion = 1953,
                Disponible = true
            };

            var creado = await _repository!.AddAsync(libro);
            Assert.IsTrue(creado.Success, $"Error al crear libro de prueba. {creado.Message}");

            var cambio = await _repository.CambiarDisponibilidadAsync(creado.Data!.Id, false);

            Assert.IsTrue(cambio, "No se pudo cambiar la disponibilidad del libro.");

            var disponible = await _repository.EstaDisponibleAsync(creado.Data.Id);
            Assert.IsFalse(disponible, "El libro debería estar marcado como no disponible.");
        }

        // CU-05 Desactivar libro (soft delete)
        [TestMethod]
        public async Task EliminarLibro_DeberiaDesactivarEntidad()
        {
            var libro = new Libro
            {
                Titulo = "1984",
                Autor = "George Orwell",
                ISBN = "9780451524935",
                Editorial = "Secker & Warburg",
                Categoria = "Distopía",
                AnioPublicacion = 1949,
                Disponible = true
            };

            var creado = await _repository!.AddAsync(libro);
            Assert.IsTrue(creado.Success, $"Error al crear libro para eliminar. {creado.Message}");

            var eliminado = await _repository.DeleteAsync(creado.Data!.Id);
            Assert.IsTrue(eliminado.Success, $"La eliminación (desactivación) falló. {eliminado.Message}");

            var encontrado = await _repository.GetByIdAsync(creado.Data.Id);
            Assert.IsTrue(encontrado.Success, "No se pudo recuperar el libro después de eliminar.");
            Assert.IsFalse(encontrado.Data!.Activo, "El libro no fue marcado como inactivo.");
        }
    }
}
