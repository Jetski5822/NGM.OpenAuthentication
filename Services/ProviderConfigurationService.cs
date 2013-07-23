using System.Collections.Generic;
using System.Linq;
using NGM.OpenAuthentication.Models;
using Orchard;
using Orchard.Data;

namespace NGM.OpenAuthentication.Services {
    public interface IProviderConfigurationService : IDependency {
        IEnumerable<ProviderConfigurationRecord> GetAll();
        ProviderConfigurationRecord Get(string providerName);
        void Delete(int id);
        void Create(ProviderConfigurationCreateParams parameters);
        bool VerifyUnicity(string providerName);
    }

    public class ProviderConfigurationService : IProviderConfigurationService {
        private readonly IRepository<ProviderConfigurationRecord> _repository;

        public ProviderConfigurationService(IRepository<ProviderConfigurationRecord> repository) {
            _repository = repository;
        }

        public IEnumerable<ProviderConfigurationRecord> GetAll() {
            return _repository.Table.ToList();
        }

        public ProviderConfigurationRecord Get(string providerName) {
            return _repository.Get(o => o.ProviderName == providerName);
        }

        public void Delete(int id) {
            _repository.Delete(_repository.Get(o => o.Id == id));
        }

        public bool VerifyUnicity(string providerName) {
            return _repository.Get(o => o.ProviderName == providerName) == null; 
        }

        public void Create(ProviderConfigurationCreateParams parameters) {
            _repository.Create(new ProviderConfigurationRecord {
                    DisplayName = parameters.DisplayName,
                    ProviderName = parameters.ProviderName,
                    ProviderIdentifier = parameters.ProviderIdentifier,
                    ProviderIdKey = parameters.ProviderIdKey,
                    ProviderSecret = parameters.ProviderSecret,
                    IsEnabled = true
                });
        }
    }

    public class ProviderConfigurationCreateParams {
        public string DisplayName { get; set; }
        public string ProviderName { get; set; }
        public string ProviderIdentifier { get; set; }
        public string ProviderIdKey { get; set; }
        public string ProviderSecret { get; set; }
    }
}