using System.ComponentModel.DataAnnotations;

namespace Projekt_befit.Models
{
	public class ExerciseName
	{
		public int Id { get; set; }

		[MaxLength(40)]
		[Display(Name = "Nazwa ćwiczenia")]
		public string Name { get; set; }

	
		public string? UserId { get; set; }
		public virtual ApplicationUser? User { get; set; }

	}
}
