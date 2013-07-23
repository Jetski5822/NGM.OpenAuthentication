using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NGM.OpenAuthentication.Models;
using Orchard.Data;
using Orchard.ContentManagement.Handlers;

namespace NGM.OpenAuthentication.Handlers {
    [UsedImplicitly]
    public class UserProvidersPartHandler : ContentHandler {
        private readonly IRepository<UserProviderRecord> _userProviderRepository;

        public UserProvidersPartHandler(IRepository<UserProviderRecord> userProviderRepository) {
            _userProviderRepository = userProviderRepository;

            Filters.Add(new ActivatingFilter<UserProvidersPart>("User"));
            OnLoaded<UserProvidersPart>(LazyLoadHandlers);
        }

        protected void LazyLoadHandlers(LoadContentContext context, UserProvidersPart part) {
            // Add handlers that will load content for id's just-in-time
            part.ProviderEntriesField.Loader(x => OnLoader(context));
        }

        private IList<UserProviderEntry> OnLoader(LoadContentContext context) {
                return _userProviderRepository
                    .Fetch(x => x.UserId == context.ContentItem.Id)
                    .Select(x => new UserProviderEntry {
                        Id = x.Id,
                        ProviderUserId = x.ProviderUserId,
                        ProviderName = x.ProviderName
                    })
                    .ToList();
        }
    }
}