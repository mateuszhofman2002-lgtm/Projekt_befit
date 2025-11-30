using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_befit.Data;
using Projekt_befit.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_befit.Controllers
{
	[Authorize]
	public class StatsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public StatsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		private string GetUserId() => _userManager.GetUserId(User);

		public async Task<IActionResult> Index()
		{
			var userId = GetUserId();
			var fourWeeksAgo = DateTime.Now.AddDays(-28);

			// pobieramy wszystkie wykonane ćwiczenia użytkownika
			var exerciseLists = await _context.ExerciseLists
				.Include(el => el.TrainingSession)
				.Where(el =>
					el.TrainingSession.UserId == userId &&
					el.TrainingSession.StartTime >= fourWeeksAgo)
				.ToListAsync();

			// pobieramy ćwiczenia użytkownika ORAZ globalne (UserId == null)
			var exerciseNames = await _context.ExerciseNames
				.Where(en => en.UserId == userId || en.UserId == null)
				.ToListAsync();

			var stats = exerciseNames.Select(en => new ExerciseStatsViewModel
			{
				ExerciseName = en.Name,

				SessionsCount = exerciseLists
					.Where(el => el.ExerciseNameId == en.Id)
					.Select(el => el.TrainingSessionId)
					.Distinct()
					.Count(),

				TotalReps = exerciseLists
					.Where(el => el.ExerciseNameId == en.Id)
					.Sum(el => el.Sets * el.RepsPerSet),

				AverageWeight = exerciseLists
					.Where(el => el.ExerciseNameId == en.Id)
					.Select(el => (float?)el.Weight)
					.DefaultIfEmpty(0)
					.Average() ?? 0,

				MaxWeight = exerciseLists
					.Where(el => el.ExerciseNameId == en.Id)
					.Select(el => (float?)el.Weight)
					.DefaultIfEmpty(0)
					.Max() ?? 0

			}).Where(s => s.SessionsCount > 0).ToList(); // usuwamy puste ćwiczenia

			return View(stats);
		}
	}
}
