using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

[Authorize(Roles = "Admin")]
public class TeacherController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public TeacherController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Teacher
    public async Task<IActionResult> Index()
    {
        return View(await _context.Teachers.ToListAsync());
    }

    // GET: Teacher/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers
            .Include(t => t.Courses)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (teacher == null) return NotFound();

        return View(teacher);
    }

    // GET: Teacher/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Teacher/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Teacher teacher, string password)
    {
        if (ModelState.IsValid)
        {
            teacher.UserName = teacher.Email;
            teacher.EmailConfirmed = true;
            var result = await _userManager.CreateAsync(teacher, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(teacher, "Teacher");
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(teacher);
    }

    // GET: Teacher/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound();

        return View(teacher);
    }

    // POST: Teacher/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, Teacher teacher)
    {
        if (id != teacher.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(teacher);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(teacher.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(teacher);
    }

    // GET: Teacher/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (teacher == null) return NotFound();

        return View(teacher);
    }

    // POST: Teacher/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TeacherExists(string id)
    {
        return _context.Teachers.Any(e => e.Id == id);
    }
}