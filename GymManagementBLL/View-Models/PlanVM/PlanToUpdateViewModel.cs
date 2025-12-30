using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.View_Models.PlanVM
{
    public class PlanToUpdateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Plan Name is length between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must have letter and speaces only ")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Plan Name is length between 2 and 200")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "DurationDays is required")]
        [Range(1, 365, ErrorMessage = "Duration Days is between 1 and 365")]
        public int DurationDays { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(250, 10000, ErrorMessage = "Price between 250 and 10000 EGY")]
        [Precision(10, 2)]
        public decimal Price { get; set; }
    }
}
