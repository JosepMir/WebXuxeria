namespace WebXuxeria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Admin.Collection",
                c => new
                    {
                        CollectionId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Language = c.String(),
                        Order = c.Int(nullable: false),
                        Image = c.Binary(),
                        IsPublic = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CollectionId);
            
            CreateTable(
                "Admin.Word",
                c => new
                    {
                        WordId = c.Int(nullable: false, identity: true),
                        Part1 = c.String(),
                        Part2 = c.String(),
                        Part3 = c.String(),
                        Part4 = c.String(),
                        Image = c.Binary(),
                        Part1Sound = c.Binary(),
                        Part2Sound = c.Binary(),
                        Part3Sound = c.Binary(),
                        Part4Sound = c.Binary(),
                        Order = c.Int(nullable: false),
                        Collection_CollectionId = c.Int(),
                    })
                .PrimaryKey(t => t.WordId)
                .ForeignKey("Admin.Collection", t => t.Collection_CollectionId)
                .Index(t => t.Collection_CollectionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Admin.Word", "Collection_CollectionId", "Admin.Collection");
            DropIndex("Admin.Word", new[] { "Collection_CollectionId" });
            DropTable("Admin.Word");
            DropTable("Admin.Collection");
        }
    }
}
