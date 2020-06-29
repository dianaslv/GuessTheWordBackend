using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Exam.Web.Core.Helpers.Commons;
using Exam.Web.Core.Helpers.Commons.Filters.Implementations;
using Exam.Web.Core.Helpers.Commons.Filters.Interfaces;
using Exam.Web.Core.Models.Entities;
using Exam.Web.Core.Repository.Interfaces;
using Exam.Web.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Exam.Web.Core.Services.Implementations
{
    public class StudentService : IStudentService
    {
         private readonly ILogger<StudentService> m_logger;
        private readonly IStudentRepository m_repository;

        public StudentService(ILogger<StudentService> logger, IStudentRepository repository)
        {
            m_logger = logger;
            m_repository = repository;
        }

        public async Task AddAsync(Student Student)
        {
            try
            {
                await m_repository.CreateAsync(Student);
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to create a Student with the properties : {JsonConvert.SerializeObject(Student, Formatting.Indented)}");
                throw;
            }
        }

        public async Task UpdateAsync(Student Student)
        {
            try
            {
                await m_repository.UpdateAsync(new List<Student> {Student});
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to update a Student with the properties : {JsonConvert.SerializeObject(Student, Formatting.Indented)}");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var Student = await SearchAsync(new Pagination(), new SimpleFilter<Student>
                {
                    SearchTerm = id.ToString()
                });
                await m_repository.DeleteAsync(Student.Item2.First());
            }
            catch (ValidationException e)
            {
                m_logger.LogWarning(e, "A validation failed");
                throw;
            }
            catch (Exception e) when (e.GetType() != typeof(ValidationException))
            {
                m_logger.LogCritical(e, $"Unexpected Exception while trying to delete a Student for id : {id}");
                throw;
            }
        }

        public async Task<Tuple<int, List<Student>>> SearchAsync(Pagination pagination, IFilter<Student> filter)
        {
            try
            {
                return await m_repository.SearchAsync(pagination, filter);
            }
            catch (Exception e)
            {
                m_logger.LogCritical(e, "Unexpected Exception while trying to search for Students");
                throw;
            }
        }
    }
}