using QuizPortal.Models;
using System;

public class CompletedQuiz
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int QuizId { get; set; }
    public int Score { get; set; }
    public DateTime CompletedAt { get; set; }

    public User User { get; set; }
    public Quiz Quiz { get; set; }
}
