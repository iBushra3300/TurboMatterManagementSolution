namespace TurboMatterManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iMigration1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CostCenterMatters", "CostCenter_Id", "dbo.CostCenters");
            DropForeignKey("dbo.CostCenterMatters", "Matter_Id", "dbo.Matters");
            DropIndex("dbo.CostCenterMatters", new[] { "CostCenter_Id" });
            DropIndex("dbo.CostCenterMatters", new[] { "Matter_Id" });
            DropTable("dbo.CostCenters");
            DropTable("dbo.CostCenterMatters");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CostCenterMatters",
                c => new
                    {
                        CostCenter_Id = c.Int(nullable: false),
                        Matter_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CostCenter_Id, t.Matter_Id });
            
            CreateTable(
                "dbo.CostCenters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 100),
                        CompanyName = c.String(nullable: false, maxLength: 150),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.CostCenterMatters", "Matter_Id");
            CreateIndex("dbo.CostCenterMatters", "CostCenter_Id");
            AddForeignKey("dbo.CostCenterMatters", "Matter_Id", "dbo.Matters", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CostCenterMatters", "CostCenter_Id", "dbo.CostCenters", "Id", cascadeDelete: true);
        }
    }
}
