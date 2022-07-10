namespace KUSYS_Demo.Models
{
    public class Course
    {
        public string? CourseId { get; set; }
        public string? CourseName { get; set; }

        public ICollection<Student> Students { get; } = new List<Student>();
    }
}
