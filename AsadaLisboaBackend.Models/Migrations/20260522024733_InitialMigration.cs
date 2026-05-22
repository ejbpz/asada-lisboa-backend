using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AsadaLisboaBackend.Models.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUsSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SectionType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "character varying(2500)", maxLength: 2500, nullable: false),
                    Order = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Charges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Order = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Extension = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MimeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisualSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SettingType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Order = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisualSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FirstLastName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SecondLastName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChargeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Charges_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "Charges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", maxLength: 5000, nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastEditionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentsCategories",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentsCategories", x => new { x.CategoriesId, x.DocumentsId });
                    table.ForeignKey(
                        name: "FK_DocumentsCategories_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentsCategories_Documents_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImagesCategories",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImagesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesCategories", x => new { x.CategoriesId, x.ImagesId });
                    table.ForeignKey(
                        name: "FK_ImagesCategories_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImagesCategories_Images_ImagesId",
                        column: x => x.ImagesId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsCategories",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsCategories", x => new { x.CategoriesId, x.NewsId });
                    table.ForeignKey(
                        name: "FK_NewsCategories_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsCategories_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AboutUsSections",
                columns: new[] { "Id", "Content", "Order", "SectionType" },
                values: new object[,]
                {
                    { new Guid("02f9cf72-83fd-4df0-ba40-8fd8ae2e9c8e"), "Ser una organización comunal líder en la gestión del recurso hídrico a nivel local, reconocida por su eficiencia operativa, transparencia administrativa y compromiso con la sostenibilidad ambiental. La ASADA Urbanización Lisboa aspira a consolidarse como un modelo de referencia en la administración de servicios de agua potable, incorporando mejoras continuas en sus procesos, tecnología e infraestructura. Busca garantizar el acceso seguro y sostenible al agua para las generaciones presentes y futuras, fortaleciendo la participación comunitaria y fomentando una cultura de uso responsable del recurso hídrico.", (byte)3, "Visión" },
                    { new Guid("71dadbe4-7815-4994-a8da-7ae6154769df"), "La ASADA Urbanización Lisboa surge como una iniciativa comunal ante la necesidad de administrar de forma organizada el sistema de abastecimiento de agua potable de la comunidad. Con el crecimiento de la población y el aumento en la demanda del servicio, los vecinos se organizaron para garantizar una distribución eficiente, segura y continua del recurso hídrico.\nCon el paso del tiempo, la organización ha fortalecido su infraestructura mediante la implementación de pozos, tanques de almacenamiento y redes de distribución, así como el desarrollo de procesos administrativos y financieros que permiten la sostenibilidad del servicio. En la actualidad, la ASADA continúa enfocada en la mejora continua, promoviendo el uso responsable del agua y la participación activa de la comunidad.", (byte)1, "Historia" },
                    { new Guid("8c764a82-e07b-4f86-be81-dbb7a4478a7e"), "La ASADA Urbanización Lisboa se encarga de administrar, operar y mantener el sistema de acueducto comunal, asegurando la captación, almacenamiento y distribución del agua potable hacia los usuarios. Asimismo, gestiona la facturación y el cobro del servicio, brinda atención a los abonados, ejecuta labores de mantenimiento preventivo y correctivo en la infraestructura, y vela por la calidad del agua suministrada.\nParalelamente, cumple funciones administrativas y financieras, incluyendo la elaboración de informes, la organización de asambleas y el cumplimiento de regulaciones vigentes, además de promover el uso responsable del recurso hídrico y la protección del entorno ambiental.", (byte)4, "Funciones" },
                    { new Guid("e8fa8e42-a5a2-4d94-8110-613f639ad896"), "Brindar el servicio de agua potable a la comunidad de Urbanización Lisboa, garantizando altos estándares de calidad, continuidad y cobertura, mediante una gestión responsable y eficiente de los recursos hídricos y financieros.\nLa organización procura el adecuado mantenimiento, mejora y expansión de la infraestructura del acueducto, asegurando la sostenibilidad del servicio en el tiempo. Asimismo, promueve una atención oportuna y transparente a los abonados, contribuyendo a la salud pública, el bienestar social y el desarrollo integral de la comunidad, bajo principios de equidad, responsabilidad y compromiso ambiental.", (byte)2, "Misión" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("6fcf3478-3ced-400b-92a6-dbe6a9d37446"), "6FCF3478-3CED-400B-92A6-DBE6A9D37446", "Escritor", "ESCRITOR" },
                    { new Guid("864c6f86-5fdf-4279-857b-d7b8d99170b2"), "864C6F86-5FDF-4279-857B-D7B8D99170B2", "Administrador", "ADMINISTRADOR" },
                    { new Guid("caa2cbcd-3a76-4ed0-ab4f-7d07034692eb"), "CAA2CBCD-3A76-4ED0-AB4F-7D07034692EB", "Lector", "LECTOR" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("03e622ec-438b-49f3-8c53-3a3873651c16"), "Proyectos Ejecutados" },
                    { new Guid("2b3b492d-e243-4fc8-8217-03ea9998a201"), "Sugerencias" },
                    { new Guid("3643cd9f-99fe-4873-85f9-2a7b6b4dac28"), "Tanque Principal" },
                    { new Guid("560841fb-884b-45f6-9a09-18d4641b7af5"), "Hidrantes" },
                    { new Guid("5aa04636-cd02-4184-bbe0-603a2e785988"), "Convenios" },
                    { new Guid("5dcb6ba5-0084-4420-8c7e-152df55f8b0b"), "Recibos" },
                    { new Guid("64c63c8a-20e2-487d-a365-815e20978e81"), "Medidores" },
                    { new Guid("6da5cf2d-e946-43ed-a092-d5b4c9282ffa"), "Estados Financieros" },
                    { new Guid("712b5566-24bb-4681-9b7d-e636bdc2ab8e"), "Asamblea Ordinaria" },
                    { new Guid("71b9c756-b148-4e0f-a5c7-09a8a7aa209e"), "Dudas" },
                    { new Guid("77607c99-2ed1-4b8e-89cc-ccc7c6344e53"), "Lineamientos" },
                    { new Guid("783e94bc-4748-4223-a150-8892354b865b"), "Colindancia" },
                    { new Guid("81f11c15-3556-40be-8074-a6be7a5d5ab4"), "Informes" },
                    { new Guid("82bc31fd-3438-4b17-8216-a77c863bfb19"), "SINPE Móvil" },
                    { new Guid("88af46be-8e2e-4451-a746-ee0864e7145a"), "Ahorro" },
                    { new Guid("93ca9020-78fd-4080-8d57-469eb05d9dc3"), "Reglamentos" },
                    { new Guid("a12d2ab5-24d2-420c-90d2-2cc3468a33f3"), "Evento" },
                    { new Guid("b2dbcf36-fddf-4752-b39d-b31f61e704a3"), "Estudios" },
                    { new Guid("ba84f8ac-ead7-4cb3-83b4-77238df95884"), "Solicitudes" },
                    { new Guid("be75e73d-145e-41b1-b3ac-149abc0ade06"), "Mantenimiento" },
                    { new Guid("c5d3b3b3-e69b-48d9-93a5-a4803af196eb"), "Asamblea Extraordinaria" },
                    { new Guid("cc1f685c-356e-46ec-a027-3c570fdc6009"), "Reciclaje" },
                    { new Guid("ceb0bb8a-0414-474c-b378-46491b4c08e8"), "Exámenes" },
                    { new Guid("f62ed863-70d4-446b-9088-10efd6cb6c77"), "Pozo Principal" }
                });

            migrationBuilder.InsertData(
                table: "Charges",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0feb0b33-cf81-4973-9754-e45de997069d"), "Administrador" },
                    { new Guid("1bce957a-451a-425a-b438-58c0227ff9fc"), "Secretario" },
                    { new Guid("28524859-89fd-4f06-a39c-c62150c1284c"), "Vocal 3" },
                    { new Guid("56f6b0c4-9e67-4cb6-ac31-d95ac0d10271"), "Presidente" },
                    { new Guid("64849ca6-6daa-4174-a240-a640cd09509f"), "Vocal 2" },
                    { new Guid("7d591140-46fa-47a3-9875-d450896c8b14"), "Vicepresidente" },
                    { new Guid("9db79916-bac2-479a-84a1-dfb13c83dda8"), "Vocal 1" },
                    { new Guid("bee47254-066c-414b-a872-3cc72d708a67"), "Fiscal" },
                    { new Guid("c8fcb329-dcaf-4352-80fc-5cc7730482e7"), "Tesorero" }
                });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "ContactType", "Order", "Value" },
                values: new object[,]
                {
                    { new Guid("0ac0c004-b812-4f29-855f-5813e89146d9"), "Correo electrónico", (byte)1, "asadaurblisboa@gmail.com" },
                    { new Guid("37dd984f-7059-48ae-a6d5-cf57d8c2667b"), "Facebook", (byte)5, "https://www.facebook.com/asadalisboa/" },
                    { new Guid("491d6c63-98f0-44f9-b837-ff9640d01921"), "Ubicación", (byte)4, "https://maps.app.goo.gl/7vArCkz5iEG5abq48" },
                    { new Guid("a78f2955-59b4-4c88-a429-b9644b19552c"), "Teléfono Fijo", (byte)3, "+506 2433-3882" },
                    { new Guid("cee070c5-4fa6-40a3-ae12-db5f2ea8bad1"), "Teléfono Celular", (byte)2, "+506 8459-5494" },
                    { new Guid("e0b3df15-177c-4581-8fc0-944b205d3b3a"), "Horario", (byte)6, "Lunes a viernes: 8:00 a.m. - 12:00 p.m." }
                });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "Id", "Extension", "MimeType", "Name" },
                values: new object[,]
                {
                    { new Guid("1e42a6e9-0baa-4c51-8812-e459e0678130"), ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Excel" },
                    { new Guid("1fa5211f-f4ea-42d1-8c5e-dee71d9a75a7"), ".doc", "application/msword", "Word" },
                    { new Guid("3f7294fc-4d1c-4a58-8576-9e2d404b2846"), ".xls", "application/vnd.ms-excel", "Excel" },
                    { new Guid("55931b3a-ae2b-4aec-8625-7c9adbdfab5f"), ".csv", "text/csv", "CSV" },
                    { new Guid("9029eb69-0e77-4cf6-8622-c7902d565745"), ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Word" },
                    { new Guid("d8fbe9b6-1a5e-41ea-98ea-e6641a6047c8"), ".pdf", "application/pdf", "PDF" },
                    { new Guid("d9ec3706-86f8-4c3d-a770-e3e38221a7e8"), ".txt", "text/plain", "Texto" },
                    { new Guid("f4365613-d7e7-488e-b24e-4834645408d6"), ".zip", "application/octet-stream", "ZIP" }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2c36a4b8-de3e-4607-9b7b-dbe0f0e00390"), "Borrador" },
                    { new Guid("5c1cebda-fc8c-44ac-997c-aaf015572d46"), "Publicado" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsSections_Order",
                table: "AboutUsSections",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChargeId",
                table: "AspNetUsers",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Charges_Name",
                table: "Charges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Order",
                table: "Contacts",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentTypeId",
                table: "Documents",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PublicationDate",
                table: "Documents",
                column: "PublicationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Slug",
                table: "Documents",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_StatusId",
                table: "Documents",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentsCategories_DocumentsId",
                table: "DocumentsCategories",
                column: "DocumentsId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_Extension",
                table: "DocumentTypes",
                column: "Extension",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_PublicationDate",
                table: "Images",
                column: "PublicationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Images_Slug",
                table: "Images",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_StatusId",
                table: "Images",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesCategories_ImagesId",
                table: "ImagesCategories",
                column: "ImagesId");

            migrationBuilder.CreateIndex(
                name: "IX_News_PublicationDate",
                table: "News",
                column: "PublicationDate");

            migrationBuilder.CreateIndex(
                name: "IX_News_Slug",
                table: "News",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_StatusId",
                table: "News",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsCategories_NewsId",
                table: "NewsCategories",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Name",
                table: "Statuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisualSettings_Order",
                table: "VisualSettings",
                column: "Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUsSections");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "DocumentsCategories");

            migrationBuilder.DropTable(
                name: "ImagesCategories");

            migrationBuilder.DropTable(
                name: "NewsCategories");

            migrationBuilder.DropTable(
                name: "VisualSettings");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Charges");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "Statuses");
        }
    }
}
