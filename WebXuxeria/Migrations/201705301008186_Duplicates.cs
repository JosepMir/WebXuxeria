namespace WebXuxeria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Duplicates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Admin.Duplicate",
                c => new
                    {
                        DuplicateId = c.Int(nullable: false, identity: true),
                        Image1 = c.Binary(),
                        Image2 = c.Binary(),
                        Image3 = c.Binary(),
                        Image4 = c.Binary(),
                        Word1 = c.String(),
                        Word2 = c.String(),
                        Word3 = c.String(),
                        Word4 = c.String(),
                        UseWordsNotImages = c.Boolean(nullable: false),
                        OptionalPictureInCaseImageIsAWord = c.Binary(),
                        Language = c.String(),
                    })
                .PrimaryKey(t => t.DuplicateId);
            
        }
        
        public override void Down()
        {
            DropTable("Admin.Duplicate");
        }
    }
}
