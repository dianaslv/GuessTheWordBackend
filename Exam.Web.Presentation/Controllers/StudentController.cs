using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Implementations;
using Exam.Web.Core.Helpers.Network.Models;
using Exam.Web.Core.Models.Entities;
using Exam.Web.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Exam.Web.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController
    {
        private readonly IStudentService m_StudentService;
        private readonly ILogger<StudentController> m_logger;

        public StudentController(IStudentService StudentService, ILogger<StudentController> logger)
        {
            m_StudentService = StudentService;
            m_logger = logger;
        }

        [HttpGet]
        public async Task<List<Student>> GetStudentsAsync([FromQuery(Name = "term")] string searchTerm)
        {
            var data = await m_StudentService.SearchAsync(new Pagination(), new StudentFilter
            {
                SearchTerm = searchTerm
            });

            return data.Item2;
        }

        [HttpPost("login")]
        public async Task<Student> LoginStudent([FromBody] LoginRequest loginRequest)
        {
            var data = await m_StudentService.SearchAsync(new Pagination(), new StudentFilter
            {
                SearchTerm = loginRequest.Username
            });

            return data.Item2.FirstOrDefault(t => t.Password.Equals(loginRequest.Password));
        }

        [HttpPost]
        public async Task AddAsync([FromBody] Student Student)
        {
            await m_StudentService.AddAsync(Student);
        }

        [HttpPut]
        public async Task UpdateAsync([FromBody] IEnumerable<Student> Students)
        {
            foreach (var Student in Students)
            {
                await m_StudentService.UpdateAsync(Student);
            }
        }

        [HttpDelete]
        public async Task DeleteAsync([FromBody] Guid id)
        {
            await m_StudentService.DeleteAsync(id);
        }
    }
}