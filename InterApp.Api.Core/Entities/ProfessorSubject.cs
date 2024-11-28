namespace InterApp.Core.Entities
{
    public class ProfessorSubject
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
    }
}
