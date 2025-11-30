using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_befit.Models;
using System.ComponentModel.DataAnnotations;

namespace Projekt_befit.Areas.Identity.Pages.Account
{
	public class LoginModel : PageModel
	{
		private readonly SignInManager<ApplicationUser> _signInManager;

		public LoginModel(SignInManager<ApplicationUser> signInManager)
		{
			_signInManager = signInManager;
		}

		[BindProperty]
		public InputModel Input { get; set; } = new InputModel();

		public string? ReturnUrl { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			public string Email { get; set; }

			[Required]
			[DataType(DataType.Password)]
			public string Password { get; set; }

			public bool RememberMe { get; set; }   // 🔥 BYŁO BRAK
		}

		public void OnGet(string? returnUrl = null)
		{
			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(
					Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false
				);

				if (result.Succeeded)
				{
					return LocalRedirect(returnUrl ?? "/");
				}

				ModelState.AddModelError(string.Empty, "Niepoprawne dane logowania.");
			}

			return Page();
		}
	}
}
