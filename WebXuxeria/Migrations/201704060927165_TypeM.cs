namespace WebXuxeria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TypeM : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Admin.Word", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Admin.Word", "Type", c => c.Boolean(nullable: false));
        }
    }
}
