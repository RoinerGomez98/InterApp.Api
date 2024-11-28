namespace InterApp.Common.Dtos
{
    public class TypeDocumentsDto
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public TypeDocumentsDto()
        {
            Id = 0;
        }
    }
}
