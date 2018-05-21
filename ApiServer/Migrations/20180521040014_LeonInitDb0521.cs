using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonInitDb0521 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetCategories",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DisplayIndex = table.Column<int>(nullable: false),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetCategoryTrees",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LValue = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NodeType = table.Column<string>(nullable: true),
                    ObjId = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    RValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCategoryTrees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetTags",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Dependencies = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FileAssetId = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CategoryId = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Dependencies = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FileAssetId = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Parameters = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionTrees",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LValue = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NodeType = table.Column<string>(nullable: true),
                    ObjId = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    RValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionTrees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Skirtings",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CategoryId = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skirtings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticMeshs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Dependencies = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FileAssetId = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticMeshs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Textures",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Dependencies = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FileAssetId = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Textures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountOpenId",
                columns: table => new
                {
                    OpenId = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Platform = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountOpenId", x => x.OpenId);
                });

            migrationBuilder.CreateTable(
                name: "AssetFolders",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CategoryId = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetFolders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientAssets",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CategoryId = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAssets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CategoryId = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FileExt = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    LocalPath = table.Column<string>(nullable: true),
                    Md5 = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false),
                    Size = table.Column<long>(nullable: false),
                    UploadTime = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Layouts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CategoryId = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    ActiveFlag = table.Column<int>(nullable: false),
                    ChildOrders = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: true),
                    StateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Mail = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Organizations_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActivationTime = table.Column<DateTime>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    DepartmentId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ExpireTime = table.Column<DateTime>(nullable: false),
                    Frozened = table.Column<bool>(nullable: false),
                    Icon = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Mail = table.Column<string>(nullable: true),
                    MailValid = table.Column<bool>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    PhoneValid = table.Column<bool>(nullable: false),
                    ResourceType = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganMember",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    DepartmentId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    JoinDepartmentTime = table.Column<DateTime>(nullable: false),
                    JoinOrganTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false),
                    Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganMember_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganMember_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganMember_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    Permission = table.Column<byte>(nullable: false),
                    ResId = table.Column<string>(nullable: true),
                    ResType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CategoryId = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Solutions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CategoryId = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    LayoutId = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solutions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solutions_Layouts_LayoutId",
                        column: x => x.LayoutId,
                        principalTable: "Layouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solutions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSpec",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    Album = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    ProductId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false),
                    StaticMeshIds = table.Column<string>(nullable: true),
                    TPID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpec", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSpec_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderStateItem",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Detail = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NewState = table.Column<string>(nullable: true),
                    OldState = table.Column<string>(nullable: true),
                    OperateTime = table.Column<DateTime>(nullable: false),
                    OperatorAccount = table.Column<string>(nullable: true),
                    OrderId = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    SolutionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStateItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStateItem_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderStateItem_Solutions_SolutionId",
                        column: x => x.SolutionId,
                        principalTable: "Solutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountOpenId_AccountId",
                table: "AccountOpenId",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_DepartmentId",
                table: "Accounts",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetFolders_AccountId",
                table: "AssetFolders",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetFolders_OrganizationId",
                table: "AssetFolders",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAssets_AccountId",
                table: "ClientAssets",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAssets_OrganizationId",
                table: "ClientAssets",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_OrganizationId",
                table: "Departments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentId",
                table: "Departments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_AccountId",
                table: "Files",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_OrganizationId",
                table: "Files",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Layouts_AccountId",
                table: "Layouts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Layouts_OrganizationId",
                table: "Layouts",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AccountId",
                table: "Orders",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrganizationId",
                table: "Orders",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateItem_OrderId",
                table: "OrderStateItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateItem_SolutionId",
                table: "OrderStateItem",
                column: "SolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OwnerId",
                table: "Organizations",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ParentId",
                table: "Organizations",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganMember_AccountId",
                table: "OrganMember",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganMember_DepartmentId",
                table: "OrganMember",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganMember_OrganizationId",
                table: "OrganMember",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_AccountId_ResId_ResType",
                table: "Permissions",
                columns: new[] { "AccountId", "ResId", "ResType" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_AccountId",
                table: "Products",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrganizationId",
                table: "Products",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpec_ProductId",
                table: "ProductSpec",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_AccountId",
                table: "Solutions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_LayoutId",
                table: "Solutions",
                column: "LayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_OrganizationId",
                table: "Solutions",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountOpenId_Accounts_AccountId",
                table: "AccountOpenId",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetFolders_Accounts_AccountId",
                table: "AssetFolders",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetFolders_Organizations_OrganizationId",
                table: "AssetFolders",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAssets_Accounts_AccountId",
                table: "ClientAssets",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAssets_Organizations_OrganizationId",
                table: "ClientAssets",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Accounts_AccountId",
                table: "Files",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Organizations_OrganizationId",
                table: "Files",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Layouts_Accounts_AccountId",
                table: "Layouts",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Layouts_Organizations_OrganizationId",
                table: "Layouts",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Accounts_AccountId",
                table: "Orders",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Organizations_OrganizationId",
                table: "Orders",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Accounts_OwnerId",
                table: "Organizations",
                column: "OwnerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Accounts_OwnerId",
                table: "Organizations");

            migrationBuilder.DropTable(
                name: "AccountOpenId");

            migrationBuilder.DropTable(
                name: "AssetCategories");

            migrationBuilder.DropTable(
                name: "AssetCategoryTrees");

            migrationBuilder.DropTable(
                name: "AssetFolders");

            migrationBuilder.DropTable(
                name: "AssetTags");

            migrationBuilder.DropTable(
                name: "ClientAssets");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "OrderStateItem");

            migrationBuilder.DropTable(
                name: "OrganMember");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "PermissionTrees");

            migrationBuilder.DropTable(
                name: "ProductSpec");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Skirtings");

            migrationBuilder.DropTable(
                name: "StaticMeshs");

            migrationBuilder.DropTable(
                name: "Textures");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Solutions");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Layouts");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
