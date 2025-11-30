using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt_befit.Data;
using Projekt_befit.Models;
using Projekt_befit.Models.DTO;

namespace Projekt_befit.Controllers
{
	[Authorize]
	public class ExerciseListsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public ExerciseListsController(ApplicationDbContext context,
			UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		private string GetUserId() => _userManager.GetUserId(User);

		// GET: ExerciseLists
		public async Task<IActionResult> Index()
		{
			var userId = GetUserId();

			var query = _context.ExerciseLists
				.Include(e => e.ExerciseName)
				.Include(e => e.TrainingSession)
				.Where(e => e.UserId == userId);

			var list = await query.ToListAsync();
			return View(list);
		}

		// GET: ExerciseLists/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var userId = GetUserId();

			var entry = await _context.ExerciseLists
				.Include(e => e.ExerciseName)
				.Include(e => e.TrainingSession)
				.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

			if (entry == null) return NotFound();

			return View(entry);
		}

		// GET: ExerciseLists/Create
		public IActionResult Create()
		{
			PopulateSelectLists();
			return View(new ExerciseListCreateDto());
		}

		// POST: ExerciseLists/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ExerciseListCreateDto dto)
		{
			var userId = GetUserId();

			if (ModelState.IsValid)
			{
				var entry = new ExerciseList
				{
					ExerciseNameId = dto.ExerciseNameId,
					TrainingSessionId = dto.TrainingSessionId,
					Weight = dto.Weight,
					Sets = dto.Sets,
					RepsPerSet = dto.RepsPerSet,
					UserId = userId
				};

				_context.ExerciseLists.Add(entry);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			PopulateSelectLists(dto.ExerciseNameId, dto.TrainingSessionId);
			return View(dto);
		}

		// GET: ExerciseLists/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var userId = GetUserId();

			var entry = await _context.ExerciseLists
				.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

			if (entry == null) return NotFound();

			var dto = new ExerciseListEditDto
			{
				Id = entry.Id,
				ExerciseNameId = entry.ExerciseNameId,
				TrainingSessionId = entry.TrainingSessionId,
				Weight = entry.Weight,
				Sets = entry.Sets,
				RepsPerSet = entry.RepsPerSet
			};

			PopulateSelectLists(dto.ExerciseNameId, dto.TrainingSessionId);
			return View(dto);
		}

		// POST: ExerciseLists/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, ExerciseListEditDto dto)
		{
			var userId = GetUserId();

			if (id != dto.Id) return NotFound();

			var exists = await _context.ExerciseLists
				.AnyAsync(e => e.Id == id && e.UserId == userId);

			if (!exists) return NotFound();

			if (ModelState.IsValid)
			{
				var updated = new ExerciseList
				{
					Id = dto.Id,
					ExerciseNameId = dto.ExerciseNameId,
					TrainingSessionId = dto.TrainingSessionId,
					Weight = dto.Weight,
					Sets = dto.Sets,
					RepsPerSet = dto.RepsPerSet,
					UserId = userId
				};

				_context.Update(updated);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			PopulateSelectLists(dto.ExerciseNameId, dto.TrainingSessionId);
			return View(dto);
		}

		// GET: ExerciseLists/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var userId = GetUserId();

			var entry = await _context.ExerciseLists
				.Include(e => e.ExerciseName)
				.Include(e => e.TrainingSession)
				.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

			if (entry == null) return NotFound();

			return View(entry);
		}

		// POST: ExerciseLists/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var userId = GetUserId();

			var entry = await _context.ExerciseLists
				.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

			if (entry != null)
			{
				_context.ExerciseLists.Remove(entry);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}

		private void PopulateSelectLists(int? selectedExerciseNameId = null, int? selectedTrainingSessionId = null)
		{
			var exerciseNames = _context.ExerciseNames
				.Select(x => new { x.Id, x.Name })
				.ToList();

			var userId = GetUserId();
			var sessions = _context.TrainingSessions
				.Where(t => t.UserId == userId)
				.Select(t => new
				{
					t.Id,
					Display = t.StartTime.ToString("g")
				})
				.ToList();

			ViewData["ExerciseNameId"] = new SelectList(exerciseNames, "Id", "Name", selectedExerciseNameId);
			ViewData["TrainingSessionId"] = new SelectList(sessions, "Id", "Display", selectedTrainingSessionId);
		}


	}
}
