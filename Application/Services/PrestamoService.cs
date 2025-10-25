using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.Application.Services
{
    // ============================================================================
    // SERVICIO: PrestamoService
    // MÓDULO: Préstamos y Devoluciones
    // DESCRIPCIÓN: Contiene la lógica de aplicación encargada de gestionar los
    // préstamos, devoluciones y consultas relacionadas con la entidad Préstamo.
    // CASOS DE USO RELACIONADOS:
    //   - CU-09: Registrar préstamo
    //   - CU-10: Registrar devolución
    //   - CU-11: Consultar préstamos activos por usuario
    //   - CU-12: Identificar préstamos vencidos
    // CAPA: Aplicación
    // ============================================================================
    public class PrestamoService : IPrestamoService
    {
        private readonly IPrestamoRepository _prestamoRepo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly ILibroRepository _libroRepo;
        private readonly IDetallePrestamoRepository _detalleRepo;

        public PrestamoService(
            IPrestamoRepository prestamoRepo,
            IUsuarioRepository usuarioRepo,
            ILibroRepository libroRepo,
            IDetallePrestamoRepository detalleRepo)
        {
            _prestamoRepo = prestamoRepo;
            _usuarioRepo = usuarioRepo;
            _libroRepo = libroRepo;
            _detalleRepo = detalleRepo;
        }

        // ============================================================================
        // CASO DE USO: CU-09 - Registrar préstamo
        // DESCRIPCIÓN: Permite al Administrador registrar un préstamo para un usuario
        // activo, validando la disponibilidad de los libros seleccionados y creando los
        // registros correspondientes en las tablas Prestamo y DetallePrestamo.
        // ============================================================================
        public async Task<ServiceResult<Prestamo>> RegistrarPrestamoAsync(int usuarioId, List<int> librosIds)
        {
            // CU-09: Validar usuario existente y activo
            var usuario = await _usuarioRepo.GetByIdAsync(usuarioId);
            if (!usuario.Success || usuario.Data == null)
                return ServiceResult<Prestamo>.Fail("El usuario no existe.");

            if (!usuario.Data.Activo)
                return ServiceResult<Prestamo>.Fail("El usuario está inactivo y no puede realizar préstamos.");

            // CU-09: Validar disponibilidad de libros
            var libros = new List<Libro>();
            foreach (var id in librosIds)
            {
                var libro = await _libroRepo.GetByIdAsync(id);
                if (!libro.Success || libro.Data == null)
                    return ServiceResult<Prestamo>.Fail($"El libro con ID {id} no existe.");

                if (!libro.Data.Disponible)
                    return ServiceResult<Prestamo>.Fail($"El libro '{libro.Data.Titulo}' no está disponible.");

                libros.Add(libro.Data);
            }

            // CU-09: Crear préstamo principal
            var prestamo = new Prestamo
            {
                UsuarioId = usuarioId,
                FechaPrestamo = DateTime.Now,
                FechaVencimiento = DateTime.Now.AddDays(7) // configurable en módulo Configuración (CU-14)
            };

            var creado = await _prestamoRepo.AddAsync(prestamo);
            if (!creado.Success || creado.Data == null)
                return ServiceResult<Prestamo>.Fail("No se pudo registrar el préstamo en la base de datos.");

            // CU-09: Registrar detalles del préstamo y actualizar disponibilidad
            foreach (var libro in libros)
            {
                libro.Disponible = false;
                await _libroRepo.UpdateAsync(libro);

                var detalle = new DetallePrestamo
                {
                    PrestamoId = creado.Data.Id,
                    LibroId = libro.Id,
                    FechaDevolucion = null,
                    Devuelto = false
                };

                await _detalleRepo.AddAsync(detalle);
            }

            return ServiceResult<Prestamo>.Ok(creado.Data, "Préstamo registrado correctamente.");
        }

        // ============================================================================
        // CASO DE USO: CU-10 - Registrar devolución
        // DESCRIPCIÓN: Permite registrar la devolución parcial o total de los libros
        // asociados a un préstamo, actualizando su estado en DetallePrestamo y
        // restableciendo la disponibilidad de los libros.
        // ============================================================================
        public async Task<ServiceResult> RegistrarDevolucionAsync(int prestamoId, List<int> librosIds)
        {
            var prestamo = await _prestamoRepo.GetByIdAsync(prestamoId);
            if (!prestamo.Success || prestamo.Data == null)
                return ServiceResult.Fail("El préstamo no existe.");

            foreach (var libroId in librosIds)
            {
                var detalleResult = await _detalleRepo.FindAsync(
                    d => d.PrestamoId == prestamoId && d.LibroId == libroId && d.FechaDevolucion == null);

                var detalle = detalleResult.Data?.FirstOrDefault();
                if (detalle == null)
                    continue;

                // CU-10: Marcar libro como devuelto
                detalle.FechaDevolucion = DateTime.Now;
                detalle.Devuelto = true;
                await _detalleRepo.UpdateAsync(detalle);

                // CU-10: Cambiar libro a disponible
                var libro = (await _libroRepo.GetByIdAsync(libroId)).Data;
                if (libro != null)
                {
                    libro.Disponible = true;
                    await _libroRepo.UpdateAsync(libro);
                }
            }

            // CU-10: Cerrar el préstamo si todos los libros fueron devueltos
            var pendientes = await _detalleRepo.FindAsync(
                d => d.PrestamoId == prestamoId && d.FechaDevolucion == null);

            if (!pendientes.Data!.Any())
            {
                prestamo.Data!.Activo = false;
                prestamo.Data!.FechaModificacion = DateTime.Now;
                await _prestamoRepo.UpdateAsync(prestamo.Data!);
            }

            return ServiceResult.Ok("Devolución registrada correctamente.");
        }

        // ============================================================================
        // CASO DE USO: CU-11 - Consultar préstamos activos por usuario
        // DESCRIPCIÓN: Permite al usuario autenticado (docente o estudiante)
        // consultar los préstamos que tiene activos junto con sus detalles.
        // ============================================================================
        public async Task<ServiceResult<IEnumerable<Prestamo>>> ObtenerPrestamosActivosPorUsuarioAsync(int usuarioId)
        {
            var prestamos = await _prestamoRepo.ObtenerPorUsuarioAsync(usuarioId);
            if (!prestamos.Any())
                return ServiceResult<IEnumerable<Prestamo>>.Fail("No hay préstamos activos para este usuario.");

            return ServiceResult<IEnumerable<Prestamo>>.Ok(prestamos, "Listado de préstamos activos obtenido correctamente.");
        }

        // ============================================================================
        // CASO DE USO: CU-12 - Identificar préstamos vencidos
        // DESCRIPCIÓN: Permite al Administrador identificar los préstamos cuyo
        // período de vencimiento ha expirado, mostrando los datos del usuario y
        // los libros asociados.
        // ============================================================================
        public async Task<ServiceResult<IEnumerable<Prestamo>>> ObtenerPrestamosVencidosAsync()
        {
            var vencidos = await _prestamoRepo.ObtenerPrestamosVencidosAsync();
            if (!vencidos.Any())
                return ServiceResult<IEnumerable<Prestamo>>.Fail("No existen préstamos vencidos.");

            return ServiceResult<IEnumerable<Prestamo>>.Ok(vencidos, "Listado de préstamos vencidos obtenido correctamente.");
        }
    }
}
