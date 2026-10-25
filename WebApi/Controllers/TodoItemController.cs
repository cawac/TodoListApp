using Microsoft.AspNetCore.Mvc;
using WebApi.Mappings;
using WebApi.Services;
using WebApi.ViewModels;

namespace WebApi.Controllers;

public class TodoItemController : Controller
{
    private readonly ITodoItemDatabaseService _itemService;

    public TodoItemController(ITodoItemDatabaseService itemService)
    {
        _itemService = itemService;
    }

    public async Task<IActionResult> Index(int? todoListId)
    {
        var items = (await _itemService.GetAllAsync())
            .Where(i => !todoListId.HasValue || i.TodoListId == todoListId.Value)
            .Select(i => i.ToSummaryViewModel())
            .ToList();
        return View(items);
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _itemService.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        return View(item.ToDetailsViewModel());
    }

    public IActionResult Create(int todoListId)
    {
        return View(new TodoItemCreateViewModel { TodoListId = todoListId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoItemCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        await _itemService.CreateAsync(vm.ToDto());
        return RedirectToAction(nameof(Index), new { todoListId = vm.TodoListId });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _itemService.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        return View(item.ToEditViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TodoItemEditViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var item = await _itemService.GetByIdAsync(vm.Id);
        if (item is null)
        {
            return NotFound();
        }

        vm.ApplyTo(item);
        await _itemService.UpdateAsync(vm.Id, item);
        return RedirectToAction(nameof(Details), new { id = vm.Id });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _itemService.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        return View(item.ToSummaryViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _itemService.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        await _itemService.DeleteAsync(id);
        return RedirectToAction(nameof(Index), new { todoListId = item.TodoListId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id, string? returnUrl)
    {
        var item = await _itemService.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        item.IsCompleted = !item.IsCompleted;
        await _itemService.UpdateAsync(id, item);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index), new { todoListId = item.TodoListId });
    }
}
