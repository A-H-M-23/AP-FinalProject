using System.ComponentModel.DataAnnotations;

namespace Business
{
    public class User
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Username is Required")]
        [RegularExpression("^[a-zA-z0-9@_.]{3,12}$", ErrorMessage = "Username is Invalid")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Firstname is Required")]
        [StringLength(maximumLength: 20, MinimumLength = 2, ErrorMessage = "Firstname Should be between 2 to 20 Characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is Required")]
        [StringLength(maximumLength: 25, MinimumLength = 3, ErrorMessage = "Lastname Should be between 3 to 25 Characters")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Phone Number is Required")]
        [RegularExpression("^09[0-9]{9}$", ErrorMessage = "Phone Number is`nt Valid")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression("^[A-Za-z0-9._]{1,256}@[A-Za-z0-9]{1,256}[.][A-Za-z]{2,4}[.]{0,1}[A-Za-z]{0,4}", ErrorMessage = "Email is invalid")]
        public string Email { get; set; }
        public GenderType Gender { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsLocked { get; set; } = false;
        [Required(ErrorMessage = "Password is Required")]
        [RegularExpression("^[a-zA-z0-9@&?!_.]{8,24}$", ErrorMessage = "Email is`nt Valid")]
        public string HashPassword { get; set; }
        public string ProfilePhoto { get; set; }
    }
}
