
using DataSanitization.validator;
using System.ComponentModel.DataAnnotations;

namespace DataSanitization.model
{
    public class TestDTO
    {
        [Required(ErrorMessage = "Requires a product order ID")]
        [StringLength(100, ErrorMessage = "Requires a product order ID with maximum 100 characters")]
        [XssValidator(ErrorMessage = "Xss character(s) detected in {0}")]
        public string Id { get; set; }
    }
}
