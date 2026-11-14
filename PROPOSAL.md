# Project Proposal: Reminder Web App

**Course:** CSE 325 .NET Software Development, Team 12
**Team:** Sampson Havor, Folusho Bamidele Sanni, Michael Kalani Anderson, Maisela Manhla Madise
**Group leader for the next meeting:** Maisela Manhla Madise

## 1. Title

**Reminder Web App**, a task planner that rewards progress through a points-and-levels system called "Hitting the Mark".

## 2. Project overview

**The problem.** People commonly want to be productive, but they lose track of what needs to be done, or they feel unmotivated by plain lists of tasks. Our app encourages productivity through a reward system. Users set tasks with completion requirements, including a due date, and the application tracks their productivity and rewards them with points that count toward a level system. Completing a task on time earns points based on the complexity of the task (how many steps it has), the time between setting the task and its due date, and a bonus for finishing early. When users do not complete tasks, they lose points.

**Who would use it.** Anyone who wants to be more productive and is able to use the app, at least ages 12 and up. Typical users are students and busy people who want a simple, motivating way to keep track of what they need to do.

**Why it is interesting and valuable.** The app helps people be more effective by using a reward system. Although it is a task list and scheduler, it is more interactive than a typical list: it turns a mundane task system into a game, which encourages people to use it regularly. Because it does not send reminders, users build the habit of logging in and checking their tasks for themselves.

## 3. Project scope

### What is IN (realistic for the remaining weeks of the course)
- Users set tasks through a text field, with steps for each task and a due date
- Users view, edit, and delete their tasks
- Points and levels: users are rewarded for completing tasks on time and lose points for missing them
- A dashboard with tasks that are due soon, a calendar, progress, and level milestones
- A new motivational quote each time a user logs in
- Login required, and each user's tasks and progress are saved and loaded

### What is OUT
- Reminders by email or notification (users check the dashboard, which builds the habit)
- Task categories
- Manual point settings
- Collaboration between users, and leader boards (possible future features)
- Day-planner features

We would rather build fewer features well than many features poorly.

## 4. App features (user actions)

Each feature is also a card on the Trello board, with a user story.

| # | Users can... | What it involves | User story |
|---|---|---|---|
| 1 | **Create an account and log in** | OAuth sign-in; login is required to use the app | As a user, I want to sign in securely so that my tasks and points are private to me |
| 2 | **Create tasks with steps and a due date** | Task text field, one or more steps, a due date within a limited window (for example, 2 weeks) | As a user, I want to break a task into steps and set a due date so that I know exactly what to do and when |
| 3 | **View their current tasks and progress** | Saved tasks load at login; a task with several steps shows a percentage (2 steps are 50% each, 3 steps are 33.33% each) | As a user, I want to see how far along each task is so that I stay motivated |
| 4 | **Edit and delete tasks** | Add or remove steps; changing the due date removes the early-completion bonus | As a user, I want to change my plans so that my list stays accurate |
| 5 | **Earn or lose points** | Points for completing on time (based on steps, days until due, and early completion); points lost for tasks not completed | As a user, I want rewards for finishing tasks so that I am encouraged to stay productive |
| 6 | **See what is due soon** | Dashboard shows tasks due within 3 days and upcoming due dates on a calendar | As a user, I want a quick view of upcoming deadlines so that I do not forget them |
| 7 | **See their level and badge** | Levels have titles and badges; the dashboard shows milestones and progress toward the next level | As a user, I want to see my level and what is next so that I have a goal to work toward |
| 8 | **See a motivational quote at each login** | A new quote on the dashboard every time the user logs in | As a user, I want a boost of encouragement when I start so that I feel ready to work |

Features that need a **database:** 2 to 5 and 7. Features that need **authentication:** all of them. Feature 6 uses an **external service** (the Calendar API).

## 5. Technical considerations

- **Data storage:** user profile, progress points, level, task list, task steps, and due dates. A relational database will hold users, tasks, steps, and a points history.
- **User accounts:** yes. Users create an account and must log in to use the app.
- **External services:** a Calendar API for the dashboard calendar (the exact service or component is still to be chosen).
- **Device compatibility:** the app will be responsive and usable on mobile phones, tablets, and desktops.
- **Basic security:** users sign in with OAuth; each user can only see and change their own tasks and points; all traffic uses HTTPS; inputs are validated on the client and the server; and no passwords, keys, or connection strings are stored in the repository. The app does not require or use personal data.

## 6. Project links

- **GitHub repository:** https://github.com/havor13/cse325-team12-project (every team member is a collaborator)
- **Trello board:** https://trello.com/b/w2CEld2p/cse325-group-12 (public, with the feature cards and tasks)

## 7. Group leader for the next meeting

**Maisela Manhla Madise**