using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Day_6;
using MVC_Day_6.Filters;
using MVC_Day_6.Models;

namespace MVC_Day_6.Controllers
{
    public class CoursesController : Controller
    {
        private ITIModel db = new ITIModel();

        [AuthFilter]
        // GET: Courses
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        [AuthFilter]
        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.stds = db.StudentCourses.Where(c => c.CourseId == id).Select(c => c.Student).ToList();
            ViewBag.depts = db.DepartmentCourses.Where(c => c.CourseID == id).Select(c => c.Department);
            return View(course);
        }

        [AuthFilter]
        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        [AuthFilter]
        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseId,CourseName")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        [AuthFilter]
        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        [AuthFilter]
        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseId,CourseName")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        [AuthFilter]
        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        [AuthFilter]
        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        [AuthFilter]
        public ActionResult addStudents(int id, int deptId)
        {
            var allStudents = db.Students.Where(d=> d.DeptId == deptId).ToList();
            var CourseStudents = db.StudentCourses.Where(c => c.CourseId == id).Select(c => c.Student);
            var StudentsList = allStudents.Except(CourseStudents).ToList();
            ViewBag.course = db.Courses.FirstOrDefault(c=> c.CourseId == id);
            ViewBag.dept = deptId;
            return View(StudentsList);
        }

        [HttpPost]
        public ActionResult addStudents(int id, Dictionary<string, bool> stds, int degree)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in stds)
                {
                    if (item.Value == true)
                    {
                        db.StudentCourses.Add(new StudentCourse() { StudentId = int.Parse(item.Key), CourseId = id, Degree = degree });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Details/" + id, "Courses");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [AuthFilter]
        public ActionResult deleteStudents(int id, int deptId)
        {
            var CourseStudents = db.StudentCourses.Where(c => c.CourseId == id).Select(c => c.Student).ToList();
            ViewBag.course = db.Courses.FirstOrDefault(c => c.CourseId == id);
            ViewBag.dept = deptId;
            return View(CourseStudents);
        }

        [HttpPost]
        public ActionResult deleteStudents(int id, Dictionary<string, bool> stds)
        {
            foreach (var item in stds)
            {
                if (item.Value == true)
                {
                    int x = int.Parse(item.Key);
                    var std = db.StudentCourses.FirstOrDefault(s=> s.StudentId == x && s.CourseId == id);
                    db.StudentCourses.Remove(std);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Details/" + id, "Courses");
        }

        [AuthFilter]
        [HttpGet]
        public ActionResult addInDept(int id)
        {
            var allDepartment = db.Departments.ToList();
            var crsDept = db.DepartmentCourses.Where(c => c.CourseID == id).Select(c => c.Department);
            var list = allDepartment.Except(crsDept).ToList();
            ViewBag.course = db.Courses.FirstOrDefault(c => c.CourseId == id);
           
            return View(list);
        }

        [HttpPost]
        public ActionResult addInDept(int id, Dictionary<string, bool> depts)
        {
            foreach (var item in depts)
            {
                if (item.Value == true)
                {
                    db.DepartmentCourses.Add(new DepartmentCourse() { CourseID = id, DeptID = int.Parse(item.Key)});
                }
            }
            db.SaveChanges();
            return RedirectToAction("Details/" + id, "Courses");
        }

        [HttpGet]
        [AuthFilter]
        public ActionResult deleteFromDept(int id)
        {
            var crsDept = db.DepartmentCourses.Where(c => c.CourseID == id).Select(c => c.Department).ToList();
            ViewBag.course = db.Courses.FirstOrDefault(c => c.CourseId == id);
            return View(crsDept);
        }

        [HttpPost]
        public ActionResult deleteFromDept(int id, Dictionary<string, bool> depts)
        {
            foreach (var item in depts)
            {
                if (item.Value == true)
                {
                    int deptID = int.Parse(item.Key);
                    var dept = db.DepartmentCourses.FirstOrDefault(d=> d.DeptID == deptID && d.CourseID == id);
                    db.DepartmentCourses.Remove(dept);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Details/" + id, "Courses");
        }
    }
}
