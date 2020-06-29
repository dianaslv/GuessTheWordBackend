using Exam.Web.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exam.Web.Infrastructure.Data.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentToGame> StudentToGames { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CorrectResponses> CorrectResponses { get; set; }



        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetModelsRelations(modelBuilder);
        }

        private static void SetModelsRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentToGame>()
                .HasOne(t => t.Student)
                .WithMany(t => t.StudentToGames);

            modelBuilder.Entity<StudentToGame>()
                .HasOne(t => t.Game)
                .WithMany(t => t.StudentToGames);

            modelBuilder.Entity<Round>()
                .HasOne(t => t.Game)
                .WithMany(t => t.Rounds);

            modelBuilder.Entity<Response>()
                .HasOne(t => t.Round)
                .WithMany(t => t.Responses);

            modelBuilder.Entity<Response>()
                .HasOne(t => t.Student)
                .WithMany(t => t.Responses);

            modelBuilder.Entity<Category>()
                .HasOne(t => t.Round)
                .WithOne(t => t.Category)
                .HasForeignKey<Category>(b => b.RoundId);


            modelBuilder.Entity<CorrectResponses>()
                .HasOne(t => t.Category)
                .WithMany(t => t.CorrectResponses);
        }
    }
}