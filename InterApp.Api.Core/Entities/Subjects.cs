namespace InterApp.Core.Entities
{
    public class Subjects
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? Credits { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool? Status { get; set; }
    }
}
