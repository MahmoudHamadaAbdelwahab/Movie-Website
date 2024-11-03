using System.ComponentModel.DataAnnotations;

namespace MvcMovie.ViewModels
{
	public class CreateTestViewModel
	{
		[Display(Name = "name"), Required(ErrorMessage = "required")]
		public string Name { get; set; }
	}
}
