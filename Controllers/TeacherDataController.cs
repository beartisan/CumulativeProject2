using CumulativeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using Renci.SshNet.Security.Cryptography;
///using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CumulativeProject.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        //public int TeacherId { get; private set; }

        /// <summary>
        /// Returns a list of information about teachers
        /// </summary>
        /// <example> GET api/TeacherData/ListTeachers </example>
        /// <returns>
        /// List of Teachers
        /// {teacherFname, TeacherLname, EmployeeNumber, TeacherId, Salary, HireDate}
        /// </returns>

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey=null)
        {
            //Create a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the Connection
            Conn.Open();

            //Create a new command for the database
            MySqlCommand cmd = Conn.CreateCommand();

            //Sql query
            cmd.CommandText = "Select * from Teachers where teacherfname like @key or teacherlname like @key or lower(concat(teacherfname, ' ', teacherlname)) like @key or salary like @key or hiredate like @key or employeenumber like @key";

            //clean and sanitize
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();


            MySqlDataReader ResultSet = cmd.ExecuteReader();

           

            List<Teacher> Teachers = new List<Teacher> ();

            while (ResultSet.Read())
            {
                //string teacher= ResultSet["teacherfname"] + " " + ResultSet["teacherlname"];
                int TeacherId = Convert.ToInt32(ResultSet["TeacherId"]);
                string TeacherFname = ResultSet["TeacherFname"].ToString();
                string TeacherLname = ResultSet["TeacherLname"].ToString();
                string EmployeeNumber = ResultSet["EmployeeNumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["HireDate"]);
                decimal Salary = Convert.ToDecimal(ResultSet["Salary"].ToString());


                Teacher NewTeacher = new Teacher();

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

                Teachers.Add(NewTeacher);
            }

            //Close the connection
            Conn.Close();


            return Teachers;

        }

        /// /////////////////////       FIND TEACHERS         ///////////////////////
        ///<summary>
        ///Returns an individual author from the database by specifying the primary key teacherid
        /// </summary>
        /// <param name="id"> gives the teacher's ID in the database
        /// </param>
        /// <return>Returns the Teacher object
        /// </return>
        ///<example>
        /////GET /api/TeacherData/FindTeacher/2
        /// </example>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{id}")]

        

        public Teacher FindTeacher(int id)
        {

            Teacher SelectedTeacher = new Teacher();

            //Create a connection
            MySqlConnection Conn = School.AccessDatabase();


            //Open the connection
            Conn.Open();

            //Establish a new command query for our db
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "Select * from teachers where TeacherId=@id";

            // cmd.CommandText = query;
            //sanitize the teacherId input
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather ResultSet of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();


            //while loop
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = (int)ResultSet["TeacherId"];
                string TeacherFname = ResultSet["TeacherFname"].ToString();
                string TeacherLname = ResultSet["TeacherLname"].ToString();
                DateTime HireDate = (DateTime)ResultSet["HireDate"];


                SelectedTeacher.TeacherId = Convert.ToInt32(ResultSet["TeacherId"]);
                SelectedTeacher.TeacherFname = ResultSet["TeacherFname"].ToString();
                SelectedTeacher.TeacherLname = ResultSet["TeacherLname"].ToString();
                SelectedTeacher.EmployeeNumber = ResultSet["EmployeeNumber"].ToString();
                SelectedTeacher.HireDate = Convert.ToDateTime(ResultSet["HireDate"]);
                SelectedTeacher.Salary = Convert.ToDecimal(ResultSet["Salary"].ToString());

            }
            
            Conn.Close();


            return SelectedTeacher;
        }

        ///  /////////////////////////       ADD TEACHERS           ///////////////////////////////////
        ///<summary>
        ///adds a teacher into the system (MySql Database)
        /// </summary>
        ///<param name="NewTeacher">
        ///Add a Teacher object input
        ///</param>
        ///<returns>
        ///POST api/TeacherData/AddTeacher
        ///Form Data / Post Data / Request
        ///{
        /// TeacherFname: "Severus",
        /// TeacherLname: "Snape",
        /// EmployeeNumber: "T394",
        /// HireDate: "July 31, 1975",
        /// Salary: "83.19"
        ///}
        ///</returns>

        [HttpPost] //change post request
        
        public void AddTeacher(Teacher NewTeacher)
        {

            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            //query

            //string query = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) " +
                // "values (@TeacherFname, @TeacherLname, @EmployeeNumber, CURRENT_DATE(), @Salary)";
            ///CURRENT_TIME(), 0

            MySqlCommand cmd = Conn.CreateCommand();
            //cmd.CommandText = query;

            cmd.CommandText = "insert into teachers (TeacherFname, TeacherLname, EmployeeNumber, HireDate, Salary) values (@TeacherFname, @TeacherLname, @EmployeeNumber, @HireDate, @Salary)";

            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@TeacherId", NewTeacher.TeacherId);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber); //add salary field
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //Debug.WriteLine("Trying to delete teacher with Salary " + @Salary);

            Conn.Close();


            //insert into Teacher(TeacherFname, TeacherLname, EmployeeNumber) values (@TeacherFname, @TeacherLname, @EmployeeNumber)
        }

  ///  /////////////////////        DELETE TEACHERS       ///////////////////////
        /// <summary>
        /// Deletes a teacher in the system given the teacher id
        /// </summary>
        /// <param name="id">The primary key of the teacher to delete</param>
        /// <returns>
        /// </returns>
        /// <example>
        /// POST api/TeacherData/DeleteTeacher/{id}
        /// POST api/TeacherData/DeleteTeacher/8
        /// </example>

        [HttpPost]
        //[Route("api/TeacherData/DeleteTeacher/{id}")]
        public void DeleteTeacher(int id)
        {
            Debug.WriteLine("Trying to delete teacher with id " + id);
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            //string query = "Select * from teachers where id=@id";


            MySqlCommand cmd = Conn.CreateCommand();
            // cmd.CommandText = query;

            cmd.CommandText = "Delete from teachers where TeacherId=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();


            cmd.ExecuteNonQuery();

            Conn.Close();

            //query

        }
    }
}
