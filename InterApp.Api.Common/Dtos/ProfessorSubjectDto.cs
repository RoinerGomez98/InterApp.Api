namespace InterApp.Common.Dtos
{
    public class ProfessorSubjectDto
    {
        public int? Id { get; set; }
        public int? SubjectId { get; set; }
        public int? ProfessorId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Status { get; set; }
        public string? SubjectName { get; set; }
        public string? Professor { get; set; }
        public string? UserCreated { get; set; }
        public int? StudentId { get; set; }
        public ProfessorSubjectDto()
        {
            Id = 0;
            SubjectId = 0;
            ProfessorId = 0;
            StudentId = 0;
        }
    }
}
