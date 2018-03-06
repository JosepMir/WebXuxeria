namespace WebXuxeria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pieces : DbMigration
    {
        public override void Up()
        {
            AddColumn("Admin.Word", "Part5", c => c.String());
            AddColumn("Admin.Word", "Part6", c => c.String());
            AddColumn("Admin.Word", "Part7", c => c.String());
            AddColumn("Admin.Word", "Part5Sound", c => c.Binary());
            AddColumn("Admin.Word", "Part6Sound", c => c.Binary());
            AddColumn("Admin.Word", "Part7Sound", c => c.Binary());
            AddColumn("Admin.Word", "Type", c => c.Boolean(nullable: false));

            Sql("UPDATE Admin.Word SET Type = 0");
        }
        
        public override void Down()
        {
            DropColumn("Admin.Word", "Type");
            DropColumn("Admin.Word", "Part7Sound");
            DropColumn("Admin.Word", "Part6Sound");
            DropColumn("Admin.Word", "Part5Sound");
            DropColumn("Admin.Word", "Part7");
            DropColumn("Admin.Word", "Part6");
            DropColumn("Admin.Word", "Part5");
        }
    }
}
