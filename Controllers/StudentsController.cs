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
    
    public class StudentsController : Controller
    {
        private ITIModel db = new ITIModel();

        // GET: Students
        [AuthFilter]
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Department);
            return View(students.ToList());
        }

        // GET: Students/Details/5
        [AuthFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }
        [AuthFilter]
        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.DeptId = new SelectList(db.Departments, "Id", "DeptName");
            return View();
        }

        [AuthFilter]
        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Age,Email,Password,Img,DeptId")] Student student, HttpPostedFileBase img)
        {

            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    string filename = student.Name + DateTime.Now.Millisecond + "." + img.FileName.Split('.')[1];
                    img.SaveAs(Server.MapPath("~/Imgs/") + filename);
                    student.Img = filename;
                }
                else
                {
                    student.Img = "/noone.png";
                }
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeptId = new SelectList(db.Departments, "Id", "DeptName", student.DeptId);
            return View(student);
        }


        [AuthFilter]
        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeptId = new SelectList(db.Departments, "Id", "DeptName", student.DeptId);
            return View(student);
        }

        [AuthFilter]
        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Age,Email,Password,Img,DeptId")] Student student, HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    string filename = student.Name + DateTime.Now.Millisecond + "." + img.FileName.Split('.')[1];
                    img.SaveAs(Server.MapPath("~/Imgs/") + filename);
                    student.Img = filename;
                }
                else
                {
                    student.Img = "/noone.png";
                }

                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeptId = new SelectList(db.Departments, "Id", "DeptName", student.DeptId);
            return View(student);
        }

        [AuthFilter]
        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [AuthFilter]
        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var isAuth = db.Students.Where(d => d.Email == model.Email && d.Password == model.Password).FirstOrDefault();
                if (isAuth == null)
                {
                    return View(model);
                }

                Session["Email"] = model.Email;
                Session["Password"] = model.Password;
                return RedirectToAction("index", "students");
            }
            else
            {
                return View(model);
            }
        }
    }
}
