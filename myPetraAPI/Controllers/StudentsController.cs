using Microsoft.AspNetCore.Mvc;
using myPetraAPI.Model;

namespace myPetraAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class StudentsController : ControllerBase
    {
        private readonly IStudentsRepository _studentsRepo;

        public StudentsController(IStudentsRepository studentRepo)
        {
            _studentsRepo = studentRepo;
        }

        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Student>>> Search(string name, Gender? gender)
        {
            try
            {
                var result = await _studentsRepo.Search(name, gender);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound("Student not found");

            }catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllStudents()
        {
            try
            {
                return Ok(await _studentsRepo.GetAllStudents());

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Student>> GetStudentById(Guid id)
        {
            try
            {
                var result = await _studentsRepo.GetStudentById(id);

                if(result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }            
        }

        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            try
            {
                if (student == null)
                    return BadRequest();

                var stu = await _studentsRepo.GetStudentByEmail(student.Email);

                if(stu != null)
                {
                    ModelState.AddModelError("Email", "Student email already in use");
                }

                var newStudent = await _studentsRepo.CreateStudent(student);

                return newStudent;

            }catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new student record");
            }
        }

        [HttpPut("(id:guid)")]
        public async Task<ActionResult<Student>> UpdateStudent(Guid id, Student student)
        {
            try
            {
                if(id != student.StudentId)
                {
                    return BadRequest("Student ID mismatch");
                }

                var foundStudent = await _studentsRepo.GetStudentById(id);
                if(foundStudent == null) 
                {
                    return NotFound($"Student with Id = {id} not found");
                }

                return await _studentsRepo.UpdateStudent(student);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating Student record");
            }
        }

        [HttpDelete("id:guid")]
        public async Task<ActionResult> DeleteStudent(Guid id)
        {
            try
            {
                var studentToDelete = await _studentsRepo.GetStudentById(id);

                if (studentToDelete == null)
                {
                    return NotFound($"Student with Id = {id} not found");
                }

                await _studentsRepo.DeleteStudent(id);

                return Ok($"Student with Id = {id} deleted");
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting student record");
            }
        }


    }
}
