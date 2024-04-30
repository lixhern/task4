using System.ComponentModel.DataAnnotations;

namespace task4.ViewModels
{
    public class RegisterViewModel
    {
        [Required]

        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        //[DataType(DataType.UserName)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }


        //public string DateOfRegister { get; set; }
    }
}
