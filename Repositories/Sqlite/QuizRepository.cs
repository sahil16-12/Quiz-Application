using Microsoft.EntityFrameworkCore;
using QuizPortal.Data;
using QuizPortal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizPortal.Repositories.Sqlite
{
    public class QuizRepository : IQuizRepository
    {
        private readonly AppDbContext _db;

        public QuizRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task CreateQuizAsync(Quiz quiz)
        {
            await _db.Quizzes.AddAsync(quiz);
        }

        public async Task<Quiz> GetQuizAsync(int id)
        {
            return await _db.Quizzes.FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<ICollection<Quiz>> GetAllQuizzesAsync()
        {
            return await _db.Quizzes.ToListAsync();
        }

        public void DeleteQuiz(Quiz quiz)
        {
            _db.Quizzes.Remove(quiz);
        }

        public async Task<List<CompletedQuiz>> GetCompletedQuizzesForUserAsync(int userId)
        {
            return await _db.CompletedQuizzes
                .Where(cq => cq.UserId == userId)
                .Include(cq => cq.Quiz) // Include the related Quiz entity if you need quiz details
                .ToListAsync();
        }

        public async Task CreateCompletedQuizAsync(CompletedQuiz completedQuiz)
        {
            await _db.CompletedQuizzes.AddAsync(completedQuiz);
            await _db.SaveChangesAsync(); 
        }

    }
}
