namespace myPetraAPI.Model
{
    public interface IStudentsRepository
    {
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student> GetStudentById(Guid id);
        Task<Student> GetStudentByEmail(String Email);
        Task<Student> CreateStudent(Student student);
        Task<Student> UpdateStudent(Student student);
        Task DeleteStudent(Guid id);
        Task<IEnumerable<Student>> Search(string name, Gender? gender);

    }
}

