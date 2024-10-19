using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizPortal.Helper;
using QuizPortal.Models;
using QuizPortal.Models.Dtos;
using QuizPortal.Proxies;
using QuizPortal.Repositories;
using QuizPortal.Repositories.Sqlite;

namespace QuizPortal.Controllers
{
    public class QuizController : Controller
    {
        private readonly IWiredProxy _wiredProxy;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public QuizController(IWiredProxy wiredProxy, IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _wiredProxy = wiredProxy;
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        [BindProperty]
        public CreateQuizViewDto CreateQuizViewDto { get; set; }

        public async Task<IActionResult> CreateQuiz()
        {
            if (HttpContext.Session.GetString(Constants.SessionUserId) == null)
            {
                return Redirect(Url.Action("Login", "User"));
            }

            // Get user ID from session
            var userId = int.Parse(HttpContext.Session.GetString(Constants.SessionUserId));
            var userRepository = _repositoryFactory.GetUserRepository();

            // Fetch user by ID
            var user = await userRepository.GetUserAsync(userId);

            // Check if the user is not an admin
            if (user == null || user.Role != "Admin")
            {
                return Redirect(Url.Action("Index", "Quiz"));  // Redirect non-admins
            }

            // Proceed with the quiz creation process
            var articleList = await _wiredProxy.GetLastFiveArticlesAsync();
            CreateQuizViewDto = new CreateQuizViewDto
            {
                ArticleList = articleList.ToList()
            };

            return View(CreateQuizViewDto);
        }


        [HttpPost]
        [ActionName("CreateQuiz")]
        public async Task<IActionResult> CreateQuizPost()
        {
            // Check if the user is logged in
            if (HttpContext.Session.GetString(Constants.SessionUserId) == null)
            {
                return Redirect(Url.Action("Login", "User"));
            }

            // Get user ID from session
            var userId = int.Parse(HttpContext.Session.GetString(Constants.SessionUserId));
            var userRepository = _repositoryFactory.GetUserRepository();

            // Fetch user by ID
            var user = await userRepository.GetUserAsync(userId);

            // Check if the user is an admin
            if (user == null || user.Role != "Admin")
            {
                // Redirect non-admins to the quiz index page
                return Redirect(Url.Action("Index", "Quiz"));
            }

            // Continue with quiz creation logic for admins
            CreateQuizViewDto.ErrorMessage = null;

            if (ModelState.IsValid)
            {
                // Distinct question control
                if (CreateQuizViewDto.QuestionArr.Select(q => q.QuestionText).Distinct().Count() != 4)
                {
                    CreateQuizViewDto.ErrorMessage = "Questions should be unique";
                    return View(CreateQuizViewDto);
                }

                // Distinct answers control
                foreach (var q in CreateQuizViewDto.QuestionArr)
                {
                    if (q.AnswerA == q.AnswerB ||
                        q.AnswerA == q.AnswerC ||
                        q.AnswerA == q.AnswerD ||
                        q.AnswerB == q.AnswerC ||
                        q.AnswerB == q.AnswerD ||
                        q.AnswerC == q.AnswerD)
                    {
                        CreateQuizViewDto.ErrorMessage = "A question cannot have the same answer more than once";
                        return View(CreateQuizViewDto);
                    }
                }

                // Create the quiz and questions
                var transaction = await _repositoryFactory.BeginTransactionAsync();
                var quizRepository = _repositoryFactory.GetQuizRepository();

                var selectedArt = CreateQuizViewDto.ArticleList.FirstOrDefault(a => a.ArticleId == CreateQuizViewDto.SelectedArticleId);
                if (selectedArt == null)
                {
                    return View(CreateQuizViewDto);
                }

                var quiz = _mapper.Map<Quiz>(selectedArt);
                await quizRepository.CreateQuizAsync(quiz);
                await _repositoryFactory.SaveAsync();

                var questionRepository = _repositoryFactory.GetQuestionRepository();
                foreach (var item in CreateQuizViewDto.QuestionArr)
                {
                    var ques = _mapper.Map<Question>(item);
                    ques.QuizId = quiz.Id;

                    await questionRepository.CreateQuestionAsync(ques);
                }

                await _repositoryFactory.SaveAsync();
                transaction.Commit();

                return RedirectToAction("Index", "Quiz");
            }

            return View(CreateQuizViewDto);
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(Constants.SessionUserId) == null)
            {
                return Redirect(Url.Action("Login", "User"));
            }
            var userRole = HttpContext.Session.GetString("UserRole");
            ViewBag.UserRole = userRole;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuizzes()
        {
            var quizRepository = _repositoryFactory.GetQuizRepository();

            var quizList = await quizRepository.GetAllQuizzesAsync();

            var quizDtoList = _mapper.Map<List<QuizDto>>(quizList.ToList());

            return Json(new { data = quizDtoList});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var quizRepository = _repositoryFactory.GetQuizRepository();

            var quizFromDb = await quizRepository.GetQuizAsync(id);

            if (quizFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            quizRepository.DeleteQuiz(quizFromDb);
            await _repositoryFactory.SaveAsync();

            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpGet]
        public async Task<IActionResult> Quiz(int id)
        {
            if (HttpContext.Session.GetString(Constants.SessionUserId) == null)
            {
                return Redirect(Url.Action("Login", "User"));
            }

            var quizRepository = _repositoryFactory.GetQuizRepository();
            var questionRepository = _repositoryFactory.GetQuestionRepository();

            var quizFromDb = await quizRepository.GetQuizAsync(id);

            // If quiz does not exist
            if(quizFromDb == null)
            {
                return Redirect(Url.Action("Index", "Quiz"));
            }

            var questionList = await questionRepository.GetAllQuestionsAsync(id);

            var quizDto = _mapper.Map<QuizDto>(quizFromDb);
            var questionDtoList = _mapper.Map<List<QuestionDto>>(questionList);

            var quizViewDto = new QuizViewDto();

            quizViewDto.QuizDto = quizDto;
            quizViewDto.QuestionDtoList = questionDtoList;

            return View(quizViewDto);
        }

        [HttpGet]
        public async Task<IActionResult> MyQuizzes()
        {
            var userId = HttpContext.Session.GetString("SessionUserId"); 
            if (userId == null)
            {
                Console.WriteLine("INSIDE ERROR");
                return Redirect(Url.Action("Login", "User"));
            }
            var quizRepository = _repositoryFactory.GetQuizRepository();

            var completedQuizzes = await quizRepository.GetCompletedQuizzesForUserAsync(int.Parse(userId));
            var completedQuizDtos = _mapper.Map<List<CompletedQuizDto>>(completedQuizzes);
            return View(completedQuizDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteQuiz(int userId, int quizId, int score)
        {
            if (userId == 0 || quizId == 0)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            var completedQuiz = new CompletedQuiz
            {
                UserId = userId,
                QuizId = quizId,
                Score = score,
                CompletedAt = DateTime.UtcNow
            };

            var quizRepository = _repositoryFactory.GetQuizRepository();
            await quizRepository.CreateCompletedQuizAsync(completedQuiz);

            return Json(new { success = true, message = "Quiz completed successfully." });
        }

    }
}


