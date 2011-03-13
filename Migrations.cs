using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

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
                   .WithPart("OpenAuthenticationPart")
                );
        
            return 1;
        }

        public int UpdateFrom1() {
            SchemaBuilder.AlterTable("OpenAuthenticationSettingsPartRecord", t => t.AddColumn<bool>("MicrosoftConnectEnabled"));

            return 2;
        }
    }
}