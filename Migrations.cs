using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace NGM.OpenAuthentication {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("OpenAuthenticationPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<int>("UserId")
                    .Column<string>("ClaimedIdentifier")
                    .Column<string>("FriendlyIdentifierForDisplay")
                );

            SchemaBuilder.CreateTable("OpenAuthenticationSettingsPartRecord", table => table
                .ContentPartRecord()
                .Column<bool>("Birthdate")
                .Column<bool>("Country")
                .Column<bool>("Email")
                .Column<bool>("FullName")
                .Column<bool>("Gender")
                .Column<bool>("Language")
                .Column<bool>("Nickname")
                .Column<bool>("PostalCode")
                .Column<bool>("TimeZone")
               );

            ContentDefinitionManager.AlterTypeDefinition("User",
               cfg => cfg
                   .WithPart("OpenAuthenticationPart")
                );
        
            return 1;
        }
    }
}