using System.ComponentModel.DataAnnotations;



namespace task4.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public string ReturnUrl;
    }
}
