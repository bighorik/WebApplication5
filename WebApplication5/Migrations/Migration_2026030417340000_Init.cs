using FluentMigrator;

namespace WebApplication5.Migrations
{
    [Migration(2026030417340000)]
    public class Migration_2026030417340000_Init : Migration
    {
        public override void Up()
        {
            Create.Table("study")
               .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
               .WithColumn("name").AsString().NotNullable()
               .WithColumn("code").AsString().NotNullable()
               .WithColumn("phase").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("study");
        }
    }
}
