namespace InterApp.Core.Entities
{
    public class Registrations
    {
        public int? Id { get; set; }
        public int? ProfessorId { get; set; }
        public int? SubjectId { get; set; }
        public int? StudentId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Status { get; set; }
        public string? SubjectName { get; set; }
        public string? Professor { get; set; }
        public string? Student { get; set; }
    }
}
