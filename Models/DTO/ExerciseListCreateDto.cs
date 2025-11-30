using System.ComponentModel.DataAnnotations;

namespace Projekt_befit.Models.DTO
{
	public class ExerciseListCreateDto
	{
		[Display(Name = "Rodzaj ćwiczenia")]
		public int ExerciseNameId { get; set; }

		[Display(Name = "Sesja treningowa")]
		public int TrainingSessionId { get; set; }

		[Display(Name = "Obciążenie (kg)")]
		public float Weight { get; set; }

		[Display(Name = "Liczba serii")]
		public int Sets { get; set; }

		[Display(Name = "Powtórzenia w serii")]
		public int RepsPerSet { get; set; }
	}
}
