using FluentMigrator;

namespace TestcontainersDemoSource;

[Migration(20250518, "Initial Migration")]
public class PostgresMigration : Migration
{
    /// <summary>
    /// The strings in this scheme MUST match the name of the DbSet defined in <see cref="AppDbContext"/>.
    /// In this case, the table name must match the entity <see cref="User"/>,
    /// And the columns must match its properties.
    /// </summary>
    public override void Up()
    {
        if (Schema.Table("users").Exists()) return;
        
        Console.WriteLine("users table does not exist - creating table");
            
        Create.Table("users")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("FirstName").AsString(255).NotNullable()
            .WithColumn("LastName").AsString(255).NotNullable();
    }

    public override void Down()
    {
        
    }
}