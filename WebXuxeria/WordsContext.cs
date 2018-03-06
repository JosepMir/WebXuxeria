namespace WebXuxeria
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public class WordsContext : DbContext
    {

        public WordsContext()
        {
 
            Database.Connection.ConnectionString = @"Data Source=JOSEP-PC\SQLEXPRESS;Initial Catalog=Xuxeria;Integrated Security=True;Pooling=False;";
 
            Database.SetInitializer(new CreateDatabaseIfNotExists<WordsContext>());
        }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.


        public virtual DbSet<Word> Words { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }

        public virtual DbSet<Duplicate> Duplicates { get; set; }

        public virtual DbSet<Picture> Picture { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<WordsContext>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Configure default schema
            modelBuilder.HasDefaultSchema("Admin");

            //add a UNIQUE index for column Email
            //    modelBuilder
            //.Entity<User>()
            //.Property(t => t.Email)
            //.IsRequired() 
            //.HasColumnAnnotation(
            //    IndexAnnotation.AnnotationName,
            //    new IndexAnnotation(
            //        new IndexAttribute("IX_Email", 1) { IsUnique = true }));

            //Map entity to table
            //modelBuilder.Entity<User>().ToTable("StudentInfo");
            //modelBuilder.Entity<Word>().ToTable("StandardInfo", "dbo");

        }
    }



    public class Collection
    {
        public Collection()
        {

        }
        public int CollectionId { get; set; }
        public string Name { get; set; }

        //todo
        public string Language { get; set; }
        //todo
        public int Order { get; set; }

        public virtual Picture Image { get; set; }

        //TODO
        public bool IsPublic { get; set; }

        //one column cannot have 2 foreign keys
        //public ICollection<Word> Words { get; set; }
        //public ICollection<Duplicate> Duplicates { get; set; }

        public collectionType Type { get; set; }
    }

    public enum collectionType { words = 0, duplicates = 1};

    public class Word
    {
        public Word()
        {

        }
        public int WordId { get; set; }
        public string Part1 { get; set; }
        public string Part2 { get; set; }
        public string Part3 { get; set; }
        public string Part4 { get; set; }
        public string Part5 { get; set; }
        public string Part6 { get; set; }
        public string Part7 { get; set; }

        public Guid ImageId { get; set; }
        public virtual Picture Image { get; set; }

        public byte[] Part1Sound { get; set; }
        public byte[] Part2Sound { get; set; }
        public byte[] Part3Sound { get; set; }
        public byte[] Part4Sound { get; set; }

        public byte[] Part5Sound { get; set; }
        public byte[] Part6Sound { get; set; }
        public byte[] Part7Sound { get; set; }

        //TODO
        public int Order { get; set; }

        //public int  CollectionId { get; set; } //has to be here in order to do things like word.Collection
        public int? Collection_CollectionId { get; set; }

        [ForeignKey("Collection_CollectionId")]
        public virtual Collection Collection { get; set; }

        public bool IsEmptyWord
        {
            get
            {
                return string.IsNullOrWhiteSpace(Part1) && string.IsNullOrWhiteSpace(Part2) && string.IsNullOrWhiteSpace(Part3) && string.IsNullOrWhiteSpace(Part4);
            }
        }

        public int PartCount
        {
            get
            {
                if (Part1 == "")
                    return 0;
                if (Part2 == "")
                    return 1;
                if (Part3 == "")
                    return 2;
                if (Part4 == "")
                    return 3;
                return 4;
            }
        }

        public enum TypeEnum { Word = 0, Piece = 1 };
        public TypeEnum Type { get; set; }
    }


    public class Duplicate
    {
        public Duplicate()
        {

        }
        public int DuplicateId { get; set; }
        //public string Name { get; set; }


        public virtual Picture ImageQuestion { get; set; }
        public virtual Picture Image1 { get; set; }
        public virtual Picture Image2 { get; set; }
        public virtual Picture Image3 { get; set; }
        public virtual Picture Image4 { get; set; }

        public string Question { get; set; }
        public string Answer1 { get; set; } //A  //SOL
        public string Answer2 { get; set; } //E  //SLO
        public string Answer3 { get; set; } //U  //LOS
        public string Answer4 { get; set; } //I  //LSO

        public bool Answer1IsCorrect { get; set; }
        public bool Answer2IsCorrect { get; set; }
        public bool Answer3IsCorrect { get; set; }
        public bool Answer4IsCorrect { get; set; }

        public int CorrectAnswerCount
        {
            get
            {
                var count = 0;
                if (Answer1IsCorrect) count++;
                if (Answer2IsCorrect) count++;
                if (Answer3IsCorrect) count++;
                if (Answer4IsCorrect) count++;
                return count;
            }
        }

        public byte[] OptionalPictureInCaseImageIsAWord { get; set; }

        public int Order { get; set; }
        //todo
        public string Language { get; set; }

        //public int CollectionId { get; set; } //has to be here in order to do things like word.Collection
        public int Collection_CollectionId { get; set; }

        [ForeignKey("Collection_CollectionId")]
        public virtual Collection Collection { get; set; }
    }

    public class Picture
    {
        public Picture()
        {
            PictureId = Guid.NewGuid();
        }

        [Key]
        public Guid PictureId { get; set; }

        public byte[] Content { get; set; }
    }
}


