using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("OpenAuthenticationPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<int>("UserId")
                    .Column<string>("ExternalIdentifier", c => c.WithLength(1000))
                    .Column<string>("ExternalDisplayIdentifier", c => c.WithLength(500))
                    .Column<string>("OAuthToken", c => c.WithLength(1000))
                    .Column<string>("OAuthAccessToken", c => c.WithLength(1000))
                    .Column<int>("HashedProvider")
                );

            SchemaBuilder.CreateTable("OpenAuthenticationSettingsPartRecord", table => table
                .ContentPartRecord()
                .Column<bool>("OpenIdEnabled")
                .Column<bool>("CardSpaceEnabled")
                .Column<bool>("OAuthEnabled")
                .Column<bool>("Birthdate")
                .Column<bool>("Country")
                .Column<bool>("Email")
                .Column<bool>("FullName")
                .Column<bool>("Gender")
                .Column<bool>("Language")
                .Column<bool>("Nickname")
                .Column<bool>("PostalCode")
                .Column<bool>("TimeZone")
                .Column<string>("FacebookClientIdentifier")
                .Column<string>("FacebookClientSecret")
                .Column<string>("TwitterClientIdentifier")
                .Column<string>("TwitterClientSecret")
                .Column<string>("LiveIdClientIdentifier")
                .Column<string>("LiveIdClientSecret")
                .Column<bool>("AutoRegisterEnabled")
               );

            ContentDefinitionManager.AlterTypeDefinition("User",
               cfg => cfg
                   .WithPart("OpenAuthenticationPart"));
        
            return 1;
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
    }

    [OrchardFeature("MicrosoftConnect")]
    public class MicrosoftConnectMigrations : DataMigrationImpl {
        public int Create() {
            ContentDefinitionManager.AlterPartDefinition(typeof(MicrosoftConnectSignInPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("MicrosoftConnectSignInWidget", cfg => cfg
                .WithPart("MicrosoftConnectSignInPart")
                .WithPart("WidgetPart")
                .WithPart("CommonPart")
                .WithSetting("Stereotype", "Widget"));

            return 1;
        }
    }

    //[OrchardFeature("Facebook")]
    //public class FacebookConnectMigrations : DataMigrationImpl {
    //    public int Create() {
    //        ContentDefinitionManager.AlterPartDefinition(typeof(FacebookConnectSignInPart).Name, cfg => cfg.Attachable());

    //        ContentDefinitionManager.AlterTypeDefinition("FacebookConnectSignInWidget", cfg => cfg
    //            .WithPart("FacebookConnectSignInPart")
    //            .WithPart("WidgetPart")
    //            .WithPart("CommonPart")
    //            .WithSetting("Stereotype", "Widget"));

    //        return 1;
    //    }
    //}
}