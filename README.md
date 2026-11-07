 Reminder Web App

A reminder web app that helps users turn ideas into finished tasks. Users build a to-do list, get email reminders, and see their progress through a points-and-levels system called 
This is the group project for **CSE 325: .NET Software Development** (Team 12).

## Team

| Name | Role |
|---|---|
| Sampson Havor | Group lead (Week 2) |
| Folusho Bamidele Sanni | To be assigned |
| Michael Kalani Anderson | Group lead(week 3) |
| Maisela Manhla Madise | To be assigned |

The group lead rotates each week. Roles and task owners are tracked on the Trello board.

## Project idea

Many people have good intentions but lose track of what they planned to do. This app lets a user capture an idea, turn it into a task, get reminded, and see how well they follow through.

**Target audience:** students and busy people who want a simple way to plan, get reminded, and stay motivated.

## Planned features

- **User accounts:** sign up, log in, and keep a private task list
- **To-do list (CRUD):** create, view, edit, and delete tasks, with due dates
- **Email reminders:** the app emails the user before a task is due
- **Progress tracking:** shows how many tasks are completed and how many were missed
- **"Hitting the Mark" reward system:** points and levels based on follow-through
  - Adding and completing tasks earns points, so the level goes up
  - Missed tasks lose points, so the level goes down
  - Productivity is shown as a percentage of tasks accomplished versus missed
  - Exact point values and level thresholds are still to be decided by the team

## Technology (planned)

- .NET Blazor web application
- ASP.NET Core Identity for authentication
- Entity Framework Core for the database
- Email delivery through an email service (to be chosen)
- Deployment to a cloud service

## Project management

- **Source code:** this GitHub repository
- **Task board:** Trello board (link: add the board URL here)

## Getting started

1. Clone the repository: `git clone <this repository's URL>`
2. Open the folder in Visual Studio Code.
3. The application code will be added to this repository as the project is created. Run instructions will be added here at that point.

## Working together

- Pull the latest changes before you start work: `git pull`
- Create a branch for each feature or task: `git checkout -b feature-name`
- Commit small and often, with clear messages
- Push your branch and open a pull request so a teammate can review it
- Never commit passwords, API keys, or connection strings