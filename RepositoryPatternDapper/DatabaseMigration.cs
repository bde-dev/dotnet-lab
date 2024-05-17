using FluentMigrator;

namespace RepositoryPatternDapper;

[Migration(20240519, "Create User table")]
public class DatabaseMigration : Migration
{
    public override void Up()
    {
        Console.WriteLine("Running DatabaseMigration Up");
        if (!Schema.Table("users").Exists())
        {
            Console.WriteLine("users table does not exist - creating table");
            Create.Table("users")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("FirstName").AsString(50).NotNullable()
                .WithColumn("LastName").AsString(50).NotNullable();
        }
        
        Console.WriteLine("Executing stored procedure creation scripts");
        Execute.Script("StoredProcedures/spUser_GetAll.sql");
        Execute.Script("StoredProcedures/spUser_Get.sql");
        Execute.Script("StoredProcedures/spUser_Insert.sql");
        Execute.Script("StoredProcedures/spUser_Update.sql");
        Execute.Script("StoredProcedures/spUser_Delete.sql");
    }

    public override void Down()
    {
        
    }
}