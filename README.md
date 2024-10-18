Legal System
## Project Overview

The **Legal System** project is an ASP.NET MVC application designed for managing legal cases efficiently. The application supports CRUD (Create, Read, Update, Delete) operations for various entities, including cases, lawyers, clients, admins, sessions, and courts. It aims to simplify the management of legal information and enhance workflow for legal professionals.

## Features

- **User Roles**: 
  - Admin, Lawyer, and Client roles with different access permissions.
- **CRUD Operations**: 
  - Manage cases, lawyers, clients, sessions, and courts through a user-friendly interface.
- **Session Management**: 
  - Track and manage court sessions associated with cases.

## Technologies Used

- **Framework**: 
  - ASP.NET MVC
- **Database**: 
  - SQL Server (or any other supported database)
- **Frontend**: 
  - HTML, CSS, and Bootstrap for styling.
- **Tools**: 
  - Visual Studio (for development)
  - Entity Framework (for database operations)



## Installation

### Prerequisites

Make sure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server Express

### Clone the Repository

```bash
git clone https://github.com/MaryamMohamedAuf/Legal-System.git
cd Legal-System
```

### Configure the Database

 Open the `appsettings.json` file and configure your connection string:
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_username;Password=your_password;"
    }
    ```
### Run the Application

1. Open the project in Visual Studio.
2. Set the project as the startup project.
3. Press `F5` to run the application.

## Usage

Once the application is running, you can access it by navigating to `http://localhost:5000` (or the configured port) in your web browser.

### User Registration and Login

- **Admin Login**: Access the admin dashboard to manage users and cases.
- **Lawyer and Client Registration**: Use the registration page to create new user accounts.

### Managing Entities

- **Cases**: Create, update, view, and delete cases from the case management section.
- **Lawyers**: Manage lawyer profiles and their associated cases.
- **Clients**: Add and manage client information and their respective cases.
- **Sessions**: Schedule and manage court sessions.

## Contact

For questions, suggestions, or issues, please contact:

maryammohamedauf@gmail.com
https://github.com/MaryamMohamedAuf
https://www.linkedin.com/in/maryam-mohamed-auf/

