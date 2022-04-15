using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using LMSWebAPI.Models;
using System.Threading;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

namespace LMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationSettings _appSettings;

        public StudentController(IConfiguration configuration,IOptions<ApplicationSettings> appSettings)
        {
            this._configuration = configuration;
            this._appSettings = appSettings.Value;
        }

        [HttpGet]
        [Authorize]
        [Route("getStudentInfoByID2")]
        public JsonResult getStudent2()
        {
            int userId = Int32.Parse(HttpContext.Items["UserId"].ToString());
            string query = @"GetStudentInfoById";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@StudID", SqlDbType.Int).Value = userId;


                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            Student returnStudent = new Student();
            returnStudent.FirstName = table.Rows[0][0].ToString();
            returnStudent.LastName = table.Rows[0][1].ToString();
            returnStudent.StudID = Int32.Parse(table.Rows[0][2].ToString());

            return new JsonResult(returnStudent);
        }

        [HttpGet]
        [Authorize]
        [Route("getStudentInfoByID")]
        public JsonResult getStudentInfoByID()
        {
            string query = @"GetStudentInfoById";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@StudID", SqlDbType.Int).Value = HttpContext.Items["UserId"];
                  

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            Student returnStudent = new Student();
            returnStudent.FirstName = table.Rows[0][0].ToString();
            returnStudent.LastName = table.Rows[0][1].ToString();
            return new JsonResult(returnStudent);
        }

        [HttpGet]
        [Authorize]
        [Route("getStudentCourse")]
        public JsonResult getStudentCourse()
        {
            string query = @"GetStudentCourses";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@StudID", SqlDbType.Int).Value = HttpContext.Items["UserId"].ToString();
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]

        public JsonResult Get()
        {
            string query = @"GetStudentInfo";
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
        [Route("assignCourse")]
        public JsonResult assignCourse(StudentCourse stu)
        {
            foreach(string CourseId in stu.CourseIds)
            {
                string query = @"CreateStudentCourse";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@CourseID", SqlDbType.Int).Value = CourseId;
                        myCommand.Parameters.Add("@StudID", SqlDbType.Int).Value = stu.StudID; 

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }
            return new JsonResult("Successfully Added");
        }
        
        [HttpPost]
        [Route("login")]
        public JsonResult Getlogin(StudentLoginDTO stu)
        {
            //Thread.Sleep(5000);
            string query = @"AuthenticateUser";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@Username", SqlDbType.NVarChar).Value = stu.UserName;
                    myCommand.Parameters.Add("@Password", SqlDbType.NVarChar).Value = stu.PassWord;

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            if (table.Rows.Count == 1)
            {

                AuthResponse returnStudent = new AuthResponse();
                returnStudent.StudID = Int32.Parse(table.Rows[0][0].ToString());
                returnStudent.FirstName = table.Rows[0][1].ToString();
                returnStudent.LastName = table.Rows[0][2].ToString();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim("UserID", returnStudent.StudID.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                JwtTokenResponse returnToken = new JwtTokenResponse();
                returnStudent.token = token;

                return new JsonResult(returnStudent);
            }
            return new JsonResult(new { message = "Invalid login" }) { StatusCode = StatusCodes.Status403Forbidden };
        }

        [HttpPost]
        public JsonResult Post(Student stu)
        {
            string query = @"RegisterStudent";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@Username", SqlDbType.NVarChar).Value = stu.UserName;
                    myCommand.Parameters.Add("@Password", SqlDbType.NVarChar).Value = stu.PassWord;
                    myCommand.Parameters.Add("@Firstname", SqlDbType.NVarChar).Value = stu.FirstName;
                    myCommand.Parameters.Add("@Lastname", SqlDbType.NVarChar).Value = stu.LastName;
                    myCommand.Parameters.Add("@Email", SqlDbType.NVarChar).Value = stu.Email;
                
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Student added successfully!");
        }

        [HttpPut]
        public JsonResult Put(Student stu)
        {
            string query = @"UpdateStudentInfo";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@StudID", SqlDbType.Int).Value = stu.StudID;
                    myCommand.Parameters.Add("@Username", SqlDbType.NVarChar).Value = stu.UserName;
                    myCommand.Parameters.Add("@Password", SqlDbType.NVarChar).Value = stu.PassWord;
                    myCommand.Parameters.Add("@Firstname", SqlDbType.NVarChar).Value = stu.FirstName;
                    myCommand.Parameters.Add("@Lastname", SqlDbType.NVarChar).Value = stu.LastName;
                    myCommand.Parameters.Add("@Email", SqlDbType.NVarChar).Value = stu.Email;

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
            string query = @"DeleteStudent";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LMSAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@StudID", SqlDbType.Int).Value = id;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }
    }
}
