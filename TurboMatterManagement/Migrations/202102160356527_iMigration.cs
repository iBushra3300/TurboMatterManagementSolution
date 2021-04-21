namespace TurboMatterManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Street = c.String(nullable: false, maxLength: 200),
                        City = c.String(nullable: false, maxLength: 100),
                        Zip = c.String(nullable: false, maxLength: 10),
                        Type = c.Int(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        State_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.States", t => t.State_Id, cascadeDelete: true)
                .Index(t => t.OrganizationId)
                .Index(t => t.State_Id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        BillingRegion = c.Int(nullable: false),
                        VendorNumber = c.String(nullable: false, maxLength: 20),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Matters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false),
                        MatterStatus = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        MatterTypeId = c.Int(nullable: false),
                        OpenDate = c.DateTime(nullable: false),
                        DispositionDate = c.DateTime(),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("dbo.MatterTypes", t => t.MatterTypeId, cascadeDelete: true)
                .Index(t => t.CountryId)
                .Index(t => t.MatterTypeId);
            
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
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(nullable: false, maxLength: 2),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false, maxLength: 200),
                        ContentType = c.String(nullable: false, maxLength: 100),
                        Content = c.Binary(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        Matter_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Matters", t => t.Matter_Id, cascadeDelete: true)
                .Index(t => t.Matter_Id);
            
            CreateTable(
                "dbo.MatterTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Phones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false, maxLength: 50),
                        Type = c.Int(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 15),
                        Password = c.String(nullable: false, maxLength: 10),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 50),
                        Locked = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CostCenterMatters",
                c => new
                    {
                        CostCenter_Id = c.Int(nullable: false),
                        Matter_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CostCenter_Id, t.Matter_Id })
                .ForeignKey("dbo.CostCenters", t => t.CostCenter_Id, cascadeDelete: true)
                .ForeignKey("dbo.Matters", t => t.Matter_Id, cascadeDelete: true)
                .Index(t => t.CostCenter_Id)
                .Index(t => t.Matter_Id);
            
            CreateTable(
                "dbo.MatterOrganizations",
                c => new
                    {
                        Matter_Id = c.Int(nullable: false),
                        Organization_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Matter_Id, t.Organization_Id })
                .ForeignKey("dbo.Matters", t => t.Matter_Id, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id, cascadeDelete: true)
                .Index(t => t.Matter_Id)
                .Index(t => t.Organization_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Addresses", "State_Id", "dbo.States");
            DropForeignKey("dbo.Phones", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.MatterOrganizations", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.MatterOrganizations", "Matter_Id", "dbo.Matters");
            DropForeignKey("dbo.Matters", "MatterTypeId", "dbo.MatterTypes");
            DropForeignKey("dbo.Documents", "Matter_Id", "dbo.Matters");
            DropForeignKey("dbo.Matters", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.CostCenterMatters", "Matter_Id", "dbo.Matters");
            DropForeignKey("dbo.CostCenterMatters", "CostCenter_Id", "dbo.CostCenters");
            DropForeignKey("dbo.Addresses", "OrganizationId", "dbo.Organizations");
            DropIndex("dbo.MatterOrganizations", new[] { "Organization_Id" });
            DropIndex("dbo.MatterOrganizations", new[] { "Matter_Id" });
            DropIndex("dbo.CostCenterMatters", new[] { "Matter_Id" });
            DropIndex("dbo.CostCenterMatters", new[] { "CostCenter_Id" });
            DropIndex("dbo.Phones", new[] { "OrganizationId" });
            DropIndex("dbo.Documents", new[] { "Matter_Id" });
            DropIndex("dbo.Matters", new[] { "MatterTypeId" });
            DropIndex("dbo.Matters", new[] { "CountryId" });
            DropIndex("dbo.Addresses", new[] { "State_Id" });
            DropIndex("dbo.Addresses", new[] { "OrganizationId" });
            DropTable("dbo.MatterOrganizations");
            DropTable("dbo.CostCenterMatters");
            DropTable("dbo.Users");
            DropTable("dbo.States");
            DropTable("dbo.Phones");
            DropTable("dbo.MatterTypes");
            DropTable("dbo.Documents");
            DropTable("dbo.Countries");
            DropTable("dbo.CostCenters");
            DropTable("dbo.Matters");
            DropTable("dbo.Organizations");
            DropTable("dbo.Addresses");
        }
    }
}
