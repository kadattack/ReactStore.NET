using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PowerReact.Data;
using PowerReact.Entities;
using PowerReact.Middleware;

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


builder.Services.AddCors();


builder.Services.AddIdentityCore<User>().AddRoles<IdentityRole>().AddEntityFrameworkStores<DataContext>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();



var app = builder.Build();



var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
try
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    // context.Database.Migrate();
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


app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(ops =>
{
    ops.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
