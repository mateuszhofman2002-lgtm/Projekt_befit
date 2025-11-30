using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_befit.Data;
using Projekt_befit.Models;
using Projekt_befit.Models.DTO;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_befit.Controllers
{
	[Authorize]
	public class TrainingSessionsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public TrainingSessionsController(ApplicationDbContext context,
			UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		private string GetUserId() => _userManager.GetUserId(User);

		// INDEX
		public async Task<IActionResult> Index()
		{
			var userId = GetUserId();
			var sessions = await _context.TrainingSessions
				.Where(t => t.UserId == userId)
				.OrderByDescending(t => t.StartTime)
				.ToListAsync();

			return View(sessions);
		}

		// DETAILS
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var session = await _context.TrainingSessions.FirstOrDefaultAsync(t => t.Id == id);

			if (session == null || session.UserId != GetUserId())
				return NotFound();

			return View(session);
		}

		// GET: CREATE
		public IActionResult Create()
		{
			var dto = new TrainingSessionCreateDto
			{
				StartTime = DateTime.Now,
				EndTime = DateTime.Now.AddHours(1)
			};

			return View(dto);
		}


		// POST: CREATE
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(TrainingSessionCreateDto dto)
		{
			if (ModelState.IsValid)
			{
				var session = new TrainingSession
				{
					StartTime = dto.StartTime,
					EndTime = dto.EndTime,
					UserId = GetUserId()
				};

				_context.Add(session);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return View(dto);
		}

		// GET: EDIT
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var session = await _context.TrainingSessions.FindAsync(id);

			if (session == null || session.UserId != GetUserId())
				return NotFound();

			var dto = new TrainingSessionEditDto
			{
				Id = session.Id,
				StartTime = session.StartTime,
				EndTime = session.EndTime
			};

			return View(dto);
		}

		// POST: EDIT
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, TrainingSessionEditDto dto)
		{
			var session = await _context.TrainingSessions.AsNoTracking()
				.FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetUserId());

			if (session == null)
				return NotFound();

			if (ModelState.IsValid)
			{
				var updated = new TrainingSession
				{
					Id = dto.Id,
					StartTime = dto.StartTime,
					EndTime = dto.EndTime,
					UserId = GetUserId()
				};

				_context.Update(updated);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return View(dto);
		}

		// GET: DELETE
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var session = await _context.TrainingSessions
				.FirstOrDefaultAsync(t => t.Id == id);

			if (session == null || session.UserId != GetUserId())
				return NotFound();

			return View(session);
		}

		// POST: DELETE
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var session = await _context.TrainingSessions.FindAsync(id);

			if (session != null && session.UserId == GetUserId())
			{
				_context.TrainingSessions.Remove(session);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
