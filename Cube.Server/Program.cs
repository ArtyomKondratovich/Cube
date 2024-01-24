using Cube.Application.Services.Chat;
using Cube.Application.Services.Message;
using Cube.EntityFramework;
using Cube.EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CubeDbContext>(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRepositoryWrapper>(
    options => new RepositoryWrapper(options.GetRequiredService<CubeDbContext>()));
builder.Services.AddScoped<IMessageService>(
    service => new MessageService(service.GetRequiredService<IRepositoryWrapper>()));
builder.Services.AddScoped<IChatService>(
    service => new ChatService(service.GetRequiredService<IRepositoryWrapper>()));


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();