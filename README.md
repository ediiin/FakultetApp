# University Management System 🎓

A comprehensive desktop application built for universities to manage academic processes, user roles, and internal communication. Designed with a strong focus on clean architecture, security, and modern UI/UX.

## 🎥 Application Demo

[![University Management System Demo](https://img.youtube.com/watch?v=a1mMWqzqgIs/maxresdefault.jpg)](https://www.youtube.com/watch?v=a1mMWqzqgIs)

## ✨ Features by Role

The system utilizes **Role-Based Access Control (RBAC)** to serve four distinct user types:

### 👨‍🎓 Students
* **Exams:** Browse and register for upcoming exam sessions.
* **Academics:** Access course materials, announcements, and track personal success metrics.
* **Administration:** Request official university certificates and view personal data.
* **Communication:** Real-time chat with fellow students, teaching assistants, and professors.

### 👨‍🏫 Professors
* **Grading:** Grade exams and assign final course grades.
* **Content Management:** Publish announcements and upload diverse course materials (PDF, Word, video, web links).
* **Scheduling:** Create and manage exam sessions.
* **Analytics:** View detailed course and student performance statistics.
* **Communication:** Real-time chat with all system users.

### 📝 Teaching Assistants
* **Content & Analytics:** View course statistics and manage course materials/announcements (similar to professors).
* **Communication:** Real-time chat support for students.

### 🛡️ Administrators
* **User & Course Management:** Full CRUD operations for courses, professors, students, and assistants.
* **Data Integrity:** Implemented **Soft Delete** for all critical data to prevent accidental data loss.
* **Administration:** Approve/reject student certificate requests and grant admin privileges to new users.

## 🛠️ Architecture & Technical Highlights

Built using modern .NET practices to ensure scalability, maintainability, and security:

* **Tech Stack:** C# / .NET / WPF
* **Architecture Design:** 
  * **MVVM (Model-View-ViewModel):** Strict separation of UI and business logic.
  * **Service-Oriented (N-Tier):** Business logic is decoupled into dedicated services (`Services` layer).
* **Dependency Injection (DI):** Implemented for service lifetimes and highly testable code.
* **Security & Auth:**
  * Role-Based Authorization.
  * **Login Rate Limiting:** Built-in brute-force protection (freezes the login interface for a cooldown period after 5 failed attempts).
* **UI/UX:** Responsive layouts with full **Dark Mode** support.

## 🚀 Getting Started (Local Development)

1. Clone the repository:
   ``bash
   git clone [https://github.com/YourUsername/YourRepoName.git](https://github.com/YourUsername/YourRepoName.git)
   Open the solution (.sln) in Visual Studio.

2. Configure your database connection string in the configuration file.

3. Run Entity Framework migrations to generate the local dat

abase:
  ``bash
  Update-Database

4. Run the application (F5).

## (Note: Switching to the production server database is planned for future versions.)
