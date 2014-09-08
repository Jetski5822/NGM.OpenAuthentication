using System.Data;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace NGM.OpenAuthentication {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("UserProviderRecord",
                            table => table
                                .Column<int>("Id", column => column.PrimaryKey().Identity())
                                .Column<int>("UserId")
                                .Column<string>("ProviderName")
                                .Column<string>("ProviderUserId")
                            );

            SchemaBuilder.CreateTable("ProviderConfigurationRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("IsEnabled")
                    .Column<string>("DisplayName")
                    .Column<string>("ProviderName")
                    .Column<string>("ProviderIdKey")
                    .Column<string>("ProviderSecret")
                    .Column<string>("ProviderIdentifier")
                );

            return 8;
        }

        public int UpdateFrom1() {
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.AddColumn<bool>("MicrosoftConnectEnabled"));

            return 2;
        }

        public int UpdateFrom2() {
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("MicrosoftConnectEnabled"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("OpenIdEnabled"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("CardSpaceEnabled"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("OAuthEnabled"));

            return 3;
        }

        public int UpdateFrom3() {
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("Birthdate"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("Country"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("Email"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("FullName"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("Gender"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("Language"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("Nickname"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("PostalCode"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("TimeZone"));

            SchemaBuilder.CreateTable("OpenAuthenticationPermissionSettingsPartRecord", table => table
                .ContentPartRecord()
                .Column<string>("NamedPermission")
                .Column<bool>("IsEnabled")
                .Column<int>("HashedProvider")
               );

            return 4;
        }

        public int UpdateFrom4() {
            ContentDefinitionManager.AlterTypeDefinition("OpenAuthentication",
               cfg => cfg
                   .WithPart("OpenAuthenticationPart"));

            return 5;
        }
        
        public int UpdateFrom5() {
            SchemaBuilder.DropTable("OpenAuthenticationPermissionSettingsPartRecord");

            SchemaBuilder.CreateTable("ScopeProviderPermissionRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Resource")
                    .Column<string>("Scope")
                    .Column<string>("Description")
                    .Column<bool>("IsEnabled")
                    .Column<int>("HashedProvider")
                );

            return 6;
        }

        public int UpdateFrom6() {
            SchemaBuilder.AlterTable("ScopeProviderPermissionRecord", table => table.AlterColumn("HashedProvider", x => x.WithType(DbType.String)));
            SchemaBuilder.AlterTable("OpenAuthenticationPartRecord", table => table.AlterColumn("HashedProvider", x => x.WithType(DbType.String)));
            
            return 7;
        }

        public int UpdateFrom7() {
            ContentDefinitionManager.AlterTypeDefinition("User", cfg => cfg.RemovePart("OpenAuthenticationPart"));

            SchemaBuilder.DropTable("OpenAuthenticationPartRecord");
            SchemaBuilder.DropTable("ScopeProviderPermissionRecord");

            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("AutoRegisterEnabled"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("FacebookClientIdentifier"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("FacebookClientSecret"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("TwitterClientIdentifier"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("TwitterClientSecret"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("LiveIdClientIdentifier"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.DropColumn("LiveIdClientSecret"));
                
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.AddColumn<bool>("AutoRegistrationEnabled"));

            SchemaBuilder.CreateTable("UserProviderRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("UserId")
                    .Column<string>("ProviderName")
                    .Column<string>("ProviderUserId")
                );

            SchemaBuilder.CreateTable("ProviderConfigurationRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("IsEnabled")
                    .Column<string>("DisplayName")
                    .Column<string>("ProviderName")
                    .Column<string>("ProviderIdKey")
                    .Column<string>("ProviderSecret")
                    .Column<string>("ProviderIdentifier")
                );

            return 8;
        }
    }
}