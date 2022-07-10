using KUSYS_Demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KUSYS_Demo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            this.SeedDatabase(builder);
        }

        private void SeedDatabase(ModelBuilder builder)
        {
            this.SeedRoles(builder);
            this.SeedUsers(builder);
            this.SeedUserRoles(builder);

            this.SeedCoursesAndStudents(builder);
        

        }



        private void SeedRoles(ModelBuilder builder)
        {

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895712", Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" });
        }

        private void SeedUsers(ModelBuilder builder)
        {
            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();

            IdentityUser user = new IdentityUser()
            {
                Id = "c12ddd14-6340-4840-95c2-db12554843e5",
                UserName = "user@gmail.com",
                NormalizedUserName = "USER@GMAIL.COM",
                Email = "user@gmail.com",
                NormalizedEmail = "USER@GMAIL.COM",
                LockoutEnabled = false,
                PhoneNumber = "1234567890",
                EmailConfirmed = true
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "User*123");
            builder.Entity<IdentityUser>().HasData(user);


            IdentityUser admin = new IdentityUser()
            {
                Id = "e74ddd14-6340-4840-95c2-db12554843e6",
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                LockoutEnabled = false,
                PhoneNumber = "1234567890",
                EmailConfirmed = true
            };
            admin.PasswordHash = passwordHasher.HashPassword(admin, "123456A.bc");
            builder.Entity<IdentityUser>().HasData(admin);
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "fab4fac1-c546-41de-aebc-a14da6895711", UserId = "e74ddd14-6340-4840-95c2-db12554843e6" },
                new IdentityUserRole<string>() { RoleId = "fab4fac1-c546-41de-aebc-a14da6895712", UserId = "c12ddd14-6340-4840-95c2-db12554843e5" }
                );
        }



        private void SeedCoursesAndStudents(ModelBuilder builder)
        {
            Course c1 = new Course() { CourseId = "CSI101", CourseName = "Introduction to Computer Science" };
            Course c2 = new Course() { CourseId = "CSI102", CourseName = "Algorithms" };
            Course c3 = new Course() { CourseId = "MAT101", CourseName = "Calculus" };
            Course c4 = new Course() { CourseId = "PHY101", CourseName = "Physics" };
            
            builder.Entity<Course>().HasData(
                  c1, c2, c3, c4
            );

            //Student s1 = new Student() { StudentId = 1, FirstName = "Ali", LastName = "Ağaoğlu" };
            //Student s2 = new Student() { StudentId = 2, FirstName = "Servet", LastName = "Çetin" };
            //Student s3 = new Student() { StudentId = 3, FirstName = "Onur", LastName = "Ünlü" };
            //Student s4 = new Student() { StudentId = 4, FirstName = "Deniz", LastName = "Yılmaz" };
            //Student s5 = new Student() { StudentId = 5, FirstName = "Zeki", LastName = "Alasya" };

            //c1.Students.Add(s1);

            //s1.Courses.Add(c1);
            //s1.Courses.Add(c2);
            //s2.Courses.Add(c3);
            //s3.Courses.Add(c2);
            //s3.Courses.Add(c4);
            //s4.Courses.Add(c1);
            //s5.Courses.Add(c3);

            //builder.Entity<Student>().HasData(
            //        s1,s2,s3,s4,s5
            //);
        }


    }
}