using InterApp.Api.Application.Implementations;
using InterApp.Api.Application.Interfaces;
using InterApp.Core.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace InterApp.Api.DI.IoC
{
    public static class IoCRegister
    {
        public static IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddScoped<IGenericsService, GenericsService>();
            services.AddScoped<IProfessorService, ProfessorService>();
            services.AddScoped<IProfessorSubjectService, ProfessorSubjectService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
        public static IServiceCollection AddRespository(IServiceCollection services)
        {
            services.AddSingleton<RepoInter>();
            return services;
        }
        public static IServiceCollection AddConnection(IServiceCollection services, string defaultConnection)
        {
            services.AddSingleton<IDbConnection>(_ => new SqlConnection(defaultConnection));
            return services;
        }

    }
}
