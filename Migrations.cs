using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace NGM.OpenAuthentication {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("OpenAuthenticationPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<int>("UserId")
                    .Column<string>("ExternalIdentifier")
                    .Column<string>("ExternalDisplayIdentifier")
                    .Column<string>("OAuthToken")
                    .Column<string>("OAuthAccessToken")
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
                .Column<bool>("AutoRegisterEnabled")
               );

            ContentDefinitionManager.AlterTypeDefinition("User",
               cfg => cfg
                   .WithPart("OpenAuthenticationPart")
                );
        
            return 1;
        }

        public int UpgradeFrom1() {
            SchemaBuilder.CreateTable("OAuthProviderSettingsPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("Provider")
                    .Column<string>("ClientIdentifier")
                    .Column<string>("ClientSecret")
                );

            SchemaBuilder.ExecuteSql("Insert into OAuthProviderSettingsPartRecord (\"Provider\", \"ClientIdentifier\", \"ClientSecret\"))" +
                                     "Select \"Facebook\", FacebookClientIdentifier, FacebookClientSecret From OpenAuthenticationSettingsPartRecord");

            SchemaBuilder.ExecuteSql("Insert into OAuthProviderSettingsPartRecord (\"Provider\", \"ClientIdentifier\", \"ClientSecret\"))" +
                                     "Select \"Twitter\", FacebookClientIdentifier, FacebookClientSecret From OpenAuthenticationSettingsPartRecord");

            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", table => table.DropColumn("FacebookClientIdentifier"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", table => table.DropColumn("FacebookClientSecret"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", table => table.DropColumn("TwitterClientIdentifier"));
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", table => table.DropColumn("TwitterClientSecret"));

            return 2;
        }
    }
}