using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class TodoListController : Controller
    {
        private readonly TodoListDbContext _db;
        private readonly ILogger<TodoListController> _logger;

        public TodoListController(TodoListDbContext db, ILogger<TodoListController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: TodoListController
        public async Task<ActionResult> Index()
        {
            var lists = await _db.TodoLists.AsNoTracking().ToListAsync();
            return View(lists);
        }

        // GET: TodoListController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var list = await _db.TodoLists
                .Include(x => x.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (list == null)
                return NotFound();

            return View(list);
        }

        // GET: TodoListController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TodoListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Title,Description")] TodoList model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.CreatedAt = System.DateTimeOffset.UtcNow;
            _db.TodoLists.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: TodoListController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var list = await _db.TodoLists.FindAsync(id);
            if (list == null)
                return NotFound();
            return View(list);
        }

        // POST: TodoListController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Title,Description")] TodoList model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var entity = await _db.TodoLists.FindAsync(id);
            if (entity == null)
                return NotFound();

            entity.Title = model.Title;
            entity.Description = model.Description;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: TodoListController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var list = await _db.TodoLists.FindAsync(id);
            if (list == null)
                return NotFound();
            return View(list);
        }

        // POST: TodoListController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _logger?.LogInformation("DeleteConfirmed called with id={Id}", id);
            var list = await _db.TodoLists.FindAsync(id);
            if (list == null)
                return NotFound();

            _db.TodoLists.Remove(list);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
