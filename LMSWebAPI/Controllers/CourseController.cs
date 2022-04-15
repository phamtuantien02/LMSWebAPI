using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using LMSWebAPI.Models;

namespace LMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CourseController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("getCourseSlideByID")]
        public JsonResult getCourseSlideByID(CourseIdDTO id)
        {
            
                string query = @"GetCourseSlides";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@CourseId", SqlDbType.Int).Value = id.CourseId;
                       
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            
            return new JsonResult(table);
        }
        [HttpPost]
        [Route("AssignSlideToCourse")]
        public JsonResult AssignSlideToCourse(CourseSlideDTO courseSlide)
        {
            int slideNum = 1;
            foreach (string SlideId in courseSlide.SlideIds)
            {
                string query = @"CreateCourseSlide";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@CourseId", SqlDbType.Int).Value = courseSlide.CourseId;
                        myCommand.Parameters.Add("@SlideId", SqlDbType.Int).Value = SlideId;
                        myCommand.Parameters.Add("@SlideNumber", SqlDbType.Int).Value = slideNum;

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
                slideNum++;
            }
            return new JsonResult("Successfully Assigned Slide!");
        }
        [HttpPost]
        [Route("AssignTestToCourse")]
        public JsonResult AssignTestToCourse(CourseTestDTO courseTest)
        {
            foreach (string TestId in courseTest.TestIds)
            {
                string query = @"CreateCourseTest";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@CourseId", SqlDbType.Int).Value = courseTest.CourseId;
                        myCommand.Parameters.Add("@TestId", SqlDbType.Int).Value = TestId;
                        

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }
            return new JsonResult("Successfully Added");
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"GetCourseInfo";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
        [HttpPost]
        public JsonResult Post(Course course)
        {
            string query = @"CreateCourse";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@CourseCode", SqlDbType.NVarChar).Value = course.CourseCode;
                    myCommand.Parameters.Add("@CourseName", SqlDbType.NVarChar).Value = course.CourseName;
                    myCommand.Parameters.Add("@CourseCredit", SqlDbType.Int).Value = course.CourseCredit;

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }
        [HttpPut]
        public JsonResult Put(Course course)
        {
            string query = @"UpdateCourse";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@CourseID", SqlDbType.Int).Value = course.CourseID;
                    myCommand.Parameters.Add("@CourseCode", SqlDbType.NVarChar).Value = course.CourseCode;
                    myCommand.Parameters.Add("@CourseName", SqlDbType.NVarChar).Value = course.CourseName;
                    myCommand.Parameters.Add("@CourseCredit", SqlDbType.Int).Value = course.CourseCredit;


                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DeleteCourse" ;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@CourseID", SqlDbType.Int).Value = id;

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

        [Route("GetAllCourseNames")]
        public JsonResult GetAllCourseNames()
        {
            string query = @"GetCourseInfo";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
