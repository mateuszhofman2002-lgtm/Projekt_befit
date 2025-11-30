using System;
using System.ComponentModel.DataAnnotations;

namespace Projekt_befit.Models.DTO
{
	public class TrainingSessionCreateDto
	{
		[Display(Name = "Początek treningu")]
		public DateTime StartTime { get; set; }

		[Display(Name = "Koniec treningu")]
		public DateTime EndTime { get; set; }
	}
}
