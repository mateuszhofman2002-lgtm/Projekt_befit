using System.ComponentModel.DataAnnotations;

namespace Projekt_befit.Models
{
	public class ExerciseList
	{
		public int Id { get; set; }

		[Display(Name = "Rodzaj ćwiczenia")]
		public int ExerciseNameId { get; set; }
		public ExerciseName? ExerciseName { get; set; }

		[Display(Name = "Sesja treningowa")]
		public int TrainingSessionId { get; set; }
		public TrainingSession? TrainingSession { get; set; }

		[Display(Name = "Obciążenie (kg)")]
		public float Weight { get; set; }

		[Display(Name = "Liczba serii")]
		public int Sets { get; set; }

		[Display(Name = "Powtórzenia w serii")]
		public int RepsPerSet { get; set; }


		public string UserId { get; set; }
		public ApplicationUser? User { get; set; }
	}
}
