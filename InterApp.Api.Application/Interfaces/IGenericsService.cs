using InterApp.Common.Dtos;
using InterApp.Common.Utils;

namespace InterApp.Api.Application.Implementations
{
    public interface IGenericsService
    {
        Task<GenericResponse<IEnumerable<TypeDocumentsDto>>> GetTypeDocuments(TypeDocumentsDto typeDocuments);
    }
}
