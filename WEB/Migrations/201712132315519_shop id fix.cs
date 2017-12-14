namespace WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shopidfix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ShopID", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ShopID", c => c.String());
        }
    }
}
