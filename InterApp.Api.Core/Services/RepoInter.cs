using Dapper;
using InterApp.Core.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InterApp.Core.Services
{
    public class RepoInter
    {
        private readonly IDbConnection _connection;
        public RepoInter(IDbConnection dbConnection)
        {
            _connection = dbConnection;
        }

        #region Students
        public async Task<IEnumerable<Students>> GetStudents(Students students)
        {
            DynamicParameters parameters = new();
            parameters.Add(nameof(students.Id), students.Id);
            parameters.Add(nameof(students.Document), students.Document);
            parameters.Add(nameof(students.Names), students.Names);
            parameters.Add(nameof(students.Status), students.Status);

            using var conn = new SqlConnection(_connection.ConnectionString);
            return await conn.QueryAsync<Students>("SP_GET_STUDENTS", parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<int> SaveStudents(Students students)
        {
            using var conn = new SqlConnection(_connection.ConnectionString);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            var save = 0;
            using (var transaction = conn.BeginTransaction())
            {
                DynamicParameters parameters = new();
                parameters.Add("Id", students.Id);
                parameters.Add("TypeDocument", students.TypeDocument);
                parameters.Add("Document", students.Document!.Trim());
                parameters.Add("Names", students.Names!.Trim());
                parameters.Add("LastName", students.LastName!.Trim());
                parameters.Add("Email", students.Email!.Trim());
                parameters.Add("BirtDate", students.BirtDate);
                parameters.Add("Gender", students.Gender!.Trim());
                parameters.Add("Status", students.Status);

                save = await conn.ExecuteAsync("SP_INSERT_UPDATE_STUDENTS", parameters, transaction, commandType: CommandType.StoredProcedure);
                if (save > 0 && students.Users!.Count > 0)
                {
                    save = await conn.ExecuteAsync("SP_INSERT_UPDATE_USERS", students.Users, transaction, commandType: CommandType.StoredProcedure);
                }
                if (save > 0)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
            conn.Close();
            return save;
        }
        #endregion

        #region Professors
        public async Task<IEnumerable<Professor>> GetProfessors(Professor professor)
        {
            DynamicParameters parameters = new();
            parameters.Add(nameof(professor.Id), professor.Id);
            parameters.Add(nameof(professor.Document), professor.Document);
            parameters.Add(nameof(professor.Names), professor.Names);
            parameters.Add(nameof(professor.Status), professor.Status);

            using var conn = new SqlConnection(_connection.ConnectionString);
            return await conn.QueryAsync<Professor>("SP_GET_PROFESSOR", parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<int> SaveProfessors(Professor professor)
        {
            using var conn = new SqlConnection(_connection.ConnectionString);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            var save = 0;
            using (var transaction = conn.BeginTransaction())
            {
                DynamicParameters parameters = new();
                parameters.Add("Id", professor.Id);
                parameters.Add("TypeDocument", professor.TypeDocument);
                parameters.Add("Document", professor.Document);
                parameters.Add("Names", professor.Names);
                parameters.Add("LastName", professor.LastName);
                parameters.Add("Email", professor.Email);
                parameters.Add("BirtDate", professor.BirtDate);
                parameters.Add("Gender", professor.Gender);
                parameters.Add("Status", professor.Status);

                save = await conn.ExecuteAsync("SP_INSERT_UPDATE_PROFESSORS", parameters, transaction, commandType: CommandType.StoredProcedure);
                if (save > 0 && professor.Users!.Count > 0)
                {
                    save = await conn.ExecuteAsync("SP_INSERT_UPDATE_USERS", professor.Users, transaction, commandType: CommandType.StoredProcedure);
                }
                if (save > 0)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
            conn.Close();
            return save;
        }
        #endregion

        #region Subjects
        public async Task<IEnumerable<Subjects>> GetSubjects(Subjects subjects)
        {
            DynamicParameters parameters = new();
            parameters.Add(nameof(subjects.Id), subjects.Id);
            parameters.Add(nameof(subjects.Name), subjects.Name);
            parameters.Add(nameof(subjects.Status), subjects.Status);

            using var conn = new SqlConnection(_connection.ConnectionString);
            return await conn.QueryAsync<Subjects>("SP_GET_SUBJECTS", parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<int> SaveSubjects(Subjects subjects)
        {
            using var conn = new SqlConnection(_connection.ConnectionString);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            var save = 0;
            using (var transaction = conn.BeginTransaction())
            {
                save = await conn.ExecuteAsync("SP_INSERT_UPDATE_SUBJECTS", subjects, transaction, commandType: CommandType.StoredProcedure);
                if (save > 0)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
            conn.Close();
            return save;
        }
        #endregion


        #region Users
        public async Task<IEnumerable<Users>> GetUsers(Users users)
        {
            DynamicParameters parameters = new();
            parameters.Add(nameof(users.Id), users.Id);
            parameters.Add(nameof(users.Document), users.Document);
            parameters.Add(nameof(users.Password), users.Password);
            parameters.Add(nameof(users.Email), users.Email);

            using var conn = new SqlConnection(_connection.ConnectionString);
            return await conn.QueryAsync<Users>("SP_GET_USERS_INT", parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<int> SaveUsers(Users users)
        {
            using var conn = new SqlConnection(_connection.ConnectionString);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            var save = 0;
            using (var transaction = conn.BeginTransaction())
            {
                save = await conn.ExecuteAsync("SP_INSERT_UPDATE_USERS", users, transaction, commandType: CommandType.StoredProcedure);
                if (save > 0)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
            conn.Close();
            return save;
        }
        #endregion

        #region ProfessorSubject
        public async Task<IEnumerable<ProfessorSubject>> GetProfessorSubjects(ProfessorSubject professorSubject,int StudentId = 0)
        {
            DynamicParameters parameters = new();
            parameters.Add(nameof(professorSubject.Id), professorSubject.Id);
            parameters.Add(nameof(professorSubject.SubjectId), professorSubject.SubjectId);
            parameters.Add(nameof(professorSubject.ProfessorId), professorSubject.ProfessorId);
            parameters.Add(nameof(StudentId), StudentId);

            using var conn = new SqlConnection(_connection.ConnectionString);
            return await conn.QueryAsync<ProfessorSubject>("SP_GET_PROFESSORSUBJECTS", parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<int> SaveProfessorSubjects(IEnumerable<ProfessorSubject> professorSubject)
        {
            using var conn = new SqlConnection(_connection.ConnectionString);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            var save = 0;
            using (var transaction = conn.BeginTransaction())
            {
                var dl = await conn.ExecuteAsync($"DELETE FROM ProfessorSubject WHERE SubjectId ={professorSubject.FirstOrDefault()!.SubjectId}", professorSubject, transaction, commandType: CommandType.Text);
                save = await conn.ExecuteAsync("SP_INSERT_UPDATE_PROFESSORSUBJECTS", professorSubject, transaction, commandType: CommandType.StoredProcedure);
                if (save > 0)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
            conn.Close();
            return save;
        }
        #endregion


        #region Registrations
        public async Task<IEnumerable<Registrations>> GetRegistrations(Registrations registrations)
        {
            DynamicParameters parameters = new();
            parameters.Add(nameof(registrations.Id), registrations.Id);
            parameters.Add(nameof(registrations.StudentId), registrations.StudentId);
            parameters.Add(nameof(registrations.SubjectId), registrations.SubjectId);
            parameters.Add(nameof(registrations.ProfessorId), registrations.ProfessorId);

            using var conn = new SqlConnection(_connection.ConnectionString);
            return await conn.QueryAsync<Registrations>("SP_GET_REGISTRATIONS", parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<int> SaveRegistrations(IEnumerable<Registrations> registrations)
        {
            using var conn = new SqlConnection(_connection.ConnectionString);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            var save = 0;
            using (var transaction = conn.BeginTransaction())
            {
                save = await conn.ExecuteAsync("SP_INSERT_UPDATE_REGISTRATIONS", registrations, transaction, commandType: CommandType.StoredProcedure);
                if (save > 0)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
            conn.Close();
            return save;
        }
        #endregion


        #region TypeDocuments
        public async Task<IEnumerable<TypeDocuments>> GetTypeDocuments(TypeDocuments typeDocuments)
        {
            DynamicParameters parameters = new();
            parameters.Add(nameof(typeDocuments.Id), typeDocuments.Id);
            parameters.Add(nameof(typeDocuments.Code), typeDocuments.Code);
            parameters.Add(nameof(typeDocuments.Name), typeDocuments.Name);
            using var conn = new SqlConnection(_connection.ConnectionString);
            return await conn.QueryAsync<TypeDocuments>("SP_GET_TYPESDOCUMENTS", parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

    }
}
