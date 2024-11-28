using InterApp.Core.Entities;
using System.Text.Json.Serialization;

namespace InterApp.Common.Dtos
{
    public class ProfessorDto
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
        [JsonIgnore]
        public IList<Users>? Users { get; set; } = new List<Users>();
        public ProfessorDto()
        {
            Id = 0;
        }
    }
}
