using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Day_6.Models
{
    public class DepartmentCourse
    {
        [Key, Column(Order = 0)]
        public int DeptID { set; get; }
        [Key, Column(Order = 1)]
        public int CourseID { set; get; }

        [ForeignKey("DeptID")]
        public virtual Department Department { set; get; }
        [ForeignKey("CourseID")]
        public virtual Course Course { set; get; }
    }
}