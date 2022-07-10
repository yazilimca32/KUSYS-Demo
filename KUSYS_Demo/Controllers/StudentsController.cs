using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KUSYS_Demo.Data;
using KUSYS_Demo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace KUSYS_Demo.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Index()
        {
              return _context.Students != null ? 
                          View(await _context.Students.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Students'  is null.");
        }

        // GET: Students/Details/5
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return Json(new { failed = true, message = "Tüm bilgileri eksiksiz girdiginize emin olunuz" });
            }

            var student = await _context.Students.Include(s => s.Courses)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return Json(new { failed = true, message = "Öğrenci sistemde kayıtlı değil" });
            }
            return PartialView("~/Views/Students/_UserEditPartial.cshtml", student);           
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> CoursesOfStudent()
        {
            if (_context.Students == null)
            {
                return Json(new { failed = true, message = "Tüm bilgileri eksiksiz girdiginize emin olunuz" });
            }

            var loggedInUserId = _context.Users.First(p => p.Email == User.Identity.Name).Id;

            var students = _context.Students.Where(s=> s.User.Id == loggedInUserId).Include(s => s.Courses).ToList();

            if (students == null)
            {
                return Json(new { failed = true, message = "Sistemde Öğrenci yok" });
            }

            return View("AllStudentsAndCourses", students);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllStudentsAndCourses()
        {
            if (_context.Students == null)
            {
                return Json(new { failed = true, message = "Tüm bilgileri eksiksiz girdiginize emin olunuz" });
            }

            var students = _context.Students.Include(s => s.Courses).ToList();
                
            if (students == null)
            {
                return Json(new { failed = true, message = "Sistemde Öğrenci yok" });
            }

            return View(students);
        }

        // GET: Students/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("StudentId,FirstName,LastName,BirthDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
                string id = Guid.NewGuid().ToString();
                string pass = "User*123";

                IdentityUser user = new IdentityUser()
                {
                    Id = id,
                    UserName = id + "@uni.edu.tr",
                    NormalizedUserName = id + "@UNI.EDU.TR",
                    Email = id + "@uni.edu.tr",
                    NormalizedEmail = id + "@UNI.EDU.TR",
                    LockoutEnabled = false,
                    PhoneNumber = null,
                    EmailConfirmed = true
                };
                user.PasswordHash = passwordHasher.HashPassword(user, pass);
                _context.Users.Add(user);
                _context.SaveChanges();

                _context.UserRoles.Add(new IdentityUserRole<string>() { UserId = user.Id, RoleId =_context.Roles.First(r => r.Name == "User").Id });
                _context.SaveChanges();

                student.User = user;
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName,BirthDate")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Students'  is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
          return (_context.Students?.Any(e => e.StudentId == id)).GetValueOrDefault();
        }
    }
}
