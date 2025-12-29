using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.View_Models.MemberVM
{
    public class HealthRecordViewModel
    {
        [Required(ErrorMessage = "Height is required")]
        [Range(0.1, 300, ErrorMessage = "Height between 0.1 and 300 cm")]
        public decimal Height { get; set; }


        [Required(ErrorMessage = "Weight is required")]
        [Range(1, 300, ErrorMessage = "Weight between 0.1 and 300 cm")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Blood Type is required")]
        [StringLength(3, ErrorMessage = "Blood Type is max 3")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }
    }
}
