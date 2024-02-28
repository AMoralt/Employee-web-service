
using FluentMigrator;

[Migration(2)]
public class EmployeeTable: Migration
{
    public override void Up()
    {
        Execute.Sql(@"
CREATE TABLE Employee (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Surname VARCHAR(255) NOT NULL,
    Phone VARCHAR(50),
    CompanyId INT,
    Type VARCHAR(50),
    Number VARCHAR(50) NOT NULL UNIQUE,
    DepartmentId INT,
    FOREIGN KEY (DepartmentId) REFERENCES Department(Id) ON DELETE SET NULL
);
");
    }

    public override void Down()
    {
        Execute.Sql(@"DROP TABLE if exists Employee");
    }
}