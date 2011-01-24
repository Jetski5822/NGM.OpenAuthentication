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

            ContentDefinitionManager.AlterTypeDefinition("User",
               cfg => cfg
                   .WithPart("OpenAuthenticationPart")
                );
        
            return 1;
        }
    }
}