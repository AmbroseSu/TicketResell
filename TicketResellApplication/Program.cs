using DataAccess;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Impl;
using Service;
using Service.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVerificationTokenRepository, VerificationTokenRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IImageTicketRepository, ImageTicketRepository>();
builder.Services.AddScoped<ITicketRequestRepository, TicketRequestRepository>();
builder.Services.AddScoped<ITicketRequestService, TicketRequestService>();


/*builder.Services.AddDbContext<TicketResellDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB")));*/


var app = builder.Build();
// using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
// {
//     var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
//     var db = serviceScope.ServiceProvider.GetRequiredService<TicketResellDbContext>().Database;
//
//     logger.LogInformation("Migrating database...");
//
//     while (!db.CanConnect())
//     {
//         logger.LogInformation("Database not ready yet; waiting...");
//         Thread.Sleep(1000);
//         logger.LogInformation(db.GetConnectionString());
//     }
//
//     try
//     {
//         serviceScope.ServiceProvider.GetRequiredService<TicketResellDbContext>().Database.Migrate();
//         logger.LogInformation("Database migrated successfully.");
//     }
//     catch (Exception ex)
//     {
//         logger.LogError(ex, "An error occurred while migrating the database.");
//     }
// }
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();

}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();