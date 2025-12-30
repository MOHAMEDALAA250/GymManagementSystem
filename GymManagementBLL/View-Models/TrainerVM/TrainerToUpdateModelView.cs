using GymManagementDAL.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.View_Models.TrainerVM
{
    public class TrainerToUpdateModelView
    {
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Foramt")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email must be 5 and 100")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone format")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Must be valid Egyption Phone Number")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Building Number is required")]
        [Range(1, 9000, ErrorMessage = "Building Number between 1 and 9000")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street between 2 and 30")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Street must have letter and speaces only ")]
        public string Street { get; set; } = null!;
        [Required(ErrorMessage = "City Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Cite between 2 and 30")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City must have letter and speaces only ")]
        public string City { get; set; } = null!;
        [Required(ErrorMessage = "Specialties is required")]
        public Specialties Specialties { get; set; }
    }
}
