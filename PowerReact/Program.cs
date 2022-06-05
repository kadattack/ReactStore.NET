using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PowerReact.Data;
using PowerReact.Entities;

var builder = WebApplication.CreateBuilder(args);

// this config are just variables from appsettings.Development.json
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PowerReact.Data.DataContext>(opt =>
{
    opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityCore<User>().AddRoles<IdentityRole>().AddEntityFrameworkStores<DataContext>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();



var app = builder.Build();



var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
try
{
    context.Database.Migrate();
    DbInitializer.Initialize(context);
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
finally
{
    scope.Dispose();
}

// var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
