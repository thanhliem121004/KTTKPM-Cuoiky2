using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class PhanQuyenController : Controller
{
    private readonly DataContext _dataContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    public PhanQuyenController(DataContext dataContext, RoleManager<IdentityRole> roleManager)
    {
        _dataContext = dataContext;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index(int pg = 1)
    {
        const int pageSize = 10;
        if (pg < 1) pg = 1;

        var roles = await _dataContext.Roles.OrderByDescending(p => p.Id).ToListAsync();

        int recsCount = roles.Count();
        var pager = new PhanTrang(recsCount, pg, pageSize);
        int recSkip = (pg - 1) * pageSize;

        var data = roles.Skip(recSkip).Take(pager.PageSize).ToList();

        ViewBag.Pager = pager;

        return View(data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] IdentityRole role)
    {
        if (ModelState.IsValid)
        {
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(role);
    }

    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        return View(role);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] IdentityRole role)
    {
        if (id != role.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingRole = await _roleManager.FindByIdAsync(id);
                if (existingRole == null)
                {
                    return NotFound();
                }

                existingRole.Name = role.Name;
                var result = await _roleManager.UpdateAsync(existingRole);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RoleExists(role.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return View(role);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> RoleExists(string id)
    {
        return await _roleManager.RoleExistsAsync(id);
    }
}