using System.Security.Claims;
using System.Text;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Repository;
using Repository.Impl;
using Service;
using Service.Impl;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Your API", Version = "v1" });

    // Configure Swagger to use the Bearer token
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
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

builder.Services.AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(option =>
    {
        option.SaveToken = true;
        option.RequireHttpsMetadata = false;
        option.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            RoleClaimType = ClaimTypes.Role
        };
    });

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
// builder.Services.AddTransient<ITimedBackgroundService, TimedBackgroundService>();

builder.Services.AddHostedService<TicketBackgroundService>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();