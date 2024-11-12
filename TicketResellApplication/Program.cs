using DataAccess;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
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
// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IVerificationTokenRepository, VerificationTokenRepository>();
// builder.Services.AddScoped<IUserService, UserService>();
// builder.Services.AddScoped<IJwtService, JwtService>();
// builder.Services.AddScoped<IEmailService, EmailService>();
// builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
// builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
// builder.Services.AddScoped<ICategoryService, CategoryService>();
// builder.Services.AddScoped<ITicketRepository, TicketRepository>();
// builder.Services.AddScoped<ITicketService, TicketService>();
// builder.Services.AddScoped<IPostRepository, PostRepository>();
// builder.Services.AddScoped<IPostService, PostService>();
// builder.Services.AddScoped<IImageTicketRepository, ImageTicketRepository>();
// builder.Services.AddScoped<ITicketRequestRepository, TicketRequestRepository>();
// builder.Services.AddScoped<ITicketRequestService, TicketRequestService>();
// builder.Services.AddScoped<IOrderRepository, OrderRepository>();
// builder.Services.AddScoped<IOrderService, OrderService>();
// builder.Services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
// builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
// builder.Services.AddScoped<IFeedbackService, FeedbackService>();
// builder.Services.AddScoped<IImageFeedbackRepository, ImageFeedbackRepository>();
// builder.Services.AddScoped<IPayOsService, PayOsService>();
// builder.Services.AddScoped<IPlatformFeeRepository, PlatformFeeRepository>();
// builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
// builder.Services.AddScoped<IPayOsService, PayOsService>();
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

PayOS payOS = new PayOS(configuration["PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
    configuration["PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
    configuration["PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));

builder.Services.AddSingleton(payOS);


var applicationAssembly = typeof(Program).Assembly;
var serviceAssembly = typeof(IUserService).Assembly;
var repositoryAssembly = typeof(IUserRepository).Assembly;
builder.Services.AddHttpContextAccessor();


builder.Services.Scan(scan => scan
    .FromAssemblies(applicationAssembly, serviceAssembly, repositoryAssembly)
    .AddClasses(classes => classes.InNamespaces("Service", "Service.Impl", "Repository", "Repository.Impl")) // Thay bằng namespace thực tế
    .AsMatchingInterface()  // Đăng ký các lớp dựa trên interface phù hợp
    .WithScopedLifetime()
);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});



/*builder.Services.AddDbContext<TicketResellDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB")));*/



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


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();

}
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();