using Cube.EntityFramework.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cube.EntityFramework
{
    public static class RepositoryExtensions
    {
        public static void ConfigureRepository(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<CubeDbContext>(
                options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IRepositoryWrapper>(
                options => new RepositoryWrapper(options.GetRequiredService<CubeDbContext>()));
        }
    }
}
