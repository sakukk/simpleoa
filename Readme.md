### Prerequisites

Before you begin, ensure you have met the following requirements:

- You have installed Node.js 18+.
- You have installed .NET 9.0.
- You have a SQL Server instance running.
- Docker is installed

### Setting up MSSQL with Docker

To set up MSSQL with Docker using `docker-compose.dev.yml`, follow these steps:

1. Run the following command to start the MSSQL container:

    ```sh
    docker-compose -f docker-compose.dev.yml up -d
    ```

2. Verify that the SQL Server container is running:

    ```sh
    docker ps
    ```

You should see a container named `sqlserver` in the list.

### Running EF Migrations for OaService

To run Entity Framework (EF) migrations for the OaService project, follow these steps:

1. Open a terminal and navigate to the OaService project directory:

    ```sh
    cd /e:/projects/OAApplication/OaService
    ```

2. Add a new migration by running the following command:

    ```sh
    dotnet ef migrations add <MigrationName>
    ```

    Replace `<MigrationName>` with a descriptive name for your migration.

3. Apply the migration to the database:

    ```sh
    dotnet ef database update
    ```

4. Verify that the migration has been applied successfully by checking the database.

These steps will ensure that your database schema is up to date with the latest changes in your EF model.


### Running the OaService Application
To run the OaService application, follow these steps:
1. Open a terminal or command prompt in the project directory.
2. Run the following command to start the application:

    ```sh
    dotnet run
    ```

3. The application should now be running and accessible at `http://localhost:5237`.

### Running the OaWeb Application
To run the OaWeb application, follow these steps:
1. Open a terminal or command prompt in the project directory.
2. Run the following command to start the application:

    ```sh
    npm dev
    ```
3. The application should now be running and accessible at `http://localhost:3000`.
