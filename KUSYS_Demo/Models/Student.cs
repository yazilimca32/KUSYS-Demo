using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KUSYS_Demo.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public IdentityUser? User { get; set; }

        public ICollection<Course> Courses { get; } = new List<Course>();
     
    }
}
