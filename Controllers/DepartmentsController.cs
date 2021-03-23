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
    public class DepartmentsController : Controller
    {
        private ITIModel db = new ITIModel();

        [AuthFilter]
        // GET: Departments
        public ActionResult Index()
        {
            return View(db.Departments.ToList());
        }

        [AuthFilter]
        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.courses = db.DepartmentCourses.Where(d => d.DeptID == id).Select(a => a.Course);
            return View(department);
        }

        [AuthFilter]
        // GET: Departments/Create
        public ActionResult Create()
        {
            return View();
        }

        [AuthFilter]
        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeptName")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(department);
        }

        [AuthFilter]
        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        [AuthFilter]
        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DeptName")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        [AuthFilter]
        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        [AuthFilter]
        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
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

        [AuthFilter]
        [HttpGet]
        public ActionResult addCourses(int id)
        {
            var allCourses = db.Courses.ToList();
            var deptCourses = db.DepartmentCourses.Where(d => d.DeptID == id).Select(a=> a.Course);
            var courseList = allCourses.Except(deptCourses).ToList();
            ViewBag.dept = db.Departments.FirstOrDefault(d=> d.Id == id);  
            return View(courseList);
        }

        [HttpPost]
        public ActionResult addCourses(int id, Dictionary<string, bool> crs)
        {
            foreach (var item in crs)
            {
                if (item.Value == true)
                {
                    db.DepartmentCourses.Add(new DepartmentCourse() { DeptID = id, CourseID = int.Parse(item.Key) });
                }
            }
            db.SaveChanges();
            return RedirectToAction("Details/" + id, "Departments");
        }

        [HttpGet]
        [AuthFilter]
        public ActionResult deleteCourses(int id)
        {
            var deptCourses = db.DepartmentCourses.Where(d => d.DeptID == id).Select(a => a.Course).ToList();
            ViewBag.dept = db.Departments.FirstOrDefault(d => d.Id == id);
            return View(deptCourses);
        }

        [HttpPost]
        public ActionResult deleteCourses(int id, Dictionary<string, bool> crs)
        {
            foreach (var item in crs)
            {
                if (item.Value == true)
                {
                    int x = int.Parse(item.Key);
                    var c = db.DepartmentCourses.FirstOrDefault(d => d.DeptID == id && d.CourseID == x);
                    db.DepartmentCourses.Remove(c);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Details/"+ id, "Departments");
        }

        
        public ActionResult manageCourse(int id)
        {
            var deptCourses = db.DepartmentCourses.Where(d => d.DeptID == id).Select(a => a.Course).ToList();
            ViewBag.dept = db.Departments.FirstOrDefault(d => d.Id == id);
            return View(deptCourses);
        }
    }
}
