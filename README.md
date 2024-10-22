# Quiz Portal

## Purpose

The **Quiz Portal** is designed to allow normal users to take quizzes and administrators to create, manage, and delete quizzes. Users can view their quiz history and track whether they have completed specific quizzes. Admins can view all quizzes and manage them accordingly.

The key features of the application include:
- **Quiz Creation**: Admin users can create new quizzes.
- **Quiz Participation**: Normal users can take quizzes.
- **Quiz Management**: Admins can delete quizzes if needed.
- **My Quizzes**: Users can see their completed quizzes and scores.

## Features

- **Admin and User Roles**: Admins have special permissions to create and delete quizzes, while users can participate in quizzes and view their results.
- **Authentication**: Users need to register and login to access the quiz functionalities.
- **Quiz Status**: Users can view whether they have completed a quiz.
- **Entity Framework**: Used for database operations and data management.
- **Session Management**: The role of the user is stored in the session to control access to specific features like quiz creation and deletion.

## Prerequisites

Before you begin, ensure you have met the following requirements:
- .NET Core SDK (>= 6.0)
- Visual Studio or any other preferred IDE
- SQL Server for database (or any other configured database in your project)
- Node.js (for frontend dependencies, if required)

## Getting Started

### 1. Clone the repository
Clone the repository to your local machine:
```bash
git clone https://github.com/sahil16-12/Quiz-Application.git
```
### 2. Set up the Database
Run the following commands to set up the database:

- Add a new migration :
```bash
Add-Migration InitialMigration
```
- Apply the migrations to update the database schema:
```bash
Update-Database
```
### 3. Build and Run the Application
In the root folder of the project, execute the following commands:
```bash
dotnet build
dotnet run
```
The application will now be available at https://localhost:5001/ or http://localhost:5000/.
### 4. Create Users and Admins
- To register as an Admin, you must pass the role as "Admin" during registration.
- Normal users can register with the "User" role to only take quizzes.
### 5. Session and Authorization
- The user's role is stored in the session (UserRole), which controls access to specific views and functionalities such as quiz creation and deletion.
- Logging out will destroy the session and remove the user's role from memory.
### Roles and Permissions
- **Admin Role**: Can create, take and delete quizzes.
- **User Role**: Can take quizzes and view quiz history.
### Deleting a Quiz
- Admins can delete quizzes via a "Delete" button, which is disabled for normal users. The app verifies user roles before showing the delete option.


