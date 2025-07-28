using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
[Authorize(Roles = "Admin")]
public class CourseController : Controller
{
    private readonly ApplicationDbContext _context;
    public CourseController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET: Course
    public async Task<IActionResult> Index()
    {
        var courses = await _context.Courses
            .Include(c => c.Teacher)
            .ToListAsync();
        return View(courses);
    }
    // GET: Course/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var course = await _context.Courses
            .Include(c => c.Teacher)
            .Include(c => c.StudentCourses)
            .ThenInclude(sc => sc.Student)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (course == null) return NotFound();
        return View(course);
    }
    // GET: Course/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Teachers = await _context.Teachers
        .Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = $"{t.FirstName} {t.LastName}"
        }).ToListAsync();
        return View();
    }
    // POST: Course/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course course)
    {
        ViewBag.Teachers = await _context.Teachers
            .Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = $"{t.FirstName} {t.LastName}"
            }).ToListAsync();
        course.Teacher = await _context.Teachers.FindAsync(course.TeacherId);
            course.Teacher = await _context.Teachers.FindAsync(course.TeacherId);

            _context.Add(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
    }
    // GET: Course/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var course = await _context.Courses.FindAsync(id);
        if (course == null) return NotFound();
        // Convert Teacher objects to SelectListItem
        var teachers = await _context.Teachers
            .Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = $"{t.FirstName} {t.LastName}"
            })
            .ToListAsync();
        ViewData["Teachers"] = teachers;
        return View(course);
    }
    // POST: Course/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreditHours,TeacherId")] Course course)
    {
        if (id != course.Id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(course);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(course.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        // FIX: Use ViewData instead of ViewBag
        ViewData["Teachers"] = await _context.Teachers
            .Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = $"{t.FirstName} {t.LastName}"
            })
            .ToListAsync();
        return View(course);
    }
    // GET: Course/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var course = await _context.Courses
            .Include(c => c.Teacher)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (course == null) return NotFound();
        return View(course);
    }
    // POST: Course/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool CourseExists(int id)
    {
        return _context.Courses.Any(e => e.Id == id);
    }
}