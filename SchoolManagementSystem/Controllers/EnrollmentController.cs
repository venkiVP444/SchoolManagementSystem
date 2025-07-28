using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class EnrollmentController : Controller
{
    private readonly ApplicationDbContext _context;

    public EnrollmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Enrollment/Create
    public async Task<IActionResult> Create()
    {
        var viewModel = new EnrollmentViewModel
        {
            Students = await _context.Students.ToListAsync(),
            Courses = await _context.Courses
                .Include(c => c.Teacher)
                .ToListAsync()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EnrollmentViewModel model)
    {
        if (ModelState.IsValid)
        {
            var enrollment = new StudentCourse
            {
                StudentId = model.StudentId,
                CourseId = model.CourseId,
                EnrollmentDate = DateTime.Now
            };

            // Check if enrollment already exists
            var existingEnrollment = await _context.StudentCourses
                .FirstOrDefaultAsync(e => e.StudentId == model.StudentId && e.CourseId == model.CourseId);

            if (existingEnrollment == null)
            {
                _context.StudentCourses.Add(enrollment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Student enrolled successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "This student is already enrolled in this course.");
            }
        }

        // If we got this far, something failed
        model.Students = await _context.Students.ToListAsync();
        model.Courses = await _context.Courses
            .Include(c => c.Teacher)
            .ToListAsync();

        return View(model);
    }

    // GET: Enrollment/Index
    public async Task<IActionResult> Index()
    {
        var enrollments = await _context.StudentCourses
            .Include(sc => sc.Student)
            .Include(sc => sc.Course)
            .ThenInclude(c => c.Teacher)
            .ToListAsync();

        return View(enrollments);
    }

    // GET: Enrollment/Delete/5
    // GET: Enrollment/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enrollment = await _context.StudentCourses
            .Include(e => e.Student)
            .Include(e => e.Course)
            .FirstOrDefaultAsync(m => m.Id == id); // Use Id property
        if (enrollment == null)
        {
            return NotFound();
        }

        return View(enrollment);
    }

    // POST: Enrollment/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var enrollment = await _context.StudentCourses.FindAsync(id); // Use Id property
        if (enrollment != null)
        {
            _context.StudentCourses.Remove(enrollment);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Enrollment deleted successfully.";
        }

        return RedirectToAction(nameof(Index));
    }
}