using System;
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

        public bool IsPermissionEnabled(string scope, Provider provider) {
            return Get(provider).Where(o => o.Scope == scope).FirstOrDefault() != null;
        }

        public IEnumerable<ScopeProviderPermissionRecord> Get(Provider provider) {
            return GetAll().Where(o => o.HashedProvider == ProviderHelpers.GetHashedProvider(provider));
        }

        public void Create(Provider provider, ScopePermission permissionProvider) {
            _scopeProviderPermissionRecordRepository.Create(new ScopeProviderPermissionRecord {
                Resource = permissionProvider.Resource,
                Description = permissionProvider.Description,
                HashedProvider = ProviderHelpers.GetHashedProvider(provider),
                IsEnabled = permissionProvider.IsEnabled,
                Scope = permissionProvider.Scope
            });
        }

        public void Update(Dictionary<int, bool> providerPermissions) {
            var providerPermissionRecords = GetAll();
            foreach (var providerPermissionRecord in providerPermissionRecords) {
                if (providerPermissions.ContainsKey(providerPermissionRecord.Id))
                    providerPermissionRecord.IsEnabled = providerPermissionRecord.IsEnabled;
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