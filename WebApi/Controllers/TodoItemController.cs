using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Services;
using WebApi.ViewModels;

namespace WebApi.Controllers;

public class TodoItemController : Controller
{
    private readonly ITodoItemDatabaseService _itemService;
    private readonly ITodoListDatabaseService _listService;

    public TodoItemController(ITodoItemDatabaseService itemService, ITodoListDatabaseService listService)
    {
        _itemService = itemService;
        _listService = listService;
    }

    public async Task<IActionResult> Index(int? todoListId)
    {
        var items = (await _itemService.GetAllAsync())
            .Where(i => !todoListId.HasValue || i.TodoListId == todoListId.Value)
            .ToList();
        return View(items);
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _itemService.GetByIdAsync(id);
        if (item == null) return NotFound();

        return View(item);
    }

    public IActionResult Create(int todoListId)
    {
        var vm = new TodoItemCreateViewModel { TodoListId = todoListId };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TodoItemCreateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        if (vm.DueAt.HasValue && vm.DueAt.Value < DateTimeOffset.UtcNow.Date)
        {
            ModelState.AddModelError(nameof(vm.DueAt), "Due date cannot be in the past.");
            return View(vm);
        }

        var dto = new TodoItemData
        {
            Title = vm.Title,
            Notes = vm.Notes,
            DueAt = vm.DueAt,
            TodoListId = vm.TodoListId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _itemService.CreateAsync(dto);

        return RedirectToAction(nameof(Index), new { todoListId = vm.TodoListId });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _itemService.GetByIdAsync(id);
        if (item == null) return NotFound();

        var vm = new TodoItemEditViewModel
        {
            Id = item.Id,
            Title = item.Title,
            Notes = item.Notes,
            DueAt = item.DueAt,
            IsCompleted = item.IsCompleted
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TodoItemEditViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        if (vm.DueAt.HasValue && vm.DueAt.Value < DateTimeOffset.UtcNow.Date)
        {
            ModelState.AddModelError(nameof(vm.DueAt), "Due date cannot be in the past.");
            return View(vm);
        }

        var dto = new TodoItemData
        {
            Id = vm.Id,
            Title = vm.Title,
            Notes = vm.Notes,
            DueAt = vm.DueAt,
            IsCompleted = vm.IsCompleted
        };

        await _itemService.UpdateAsync(vm.Id, dto);

        return RedirectToAction(nameof(Details), new { id = vm.Id });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _itemService.GetByIdAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _itemService.GetByIdAsync(id);
        if (item == null) return NotFound();

        await _itemService.DeleteAsync(id);

        return RedirectToAction(nameof(Index), new { todoListId = item.TodoListId });
    }
}
