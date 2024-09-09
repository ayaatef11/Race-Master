using System.ComponentModel.DataAnnotations;

namespace RunGroopWebApp.ViewModels
{
    public class CreateTestViewModel
    {
        [Display(Name = "name"), Required(ErrorMessage = "required")]
        public string Name { get; set; } = string.Empty; 
    }
}
