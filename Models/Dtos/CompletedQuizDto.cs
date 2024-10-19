using System;

namespace QuizPortal.Models.Dtos
{
    public class CompletedQuizDto
    {
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public int Score { get; set; }
        public DateTime CompletedAt { get; set; }
    }

}
