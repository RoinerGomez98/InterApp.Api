namespace InterApp.Core.Entities
{
    public class Professor
    {
        public int? Id { get; set; }
        public int? TypeDocument { get; set; }
        public string? Document { get; set; }
        public string? Names { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime? BirtDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Gender { get; set; }
        public bool? Status { get; set; }
        public string? TypeDocumentName { get; set; }
        public IList<Users>? Users { get; set; } = new List<Users>();

    }
}
