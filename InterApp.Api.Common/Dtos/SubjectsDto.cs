namespace InterApp.Common.Dtos
{
    public class SubjectsDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? Credits { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool? Status { get; set; }
        public SubjectsDto()
        {
            Id =0;
        }
    }
}
