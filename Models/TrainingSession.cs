using System;
using System.ComponentModel.DataAnnotations;

namespace Projekt_befit.Models
{
	public class TrainingSession
	{
		public int Id { get; set; }

		[Display(Name = "Początek treningu")]
		public DateTime StartTime { get; set; }

		[Display(Name = "Koniec treningu")]
		public DateTime EndTime { get; set; }

		public string UserId { get; set; }
	}
}
