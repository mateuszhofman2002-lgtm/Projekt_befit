using Microsoft.AspNetCore.Identity;

namespace Projekt_befit.Models
{
	public class ApplicationUser : IdentityUser
	{
	
		public string? DisplayName { get; set; }
	}
}
