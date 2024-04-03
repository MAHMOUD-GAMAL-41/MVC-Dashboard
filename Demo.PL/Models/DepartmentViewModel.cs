using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Deparment Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Deparment Code is required")]

        public string Code { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
