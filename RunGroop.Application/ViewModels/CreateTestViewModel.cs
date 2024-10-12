using System.ComponentModel.DataAnnotations;

namespace RunGroop.Application.ViewModels
{
    public class CreateTestViewModel
    {
        [Display(Name = "name"), Required(ErrorMessage = "required")]
        public string Name { get; set; } = string.Empty;
    }
}
