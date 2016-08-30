namespace Vidly2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBackToGenre : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.NewGenres", newName: "Genres");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Genres", newName: "NewGenres");

        }
    }
}
