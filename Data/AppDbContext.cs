using Microsoft.EntityFrameworkCore;
using QuizPortal.Models;

namespace QuizPortal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<CompletedQuiz> CompletedQuizzes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasOne(e => e.Quiz)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // CompletedQuiz Mapping
            modelBuilder.Entity<CompletedQuiz>()
                .HasKey(cq => cq.Id); 

            modelBuilder.Entity<CompletedQuiz>()
                .HasOne(cq => cq.User) // Each CompletedQuiz has one User
                .WithMany(u => u.CompletedQuizzes) // A User can have many CompletedQuizzes
                .HasForeignKey(cq => cq.UserId) // Foreign key in CompletedQuiz
                .OnDelete(DeleteBehavior.Cascade); // Delete behavior

            modelBuilder.Entity<CompletedQuiz>()
                .HasOne(cq => cq.Quiz) // Each CompletedQuiz has one Quiz
                .WithMany(q => q.CompletedQuizzes) // A Quiz can have many CompletedQuizzes
                .HasForeignKey(cq => cq.QuizId) // Foreign key in CompletedQuiz
                .OnDelete(DeleteBehavior.Cascade); // Delete behavior

            base.OnModelCreating(modelBuilder);
        }
    }
}
