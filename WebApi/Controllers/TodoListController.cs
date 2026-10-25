using Microsoft.AspNetCore.Mvc;
using WebApi.Mappings;
using WebApi.Services;
using WebApi.ViewModels;

namespace WebApi.Controllers;

public class TodoListController : Controller
{
    private const string TitleTakenMessage = "A todo list with this title already exists";

    private readonly ITodoListDatabaseService _service;
    private readonly ITodoItemDatabaseService _itemService;

    public TodoListController(ITodoListDatabaseService service, ITodoItemDatabaseService itemService)
    {
        _service = service;
        _itemService = itemService;
    }

    public async Task<IActionResult> Index()
    {
        var lists = (await _service.GetAllAsync())
            .Select(l => l.ToGetViewModel())
            .ToList();
        return View(lists);
    }

    public async Task<IActionResult> Details(int id)
    {
        var list = await _service.GetByIdAsync(id);
        if (list is null)
        {
            return NotFound();
        }

        return View(list.ToGetViewModel());
    }

    public async Task<IActionResult> AllTasks()
    {
        var items = (await _itemService.GetAllAsync())
            .OrderBy(i => i.Id)
            .Select(i => i.ToSummaryViewModel())
            .ToList();
        return View("~/Views/TodoItem/AllTasks.cshtml", items);
    }

    public IActionResult Create()
    {
        return View(new TodoListCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoListCreateViewModel vm)
    {
        if (await IsTitleTakenAsync(vm.Title, 0))
        {
            ModelState.AddModelError(nameof(vm.Title), TitleTakenMessage);
        }

        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        await _service.CreateAsync(vm.ToDto());
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var list = await _service.GetByIdAsync(id);
        if (list is null)
        {
            return NotFound();
        }

        return View(list.ToEditViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TodoListEditViewModel vm)
    {
        if (id != vm.Id)
        {
            return BadRequest();
        }

        if (await IsTitleTakenAsync(vm.Title, vm.Id))
        {
            ModelState.AddModelError(nameof(vm.Title), TitleTakenMessage);
        }

        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var updated = await _service.UpdateAsync(id, vm.ToDto());
        if (updated is null)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var list = await _service.GetByIdAsync(id);
        if (list is null)
        {
            return NotFound();
        }

        return View(list.ToGetViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!await _service.DeleteAsync(id))
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> CheckTitle(string? title, int id)
    {
        return await IsTitleTakenAsync(title, id)
            ? Json($"A todo list \"{title}\" already exists")
            : Json(true);
    }

    private async Task<bool> IsTitleTakenAsync(string? title, int id)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return false;
        }

        var lists = await _service.GetAllAsync();
        return lists.Any(l => l.Id != id && string.Equals(l.Title.Trim(), title.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
