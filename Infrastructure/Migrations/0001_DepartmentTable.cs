
using FluentMigrator;

[Migration(1)]
public class DepartmentTable: Migration
{
    public override void Up()
    {
        Execute.Sql(@"
CREATE TABLE Department (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL UNIQUE,
    Phone VARCHAR(50)
);
");
    }

    public override void Down()
    {
        Execute.Sql(@"DROP TABLE if exists Department");
    }
}