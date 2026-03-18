using CapoBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace CapoBooking.Data;

public class AppDbContext : DbContext
{
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}
