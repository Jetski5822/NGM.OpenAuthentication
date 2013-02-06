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
            OnLoaded<UserProvidersPart>((context, userOpenAuthentication) => {
                                    userOpenAuthentication.Providers = _userProviderRepository
                                        .Fetch(x => x.UserId == context.ContentItem.Id)
                                        .Select(x => new UserProviderEntry {
                                                Id = x.Id,
                                                ProviderUserId = x.ProviderUserId, 
                                                ProviderName = x.ProviderName
                                            })
                                        .ToList();
                                });
        }
    }
}