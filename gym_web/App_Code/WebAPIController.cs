using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace gym_web
{
    public class WebAPIController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetMessage()
        {
            return Ok("Hello, this is your API response!");
        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public object Get(int? id)
        {
            return new Person
            {
                Name = "John",
                Age = 30,
                Value = id
            };
        }
        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public int? Value { get; set; }
        }
        [HttpGet]
        [Route("api/schedule/{coachId}/{date}")]
        public IHttpActionResult GetSchedule(int coachId, string date)
        {
            List<ScheduleItem> schedule = new List<ScheduleItem>();
            string connectionString = "Data Source=203.64.129.17;Initial Catalog=gym;User ID=sqlserver;Password=sqlserver";
            string query = "SELECT [健身教練課程].課程編號, [健身教練課程].課程名稱 FROM [健身教練課表] JOIN [健身教練課程] ON [健身教練課程].課程編號 = [健身教練課表].課程編號 WHERE [健身教練課表].日期 = @date AND [健身教練課程].健身教練編號 = @coachId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@coachId", coachId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    schedule.Add(new ScheduleItem
                    {
                        CourseId = reader.GetInt32(0),
                        CourseName = reader.GetString(1)
                    });
                }
            }
            return Ok(schedule);
        }
        public class ScheduleItem
        {
            public int CourseId { get; set; }
            public string CourseName { get; set; }
        }
        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}