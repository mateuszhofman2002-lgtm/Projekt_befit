using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_befit.Models;

namespace Projekt_befit.Areas.Identity.Pages.Account
{
	public class LogoutModel : PageModel
	{
		private readonly SignInManager<ApplicationUser> _signInManager;

		public LogoutModel(SignInManager<ApplicationUser> signInManager)
		{
			_signInManager = signInManager;
		}

		public async Task<IActionResult> OnPost()
		{
			await _signInManager.SignOutAsync();
			return Redirect("~/");
		}
	}
}
