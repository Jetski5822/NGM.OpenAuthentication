using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using JetBrains.Annotations;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Services;
using Orchard.Environment;
using Orchard.Environment.Extensions.Models;
using Orchard.Logging;

namespace NGM.OpenAuthentication
{
    [UsedImplicitly]
    public class DefaultPermissionSettingsUpdater : IFeatureEventHandler
    {
        private readonly IScopeProviderPermissionService _scopeProviderPermissionService;
        private readonly IEnumerable<IScopePermissionProvider> _scopePermissionProviders;

        public DefaultPermissionSettingsUpdater(IScopeProviderPermissionService scopeProviderPermissionService,
            IEnumerable<IScopePermissionProvider> scopePermissionProviders) {
            _scopeProviderPermissionService = scopeProviderPermissionService;
            _scopePermissionProviders = scopePermissionProviders;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        void IFeatureEventHandler.Installing(Feature feature)
        {
            AddDefaultPermissionsForFeature(feature);
        }

        void IFeatureEventHandler.Installed(Feature feature)
        {
        }

        void IFeatureEventHandler.Enabling(Feature feature)
        {
        }

        void IFeatureEventHandler.Enabled(Feature feature)
        {
        }

        void IFeatureEventHandler.Disabling(Feature feature)
        {
        }

        void IFeatureEventHandler.Disabled(Feature feature)
        {
        }

        void IFeatureEventHandler.Uninstalling(Feature feature)
        {
        }

        void IFeatureEventHandler.Uninstalled(Feature feature)
        {
        }

        public void AddDefaultPermissionsForFeature(Feature feature) {
            var featureName = feature.Descriptor.Id;

            foreach (var scopePermissionProvider in _scopePermissionProviders.Where(x => x.Feature.Descriptor.Id == featureName)) {
                foreach (var permissionProvider in scopePermissionProvider.GetPermissions()) {
                    _scopeProviderPermissionService.Create(scopePermissionProvider.Provider, permissionProvider);
                }
            }

        }
    }
}