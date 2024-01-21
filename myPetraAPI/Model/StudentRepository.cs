using Microsoft.EntityFrameworkCore;
using myPetraAPI.Data;

namespace myPetraAPI.Model
{
    public class StudentRepository : IStudentsRepository
    {

        private readonly AppDbContext _db;

        public StudentRepository(AppDbContext db) 
        {
            this._db = db;
        }

        public async Task<Student> CreateStudent(Student student)
        {
            var result = await _db.Students.AddAsync(student);
            await _db.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteStudent(Guid id)
        {
            var result = await _db.Students.FirstOrDefaultAsync(e => e.StudentId == id);
            if (result != null)
            {
                _db.Students.Remove (result);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            return await _db.Students.ToListAsync();
        }

        public Task<Student> GetStudentById(Guid id)
        {
            return _db.Students.FirstOrDefaultAsync(e => e.StudentId == id);
        }

        public Task<Student> GetStudentByEmail(string email)
        {
            return _db.Students.FirstOrDefaultAsync(e => e.Email == email); 
        }

        public async Task<IEnumerable<Student>> Search(string name, Gender? gender)
        {
            IQueryable<Student> query = _db.Students;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name));  
            }

            if(gender != null)
            {
                query = query.Where(e => e.Gender == gender);
            }

            return await query.ToListAsync();

        }

        public async Task<Student> UpdateStudent(Student student)
        {
            var result = await _db.Students.FirstOrDefaultAsync(e => e.StudentId == student.StudentId);
            
            if (result != null)
            {
                result.FirstName = student.FirstName;
                result.LastName = student.LastName;
                result.Email = student.Email;
                result.Gender = student.Gender;

                await _db.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
