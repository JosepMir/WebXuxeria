namespace WebXuxeria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DuplicatesOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("Admin.Duplicate", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Admin.Duplicate", "Order");
        }
    }
}
