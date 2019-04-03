# BangazonAPI
Group project exploring C# controllers, integrated testing, and models while building a traditional API.


# Welcome to Bangazon Workforce Management Site :briefcase:

----
## What is the Bangazon Workforce Management Site? :computer:
It is ASP.NET Web Application using Visual Studio on Windows, SQL Server as the database engine, and Razor templating syntax. It allows human resources and IT departments of Bangazon to create, list, and view Employees and Departments.

----
## Software Dependencies :space_invader:

[SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-2017)
[Visual Studio](https://visualstudio.microsoft.com/)

----
## Installation :floppy_disk:
1. Fork Repository
2. Clone repository
3. Open SSMS and make a database using the bangazon.sql folder
4. Open visual studio, click view and open solution explorer
5. Create appsettings.json
6. Add connection string to appsettings.json
- Click view
- Select sequel server object explorer
- Click the server that begins with Desktop
- Click the databases folder and right click the database you created
- At the bottom of the pop-up, click the properties link
- Select and copy the connection string. It should be the ninth option down
- Put the connection string in the appsettings.json as the value for the Default Connection key
- Replace DataSource= with Server= 
7. Run the application from the Solution Explorer, NOT the Solution Explorer - Folder View

----
## Testing :ballot_box_with_check:
1. Click Test tab in Visual Studio
2. Click Windows dropdown and select Test Explorer
3. Select specific Tests to run using the Run dropdown menu
