using AutoMapper;
using InterApp.Common.Dtos;
using InterApp.Common.Utils;
using InterApp.Core.Entities;
using InterApp.Core.Services;
using System.Net;

namespace InterApp.Api.Application.Implementations
{
    public class GenericsService : IGenericsService
    {
        private readonly RepoInter _repoInt;
        private readonly IMapper _mapper;
        public GenericsService(RepoInter repoInt, IMapper mapper)
        {
            _repoInt = repoInt;
            _mapper = mapper;
        }

        #region TypeDocuments
        public async Task<GenericResponse<IEnumerable<TypeDocumentsDto>>> GetTypeDocuments(TypeDocumentsDto typeDocuments)
        {
            var cast = _mapper.Map<TypeDocuments>(typeDocuments);
            var result = await _repoInt.GetTypeDocuments(cast);
            GenericResponse<IEnumerable<TypeDocumentsDto>> response = new()
            {
                Result = _mapper.Map<IEnumerable<TypeDocumentsDto>>(result),
                Status = HttpStatusCode.OK
            };
            return response;

        }
        #endregion

    }
}
