using Microsoft.EntityFrameworkCore;

namespace apief
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;

        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
       


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                        optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Dbo");

             modelBuilder.Entity<Account>()
             .ToTable("Account", "Dbo")
                .HasKey(u => u.Id);

            modelBuilder.Entity<Note>()
             .ToTable("Notes", "Dbo")
                .HasKey(u => u.NoteId);

          

        }

    }

}