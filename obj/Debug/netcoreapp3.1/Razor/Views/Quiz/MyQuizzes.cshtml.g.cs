#pragma checksum "C:\Users\sahil\source\repos\dotnetcore--quiz-portal\quizportal\Views\Quiz\MyQuizzes.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "abad6f37f13c5fb90405533164300216c1bf266b84cd3da6a080e0b3cdec6adc"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Quiz_MyQuizzes), @"mvc.1.0.view", @"/Views/Quiz/MyQuizzes.cshtml")]
namespace AspNetCore
{
    #line default
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Mvc;
    using global::Microsoft.AspNetCore.Mvc.Rendering;
    using global::Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\sahil\source\repos\dotnetcore--quiz-portal\quizportal\Views\_ViewImports.cshtml"
using QuizPortal

#nullable disable
    ;
#nullable restore
#line 2 "C:\Users\sahil\source\repos\dotnetcore--quiz-portal\quizportal\Views\_ViewImports.cshtml"
using QuizPortal.Models

#line default
#line hidden
#nullable disable
    ;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"abad6f37f13c5fb90405533164300216c1bf266b84cd3da6a080e0b3cdec6adc", @"/Views/Quiz/MyQuizzes.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"13f1aeb4eff937e178385dc81f00fb80d6c03d1681418cf1b20d876af1b1eb20", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Quiz_MyQuizzes : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<QuizPortal.Models.Dtos.CompletedQuizDto>>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\sahil\source\repos\dotnetcore--quiz-portal\quizportal\Views\Quiz\MyQuizzes.cshtml"
  
    ViewData["Title"] = "My Quizzes";

#line default
#line hidden
#nullable disable

            WriteLiteral("\r\n<h2>My Quizzes</h2>\r\n\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>Quiz Title</th>\r\n            <th>Score</th>\r\n            <th>Completed At</th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
#nullable restore
#line 17 "C:\Users\sahil\source\repos\dotnetcore--quiz-portal\quizportal\Views\Quiz\MyQuizzes.cshtml"
         foreach (var quiz in Model)
        {

#line default
#line hidden
#nullable disable

            WriteLiteral("            <tr>\r\n                <td>");
            Write(
#nullable restore
#line 20 "C:\Users\sahil\source\repos\dotnetcore--quiz-portal\quizportal\Views\Quiz\MyQuizzes.cshtml"
                     quiz.QuizTitle

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                <td>");
            Write(
#nullable restore
#line 21 "C:\Users\sahil\source\repos\dotnetcore--quiz-portal\quizportal\Views\Quiz\MyQuizzes.cshtml"
                     quiz.Score

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                <td>");
            Write(
#nullable restore
#line 22 "C:\Users\sahil\source\repos\dotnetcore--quiz-portal\quizportal\Views\Quiz\MyQuizzes.cshtml"
                     quiz.CompletedAt.ToString("g")

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n            </tr>\r\n");
#nullable restore
#line 24 "C:\Users\sahil\source\repos\dotnetcore--quiz-portal\quizportal\Views\Quiz\MyQuizzes.cshtml"
        }

#line default
#line hidden
#nullable disable

            WriteLiteral("    </tbody>\r\n</table>\r\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<QuizPortal.Models.Dtos.CompletedQuizDto>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
