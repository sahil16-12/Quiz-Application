$(document).ready(function () {
    $('#articleDropDown').change(function () {

        $('#articleDesc').empty();

        var selectedArticleId = $(this).val();

        if (selectedArticleId == "")
            return;

        var desc = $('#' + selectedArticleId).text();

        console.log(desc);

        $('#articleDesc').append("<p class='border bg-success text-white' style='padding:30px; border-radius:20px;'>" + desc + "</p>");
    });
    console.log("JavaScript is running!"); // Test logging

    loadDataTable();
});

var dataTable;
var complete = false;

//function loadDataTable() {
//    dataTable = $('#quizListTable').DataTable({
//        "ajax": {
//            "url": "/quiz/getallquizzes",
//            "type": "GET",
//            "datatype": "json"
//        },
//        "columns": [
//            { "data": null},
//            { "data": "title",
//                "render": function (data, type, row, meta) {
//                    return `<a href="/quiz/quiz?id=${row.id}" style="cursor:pointer; width: 120px;">
//                                ${data}
//                            </a>`
//                },
//                "width": "40%" },
//            { "data": "articleId", "width": "20%" },
//            { "data": "createdStr", "width": "20%" },
//            {
//                "data": "id",
//                "render": function (data) {
//                    return `<div class="text-center">
//                        <a class="btn btn-danger text-white" style="cursor:pointer; width: 120px;"
//                            onclick=Delete('quiz/Delete?id=${data}')>
//                            Delete
//                        </a>
//                    </div>`;
//                },
//                "width": "20%"
//            }
//        ],
//        "language": {
//            "emptyTable": "no data found"
//        },
//        "width": "100%",
//        "columnDefs": [
//            {
//                "searchable": false,
//                "orderable": false,
//                "targets": 0
//            }
//        ],
//        "order": [
//            [1, 'asc']
//        ]
//    });

//    dataTable.on('order.dt search.dt', function () {
//        dataTable.column(0, {
//            search: 'applied',
//            order: 'applied'
//        }).nodes().each(function (cell, i) {
//            cell.innerHTML = i + 1;
//        });
//    }).draw();
//}

function loadDataTable() {
    dataTable = $('#quizListTable').DataTable({
        "ajax": {
            "url": "/quiz/getallquizzes",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": null },
            {
                "data": "title",
                "render": function (data, type, row, meta) {
                    return `<a href="/quiz/quiz?id=${row.id}" style="cursor:pointer; width: 120px;">
                                ${data}
                            </a>`
                },
                "width": "40%"
            },
            { "data": "articleId", "width": "20%" },
            { "data": "createdStr", "width": "20%" },
            {
                "data": "id",
                "render": function (data, type, row, meta) {
                    if (userRole === 'Admin') {  // Check if the user is an Admin
                        return `<div class="text-center">
                                    <a class="btn btn-danger text-white" style="cursor:pointer; width: 120px;"
                                        onclick=Delete('quiz/Delete?id=${data}')>
                                        Delete
                                    </a>
                                </div>`;
                    } else {
                        return `<div class="text-center">
                                    <span class="text-muted">No Action</span>
                                </div>`;
                    }
                },
                "width": "20%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%",
        "columnDefs": [
            {
                "searchable": false,
                "orderable": false,
                "targets": 0
            }
        ],
        "order": [
            [1, 'asc']
        ]
    });

    dataTable.on('order.dt search.dt', function () {
        dataTable.column(0, {
            search: 'applied',
            order: 'applied'
        }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
}

function Delete(deleteUrl) {
    swal({
        title: "Are you sure to delete the quiz?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: deleteUrl,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function answerOnClick(questionIndex, answer) {
    if (complete == true)
        return;

    var elemList = document.getElementsByClassName("custom+" + questionIndex);

    for (var i=0; i<elemList.length; i++) {
        elemList[i].classList.remove("btn-warning");
    }

    document.getElementById("question+" + questionIndex + '+' + answer).classList.add("btn-warning");
}
function CompleteQuiz() {
    var elemList = document.getElementsByClassName("btn-warning");

    if (elemList.length != 4) {
        toastr.error("Please complete the quiz");
        return;
    }

    var correctCount = 0;
    var urlParams = new URLSearchParams(window.location.search);
    var quizId = urlParams.get('id');
    var score = 0;

    for (var i = 3; i >= 0; i--) {
        var correctAnswer = document.getElementById('correct+' + i).value;
        var givenAnswer = elemList[i].getAttribute("for");

        if (givenAnswer == correctAnswer) {
            correctCount++;
            elemList[i].classList.add("btn-success");
        } else {
            elemList[i].classList.add("btn-danger");
        }

        elemList[i].classList.remove("btn-warning");
    }

    score = correctCount;
    console.log(userId + " " + quizId + " " + score);
    complete = true;
    document.getElementById("completeBtn").style.visibility = "hidden";

    // Create FormData object and append data
    var formData = new FormData();
    formData.append("userId", userId); // Make sure userId is available globally
    formData.append("quizId", quizId);
    formData.append("score", score);

    $.ajax({
        url: '/Quiz/CompleteQuiz',
        type: 'POST',
        processData: false,  // Don't process the data
        contentType: false,  // Don't set content type header
        data: formData,      // Use formData object for the data
        success: function (response) {
            if (response.success) {
                toastr.success("Quiz completed and saved successfully!");
                ShowResultCard(correctCount);
            } else {
                toastr.error("Failed to save the quiz: " + response.message);
            }
        },
        error: function (xhr, status, error) {
            toastr.error("An error occurred while saving the quiz.");
            console.error(error);
        }
    });
}



function ShowResultCard(correctCount) {

    var resultCard = `
        <div class="card text-center mt-4">
            <div class="card-header bg-info text-white">
                Quiz Completed!
            </div>S
            <div class="card-body">
                <h5 class="card-title">You got ${correctCount} out of 4 correct!</h5>
                <p class="card-text">Thank you for participating in the quiz.</p>
                <a href="/quiz/quiz" class="btn btn-secondary">Go to Quizzes</a>
            </div>S
            <div class="card-footer text-muted">
                Good job!
            </div>
        </div>`;

    // Append the result card to a container (you can place this container in your HTML where you want the result to show)
    $('#quizResult').html(resultCard);
}

