namespace InterApp.Core.Entities
{
    public class Users
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Document { get; set; }
        public int? TypeUser { get; set; }
        public int? ProfessorId { get; set; }
        public int? StudentId { get; set; }
        public string? Password { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Status { get; set; }
    }
}
