using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Day_6.Models
{
    public class Course
    {
        public int CourseId { set; get; }

        [Required]
        public string CourseName { set; get; }

        public List<StudentCourse> StudentCourses { get; set; }
    }
}