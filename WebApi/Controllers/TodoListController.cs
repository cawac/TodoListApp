using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.ViewModels;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers;

public class TodoListController : Controller
{
    private readonly ITodoListDatabaseService _service;
    private readonly ITodoItemDatabaseService _itemService;

    public TodoListController(ITodoListDatabaseService service, ITodoItemDatabaseService itemService)
    {
        _service = service;
        _itemService = itemService;
    }

    public async Task<ActionResult> Index()
    {
        var lists = await _service.GetAllAsync();
        return View(lists);
    }

    public async Task<ActionResult> Details(int id)
    {
        var list = await _service.GetByIdAsync(id);

        if (list == null)
            return NotFound();

        return View(list);
    }

    public async Task<ActionResult> AllTasks(string? status, string? sort)
    {
        // status: "active" (default), "completed", "all"
        var items = (await _itemService.GetAllAsync()).ToList();

        var s = (status ?? "active").ToLowerInvariant();
        if (s == "active")
            items = items.Where(i => !i.IsCompleted).ToList();
        else if (s == "completed")
            items = items.Where(i => i.IsCompleted).ToList();

        // sorting: name_asc, name_desc, due_asc, due_desc, priority_asc, priority_desc
        var so = (sort ?? "name_asc").ToLowerInvariant();
        items = so switch
        {
            "name_desc" => items.OrderByDescending(i => i.Title).ToList(),
            "due_asc" => items.OrderBy(i => i.DueAt ?? DateTimeOffset.MaxValue).ToList(),
            "due_desc" => items.OrderByDescending(i => i.DueAt ?? DateTimeOffset.MinValue).ToList(),
            "priority_asc" => items.OrderBy(i => i.Priority).ToList(),
            "priority_desc" => items.OrderByDescending(i => i.Priority).ToList(),
            _ => items.OrderBy(i => i.Title).ToList(),
        };

        ViewData["StatusFilter"] = s;
        ViewData["Sort"] = so;
        return View("~/Views/TodoItem/AllTasks.cshtml", items);
    }
    public ActionResult Create()
    {
        return View(new TodoListData());
    }

    [HttpPost]
    public async Task<ActionResult> Create(TodoListData model)
    {
        if (!ModelState.IsValid) return View(model);

        model.CreatedAt = System.DateTimeOffset.UtcNow;
        await _service.CreateAsync(model);
        return RedirectToAction(nameof(Index));
    }

    public async Task<ActionResult> Edit(int id)
    {
        var list = await _service.GetByIdAsync(id);
        if (list == null) return NotFound();
        return View(list);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(int id, TodoListData model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);

        var updated = await _service.UpdateAsync(id, model);
        if (updated == null) return NotFound();
        return RedirectToAction(nameof(Index));
    }
    public async Task<ActionResult> Delete(int id)
    {
        var list = await _service.GetByIdAsync(id);
        if (list == null)
            return NotFound();
        return View(list);
    }

    [HttpPost]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var list = await _service.GetByIdAsync(id);
        if (list == null)
            return NotFound();

        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
