using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_befit.Data;
using Projekt_befit.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_befit.Controllers
{
	public class ExerciseNamesController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public ExerciseNamesController(ApplicationDbContext context,
									   UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		private string GetUserId() => _userManager.GetUserId(User);

		// --------------------------------------------------------
		//  PUBLIC ACCESS — każdy użytkownik widzi listę i szczegóły
		// --------------------------------------------------------

		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			var items = await _context.ExerciseNames.ToListAsync();
			return View(items);
		}

		[AllowAnonymous]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var item = await _context.ExerciseNames.FirstOrDefaultAsync(e => e.Id == id);
			if (item == null) return NotFound();

			return View(item);
		}

		// --------------------------------------------------------
		//  CREATE — dostęp dla ZALOGOWANYCH użytkowników
		// --------------------------------------------------------

		[Authorize]
		public IActionResult Create() => View();

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name")] ExerciseName exerciseName)
		{
			if (ModelState.IsValid)
			{
				exerciseName.UserId = GetUserId();

				_context.Add(exerciseName);
				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(Index));
			}

			return View(exerciseName);
		}

		// --------------------------------------------------------
		//  EDIT — tylko ADMIN
		// --------------------------------------------------------

		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var item = await _context.ExerciseNames.FindAsync(id);

			if (item == null)
				return NotFound();

			return View(item);
		}

		[HttpPost]
		[Authorize(Roles = "Administrator")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserId")] ExerciseName exerciseName)
		{
			if (id != exerciseName.Id)
				return NotFound();

			if (ModelState.IsValid)
			{
				_context.Update(exerciseName);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return View(exerciseName);
		}

		// --------------------------------------------------------
		//  DELETE — tylko ADMIN
		// --------------------------------------------------------

		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var item = await _context.ExerciseNames.FirstOrDefaultAsync(e => e.Id == id);

			if (item == null)
				return NotFound();

			return View(item);
		}

		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Administrator")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var item = await _context.ExerciseNames.FindAsync(id);

			if (item != null)
			{
				_context.ExerciseNames.Remove(item);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
