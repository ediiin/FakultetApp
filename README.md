📝 FacultyApp - University Portal & Management System
FacultyApp is a modern desktop application built using WPF (Windows Presentation Foundation) and .NET 10.0. Designed to replicate a real-world university ecosystem, the application serves a dual purpose: a robust administration dashboard for faculty staff and a comprehensive information hub for students.

The project strictly adheres to clean code principles, separation of concerns, modern UI/UX workflows, and scalable architecture.

🚀 Key Features
1. Admin Management Dashboard (Current)
Smart Data Segregation: Differentiates between immutable student data (Index number, National ID/JMBG, Gender, Date of Birth) and fluid academic attributes (Email, Student Status, Current Academic Year/Major, Graduation status).

Secure Password Management: Implements a non-destructive password updates workflow. Existing password hashes remain securely untouched unless explicitly overridden by an administrator, preventing accidental data loss.

Granular Input Validation: Real-time visual feedback using dedicated error containers prevents the submission of corrupted or incomplete form states to the database.

2. Comprehensive Student Portal (Upcoming Roadmap)
The application is expanding into a full student-centric platform, transforming it into the ultimate campus companion from the student's perspective:

Professor Announcements Feed: A centralized notice board where students can view real-time updates, exam schedules, and important posts published by faculty members.

Document Repository: Seamless access to academic resources, lecture slides, syllabi, and official documents uploaded by professors.

Direct Messaging System: Integrated internal chat communication allowing students to securely message their professors regarding coursework, consultations, and academic inquiries.

🛠️ Tech Stack
Framework: .NET 10.0 (WPF)

Language: C# / XAML

Dependency Injection: Microsoft.Extensions.DependencyInjection

Security: BCrypt.Net (Industry-standard salted password hashing)

Layout Engine: Responsive fluid grids paired with asynchronous ScrollViewer containers for flexible display scaling.

Database / ORM: [e.g., Entity Framework Core / SQL Server / SQLite - update this line]

📐 Architecture & Core Concepts
Dependency Injection (DI): Fully decoupled architecture utilizing .NET 10's native DI container to manage the lifecycle of services and views (AddTransient / AddSingleton).

Hybrid View Initialization: Dynamic data-bound views (such as EditStudentView) utilize a decoupling pattern where dependencies are resolved via DI through the constructor, while runtime contextual entity data is passed seamlessly via custom Inicijalizuj() channels.
💻 Getting Started
Prerequisites
Visual Studio 2025 / 2026 (with ".NET Desktop Development" workload installed)

.NET 10.0 SDK or newer

Installation & Run
Clone the repository to your local machine:

Bash
git clone https://github.com/your-username/FacultyApp.git
Open the solution file (FakultetApp.sln) in Visual Studio.

Restore the required NuGet packages:

Right-click the Solution -> Restore NuGet Packages.

Apply database migrations [e.g., Run Update-Database in the Package Manager Console if using EF Core].

Press F5 or click Start to launch the application.

🔒 Security Standards
No Plain-Text Passwords: Passwords undergo heavy cryptographic computation using the BCrypt algorithm before committing to the storage layer.

UI Defense: Form fields are structurally isolated from structural entity operations until explicit validation checks evaluate to true.

👨‍💻 Author
Edin Alibegović - Software Engeneering Student, Faculty of Information Technology (FIT) Mostar

GitHub: @ediiin

Visual Tree Loosely-Coupled Navigation: Navigating from child views back to parent layouts is achieved dynamically using the VisualTreeHelper class, avoiding anti-patterns like circular dependencies or hardcoded parent references.
