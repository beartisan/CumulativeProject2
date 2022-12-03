using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CumulativeProject.Models;

namespace CumulativeProject.Controllers
{
    public class TeacherController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        // GET: Teacher/List?SearchKey={value}

        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController MyController = new TeacherDataController();
            IEnumerable<Teacher> MyTeacher = MyController.ListTeachers(SearchKey);

            //console.log stuff
            Debug.WriteLine("SearchKey is " + SearchKey);
            Debug.WriteLine("I have accessed " + MyTeacher.Count());

            return View(MyTeacher);
        }

        // GET: Teacher/Show/{TeacherId}
        public ActionResult Show(int id)
        {
            TeacherDataController MyController = new TeacherDataController();
            Teacher SelectedTeacher = MyController.FindTeacher(id);

            return View(SelectedTeacher);
        }

        //GET: /Teacher/Add
        public ActionResult Add()
        {
            return View();
        }

        //POST: /Teacher/Create
        /// <summary>
        /// links it to teacher/Add
        /// </summary>
        /// <param name="TeacherFname">Teacher's First Name</param>
        /// <param name="TeacherLname">Teacher's Last Name</param>
        /// <param name="EmployeeNumber">Teacher's Employee Number</param>
        /// <param name="HireDate">Teacher's join date</param>
        /// <param name="Salary">shows Teacher's salary per hour</param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, decimal Salary)
        {
            Debug.WriteLine("I am trying to create a new Teacher with " + TeacherFname);

            Teacher NewTeacher = new Teacher();

            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = HireDate;
            NewTeacher.Salary = Salary;

            TeacherDataController MyController = new TeacherDataController();

            MyController.AddTeacher(NewTeacher);


            //redirect to List of teachers
            return RedirectToAction("List");
        }


        //GET: /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController MyController = new TeacherDataController();
            Teacher SelectedTeacher = MyController.FindTeacher(id);

            return View(SelectedTeacher);
        }

        //POST: /Teacher/Delete/{id}
        public ActionResult Delete(int id)
        {
            TeacherDataController MyController = new TeacherDataController();
            MyController.DeleteTeacher(id);

            return RedirectToAction("List");
        }
    }
}