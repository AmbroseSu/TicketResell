using System.Diagnostics;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataAccess;

public class TicketResellDbContext : DbContext
{

    public TicketResellDbContext() {}
    
    public TicketResellDbContext(DbContextOptions<TicketResellDbContext> options) : base(options) { }

    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<CartItem> CartItems { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Chat> Chats { get; set; }
    public virtual DbSet<Feedback> Feedbacks { get; set; }
    public virtual DbSet<ImageTicket> ImageTickets { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
    public virtual DbSet<PlatformFee> PlatformFees { get; set; }
    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<Ticket> Tickets { get; set; }
    public virtual DbSet<TicketRequest> TicketRequests { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserChat> UserChats { get; set; }
    public virtual DbSet<VerificationToken> VerificationTokens { get; set; }
    private static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => 
    {
        builder.AddConsole(); 
    });

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(GetConnectionString())
                .EnableSensitiveDataLogging() // Bật log dữ liệu nhạy cảm
                .UseLoggerFactory(MyLoggerFactory) // Kích hoạt logger
                .EnableDetailedErrors(); // Hiển thị lỗi chi tiết;
        }
    }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var strConn = config.GetConnectionString("DB");

        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AddressName);
            entity.Property(e => e.IsDeleted);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreateAt);
            entity.Property(e => e.UpdateAt);
            entity.Property(e => e.IsDeleted);
            
            entity.HasMany(e => e.CartItems)
                .WithOne(e => e.Cart)
                .HasForeignKey(e => e.CartId);
            //entity.HasOne(e => e.User)
            //    .WithOne(e => e.Cart)
            //    .HasForeignKey<User>(e => e.CartId);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("CartItem");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity);
            entity.Property(e => e.IsDeleted);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name);
            entity.Property(e => e.IsDeleted);
            
            entity.HasMany(e => e.Tickets)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId);
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.ToTable("Chat");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name);
            entity.Property(e => e.IsDeleted);
            
            entity.HasMany(e => e.UserChats)
                .WithOne(e => e.Chat)
                .HasForeignKey(e => e.ChatId);
            entity.HasMany(e => e.Messages)
                .WithOne(e => e.Chat)
                .HasForeignKey(e => e.ChatId);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.ToTable("Feedback");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Rating);
            entity.Property(e => e.Context);
            entity.Property(e => e.IsDeleted);
        });

        modelBuilder.Entity<ImageTicket>(entity =>
        {
            entity.ToTable("ImageTicket");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ImageUrl);
            entity.Property(e => e.IsDeleted);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Message");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content);
            entity.Property(e => e.SendAt);
            entity.Property(e => e.Status);
            entity.Property(e => e.IsDeleted);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price);
            entity.Property(e => e.Quantity);
            entity.Property(e => e.Address);
            entity.Property(e => e.OrderDate);
            entity.Property(e => e.IsDeleted);
            
            entity.HasMany(e => e.OrderStatuses)
                .WithOne(e => e.Order)
                .HasForeignKey(e => e.OrderId);
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.ToTable("OrderStatus");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name);
            entity.Property(e => e.Date);
        });

        modelBuilder.Entity<PlatformFee>(entity =>
        {
            entity.ToTable("PlatformFee");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name);
            entity.Property(e => e.Quantity);
            entity.Property(e => e.IsDeleted);
            
            entity.HasMany(e => e.Transactions)
                .WithOne(e => e.PlatformFee)
                .HasForeignKey(e => e.PlatformFeeId);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Post");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title);
            entity.Property(e => e.Description);
            entity.Property(e => e.CreatedDate);
            entity.Property(e => e.Status);
            entity.Property(e => e.IsDeleted);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name);
            entity.Property(e => e.Price);
            entity.Property(e => e.Quantity);
            entity.Property(e => e.ExpirationDate);
            entity.Property(e => e.Venue);
            entity.Property(e => e.Status);
            entity.Property(e => e.IsDeleted);
            
            entity.HasMany(e => e.Feedbacks)
                .WithOne(e => e.Ticket)
                .HasForeignKey(e => e.TicketId);
            entity.HasMany(e => e.CartItems)
                .WithOne(e => e.Ticket)
                .HasForeignKey(e => e.TicketId);
            entity.HasMany(e => e.Posts)
                .WithOne(e => e.Ticket)
                .HasForeignKey(e => e.TicketId);
            entity.HasMany(e => e.ImageTickets)
                .WithOne(e => e.Ticket)
                .HasForeignKey(e => e.TicketId);
            entity.HasMany(e => e.Orders)
                .WithOne(e => e.Ticket)
                .HasForeignKey(e => e.TicketId);
            //entity.HasOne(e => e.TicketRequest)
            //    .WithOne(e => e.Ticket)
            //    .HasForeignKey<TicketRequest>(e => e.TicketId);
        });

        modelBuilder.Entity<TicketRequest>(entity =>
        {
            entity.ToTable("TicketRequest");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price);
            entity.Property(e => e.Quantity);
            entity.Property(e => e.TicketRequestDate);
            entity.Property(e => e.Address);
            entity.Property(e => e.Status);
            entity.Property(e => e.IdDeleted);
            
            entity.HasOne(e => e.Ticket)
                .WithOne(e => e.TicketRequest)
                .HasForeignKey<Ticket>(e => e.TicketRequestId);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transaction");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price);
            entity.Property(e => e.TransactionDate);
            entity.Property(e => e.PaymentMethod);
            entity.Property(e => e.Promotion);
            entity.Property(e => e.Status);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Fullname);
            entity.Property(e => e.PhoneNumber);
            entity.Property(e => e.Email);
            entity.Property(e => e.Address);
            entity.Property(e => e.Password);
            entity.Property(e => e.Image);
            entity.Property(e => e.Role);
            entity.Property(e => e.Gender);
            entity.Property(e => e.FcmToken);
            entity.Property(e => e.PostTime);
            entity.Property(e => e.Points);
            entity.Property(e => e.Status);
            entity.Property(e => e.IsEnabled);
            entity.Property(e => e.IsDeleted);
            
            entity.HasMany(e => e.Transactions)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
            entity.HasMany(e => e.Feedbacks)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
            entity.HasMany(e => e.Addresses)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
            entity.HasMany(e => e.Posts)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
            entity.HasMany(e => e.TicketRequests)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
            entity.HasMany(e => e.Orders)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
            entity.HasMany(e => e.Messages)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
            entity.HasMany(e => e.UserChats)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Cart)
                .WithOne(e => e.User)
                .HasForeignKey<Cart>(e => e.UserId);
            entity.HasOne(e => e.VerificationToken)
                .WithOne(e => e.User)
                .HasForeignKey<VerificationToken>(e => e.UserId);
        });

        modelBuilder.Entity<UserChat>(entity =>
        {
            entity.ToTable("UserChat");
            entity.HasKey(e => new { e.ChatId, e.UserId });
        });

        modelBuilder.Entity<VerificationToken>(entity =>
        {
            entity.ToTable("VerificationToken");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Token);
            entity.Property(e => e.ExpirationTime);

            //entity.HasOne(e => e.User)
            //    .WithOne(e => e.VerificationToken)
            //   .HasForeignKey<User>(e => e.VerificationTokenId);
        });

    }

}