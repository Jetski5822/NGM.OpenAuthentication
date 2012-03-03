using System.Linq;
using System.Collections.Generic;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;
using Orchard.Data;

namespace NGM.OpenAuthentication.Services {
    public class ScopeProviderPermissionService : IScopeProviderPermissionService {
        private readonly IRepository<ScopeProviderPermissionRecord> _scopeProviderPermissionRecordRepository;

        public ScopeProviderPermissionService(IRepository<ScopeProviderPermissionRecord> scopeProviderPermissionRecordRepository) {
            _scopeProviderPermissionRecordRepository = scopeProviderPermissionRecordRepository;
        }

        public bool IsPermissionEnabled(string scope, AccessControlProvider provider) {
            return Get(provider).FirstOrDefault(o => o.Scope == scope) != null;
        }

        public IEnumerable<ScopeProviderPermissionRecord> Get(AccessControlProvider provider) {
            return GetAll().Where(o => o.HashedProvider == provider.Hash);
        }

        public void Create(AccessControlProvider provider, ScopePermission permissionProvider) {
            _scopeProviderPermissionRecordRepository.Create(new ScopeProviderPermissionRecord {
                Resource = permissionProvider.Resource,
                Description = permissionProvider.Description,
                HashedProvider = provider.Hash,
                IsEnabled = permissionProvider.IsEnabled,
                Scope = permissionProvider.Scope
            });
        }

        public void Update(Dictionary<int, bool> providerPermissions) {
            var providerPermissionRecords = GetAll();
            foreach (var providerPermissionRecord in providerPermissionRecords) {
                if (providerPermissions.ContainsKey(providerPermissionRecord.Id))
                    providerPermissionRecord.IsEnabled = providerPermissions[providerPermissionRecord.Id];
                else
                    providerPermissionRecord.IsEnabled = false;
                
                _scopeProviderPermissionRecordRepository.Update(providerPermissionRecord);
            }
        }

        public IEnumerable<ScopeProviderPermissionRecord> GetAll() {
            return _scopeProviderPermissionRecordRepository.Table.AsEnumerable();
        }
    }
}