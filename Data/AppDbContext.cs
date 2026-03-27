namespace TaskManager.API.Models;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Models;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext>options):base(options){}
    public DbSet<User> Users=>Set<User>();
    public DbSet<Board> Boards=>Set<Board>();
    public DbSet<BoardMember> BoardMembers=>Set<BoardMember>();
    public DbSet<TaskItem> Tasks=>Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoardMember>().HasKey(bm=>new{bm.UserId, bm.BoardId});
        modelBuilder.Entity<BoardMember>().HasOne(bm=>bm.User).WithMany(u=>u.BoardMembers).HasForeignKey(bm=>bm.UserId);
        modelBuilder.Entity<BoardMember>().HasOne(bm=>bm.Board).WithMany(b=>b.Members).HasForeignKey(bm=>bm.BoardId);
    }
}
