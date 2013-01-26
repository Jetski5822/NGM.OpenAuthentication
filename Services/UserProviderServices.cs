using NGM.OpenAuthentication.Models;
using Orchard;
using Orchard.Data;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IUserProviderServices : IDependency {
        UserProviderRecord Get(string providerName, string providerUserId);
        void Create(string providerName, string providerUserId, IUser user);
        void Update(string providerName, string providerUserId, IUser user);
    }

    public class UserProviderServices : IUserProviderServices {
        private readonly IRepository<UserProviderRecord> _repository;

        public UserProviderServices(IRepository<UserProviderRecord> repository) {
            _repository = repository;
        }

        public UserProviderRecord Get(string providerName, string providerUserId) {
            return _repository.Get(o => o.ProviderName == providerName && o.ProviderUserId == providerUserId);
        }

        public void Create(string providerName, string providerUserId, IUser user) {
            var record = new UserProviderRecord
                {
                    UserId = user.Id,
                    ProviderName = providerName,
                    ProviderUserId = providerUserId
                };

            _repository.Create(record);
        }

        public void Update(string providerName, string providerUserId, IUser user) {
            var record = Get(providerName, providerUserId);

            record.UserId = user.Id;

            _repository.Update(record);
        }
    }
}