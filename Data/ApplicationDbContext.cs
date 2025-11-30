using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projekt_befit.Models;

namespace Projekt_befit.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<TrainingSession> TrainingSessions { get; set; }
		public DbSet<ExerciseList> ExerciseLists { get; set; }
		public DbSet<ExerciseName> ExerciseNames { get; set; }
	}
}
