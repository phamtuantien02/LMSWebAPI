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
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TestController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("getTestByTestID")]

        public JsonResult getTestByTestID(TestIdDTO test)
        {
            string query = @"GetAllTestQuestionAndAnswerByTestID";
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");

            var t5 = new Questions();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@TestID", SqlDbType.Int).Value = test.testID;

                    SqlDataReader reader = myCommand.ExecuteReader();

                    

                    if (reader.HasRows)
                    {
                        int currentquestionID = 0;
                        while (reader.Read())
                        {

                            if(currentquestionID != reader.GetInt32(0))
                            {
                                t5.questions.Add(new Question(reader.GetString(1)));
                                currentquestionID = reader.GetInt32(0);

                            }
                            var q = t5.questions.LastOrDefault();   
                            q.options.Add(new AnswerOptions(reader.GetString(2),reader.GetBoolean(3)));
                            
                        }
                    }

                }
            }

            return new JsonResult(t5);
        }

        [HttpPost]
        [Route("getTestInfoByCourseID")]
        public JsonResult getTestInfoByCourseID(CourseIdDTO course)
        {
            string query = @"GetAllTestInfoByCourseID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@CourseID", SqlDbType.Int).Value = course.CourseId;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
        [HttpPost]
        [Route("AssignQuestion")]
        public JsonResult AssignQuestions(AssignTestQuestionDTO test)
        {
            foreach (string questionID in test.QuestionIds)
            {
                string query = @"AssignTestQuestion";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@TestID", SqlDbType.Int).Value = test.TestId;
                        myCommand.Parameters.Add("@TestQuestionID", SqlDbType.Int).Value =  questionID;


                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }
            return new JsonResult("Added Successfully");
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"GetAllTestInfo";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
        [HttpPost]
        public JsonResult Post(Test test)
        {
            string query = @"CreateTest";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@NumberOfQuestions", SqlDbType.Int).Value = test.numQuest;
                    myCommand.Parameters.Add("@AttemptAllowed", SqlDbType.Int).Value = test.testAttempts;
                    myCommand.Parameters.Add("@PassingScore", SqlDbType.Int).Value = test.passingScore;

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }
        [HttpPut]
        public JsonResult Put(Test test)
        {
            string query = @"UpdateTest";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@TestID", SqlDbType.Int).Value = test.testID;
                    myCommand.Parameters.Add("@NumberOfQuestions", SqlDbType.Int).Value = test.numQuest;
                    myCommand.Parameters.Add("@AttemptAllowed", SqlDbType.Int).Value = test.testAttempts;
                    myCommand.Parameters.Add("@PassingScore", SqlDbType.Int).Value = test.passingScore;

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
            string query = @"DeleteTest";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@TestID", SqlDbType.Int).Value = id;

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

        [Route("GetAllTest")]
        public JsonResult GetAllCourseNames()
        {
            string query = @"GetAllTestInfo";
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
