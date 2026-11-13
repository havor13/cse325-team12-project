# Reminder Web App

A productivity web app that turns a task list into a game. Users create tasks with steps and due dates, and earn points and levels for finishing them on time through a reward system called **"Hitting the Mark"**. Missed tasks lose points.

This is the group project for **CSE 325: .NET Software Development** (Team 12).

## Team

| Name | Role |
|---|---|
| Sampson Havor | Group lead (Week 2) |
| Folusho Bamidele Sanni | To be assigned |
| Michael Kalani Anderson | Group lead (Week 3) |
| Maisela Manhla Madise | Group lead (Week 4, the next meeting) |

The group lead rotates each week. Roles and task owners are tracked on the Trello board.

## The problem

People commonly want to be productive, but they lose track of what needs to be done, or feel unmotivated by plain lists of tasks. This app encourages productivity through a reward system. Users set tasks with completion requirements, including a due date, and the app tracks their productivity and rewards them with points that count toward a level system.

Completing a task on time earns points based on the complexity of the task (the number of steps), the time between setting the task and its due date, and a bonus for finishing early. Tasks that are not completed cost the user points.

**Target users:** anyone who wants to be more productive and can use the app, ages 12 and up.

**What makes it valuable:** it is more than a scheduler. It makes a mundane task list interactive, so users are encouraged to keep using it.

## Features

The five main features:

1. **Tasks:** create, view, edit, and delete tasks, with steps and a due date
2. **Rewards and penalties:** earn points for completing tasks, lose points for missing them
3. **Load and save tasks:** each user's data is stored and loaded when they log in
4. **Due-date reminders:** the dashboard shows tasks that are due within 3 days
5. **Motivational quotes:** a new quote is shown at each login

More detail:

- Tasks are entered through a text field, with steps for each task
- Each task has a due date, within a limited time window (for example, 2 weeks)
- Editing a task, such as adding or removing steps, is allowed. Changing the due date removes the early-completion bonus
- A task with several steps shows a progress percentage (for example, 2 steps are 50% each, and 3 steps are 33.33% each)
- The dashboard shows upcoming due dates on a calendar
- The dashboard shows level milestones and progress toward the next level, and each level has a title and a badge
- Users must log in, and the points and levels are saved per user
- Exact point values and level thresholds are still to be decided by the team

## What the app will not do

- **No email reminders.** Users must log in and check their dashboard, which encourages the habit of checking tasks every day
- No categories
- No manual point settings
- No collaboration between users, and no leader boards (possible future features)
- No day-planner features

## Technical considerations

- **Platform:** .NET Blazor web application
- **Data stored:** user profile, progress points, level, task list, task steps, and due dates
- **Login:** users create an account and log in, using OAuth. No personal data is required or used
- **External service:** a Calendar API for the dashboard calendar
- **Devices:** works on phones, tablets, and desktops
- **Deployment:** to a cloud service

## Project links

- **Source code:** https://github.com/havor13/cse325-team12-project
- **Task board (Trello):** https://trello.com/b/w2CEld2p/cse325-group-12

## Getting started

1. Clone the repository: `git clone https://github.com/havor13/cse325-team12-project.git`
2. Open the folder in Visual Studio Code.
3. The application code is added to this repository as the project is built. Run instructions will be added here at that point.

## Working together

- Pull the latest changes before you start work: `git pull`
- Create a branch for each feature or task: `git checkout -b feature-name`
- Commit small and often, with clear messages
- Push your branch and open a pull request so a teammate can review it
- Never commit passwords, API keys, or connection strings