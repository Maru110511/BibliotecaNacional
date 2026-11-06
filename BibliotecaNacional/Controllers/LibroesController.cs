using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaNacional.Models;
using BibliotecaNacional.Data;

namespace BibliotecaNacional.Controllers
{
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Libros
        // Muestra los libros y permite buscar y filtrar
        public async Task<IActionResult> Index(
            string buscar,
            string categoria,
            string estado)
        {
            var libros = _context.Libros.AsQueryable();

            // Buscar por título o autor
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                libros = libros.Where(l =>
                    l.Titulo.Contains(buscar) ||
                    l.Autor.Contains(buscar));
            }

            // Filtrar por categoría
            if (!string.IsNullOrWhiteSpace(categoria))
            {
                libros = libros.Where(l =>
                    l.Categoria == categoria);
            }

            // Filtrar por estado
            if (!string.IsNullOrWhiteSpace(estado))
            {
                libros = libros.Where(l =>
                    l.Estado == estado);
            }

            // Obtener categorías existentes
            ViewBag.Categorias = await _context.Libros
                .Select(l => l.Categoria)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            // Obtener estados existentes
            ViewBag.Estados = await _context.Libros
                .Select(l => l.Estado)
                .Distinct()
                .OrderBy(e => e)
                .ToListAsync();

            // Cantidad total de libros
            ViewBag.TotalLibros = await _context.Libros
                .CountAsync();

            // Cantidad de resultados encontrados
            ViewBag.TotalResultados = await libros
                .CountAsync();

            // Resumen agrupado por categoría
            ViewBag.ResumenCategorias = await _context.Libros
                .GroupBy(l => l.Categoria)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Cantidad = g.Count()
                })
                .OrderBy(x => x.Categoria)
                .ToListAsync();

            // Guardar valores seleccionados
            ViewBag.Buscar = buscar;
            ViewBag.CategoriaSeleccionada = categoria;
            ViewBag.EstadoSeleccionado = estado;

            // Enviar resultados a la vista
            return View(await libros.ToListAsync());
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Titulo,Autor,Categoria,AnioPublicacion,Estado")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] =
                    "El libro se registró correctamente.";

                return RedirectToAction(nameof(Index));
            }

            return View(libro);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FindAsync(id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Titulo,Autor,Categoria,AnioPublicacion,Estado")] Libro libro)
        {
            if (id != libro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);

                    await _context.SaveChangesAsync();

                    TempData["Mensaje"] =
                        "El libro se actualizó correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(libro);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros
                .FindAsync(id);

            if (libro != null)
            {
                _context.Libros.Remove(libro);

                await _context.SaveChangesAsync();

                TempData["Mensaje"] =
                    "El libro se eliminó correctamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Verifica si existe un libro
        private bool LibroExists(int id)
        {
            return _context.Libros
                .Any(e => e.Id == id);
        }
    }
}