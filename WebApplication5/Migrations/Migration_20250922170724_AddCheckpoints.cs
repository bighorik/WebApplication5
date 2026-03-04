using FluentMigrator;

namespace Delivery.Infrastructure.Migrations;

[Migration(20250922170724)]
public class Migration_20250922170724_AddCheckpoints : Migration
{
    public override void Up() => Create.Table("checkpoints")
            .WithColumn("id").AsString().PrimaryKey()
            .WithColumn("position").AsInt64().Nullable();

    public override void Down() => Delete.Table("checkpoints");
}
