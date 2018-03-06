namespace WebXuxeria.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebXuxeria.WordsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "WebXuxeria.WordsContext";
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(WebXuxeria.WordsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //context.Words.AddOrUpdate(
            //  new Word { Type = Word.TypeEnum.Piece, Part1 = "pa", Part2 = "pe", Part3 = "pi", Part4 = "po", Part5 = "pu" },
            //  new Word { Type = Word.TypeEnum.Piece, Part1 = "ma", Part2 = "me", Part3 = "mi", Part4 = "mo", Part5 = "mu" },
            //  new Word { Type = Word.TypeEnum.Piece, Part1 = "ca", Part2 = "ce", Part3 = "ci", Part4 = "co", Part5 = "cu" },
            //  new Word { Type = Word.TypeEnum.Piece, Part1 = "ba", Part2 = "be", Part3 = "bi", Part4 = "bo", Part5 = "bu" },
            //  new Word { Type = Word.TypeEnum.Piece, Part1 = "ta", Part2 = "te", Part3 = "ti", Part4 = "to", Part5 = "tu" },
            //  new Word { Type = Word.TypeEnum.Piece, Part1 = "la", Part2 = "le", Part3 = "li", Part4 = "lo", Part5 = "lu" },
            //  new Word { Type = Word.TypeEnum.Piece, Part1 = "xa", Part2 = "xe", Part3 = "xi", Part4 = "xo", Part5 = "xu" },
            //  new Word { Type = Word.TypeEnum.Piece },
            //  new Word { Type = Word.TypeEnum.Piece },
            //  new Word { Type = Word.TypeEnum.Piece }
            //);

        }
    }
}
