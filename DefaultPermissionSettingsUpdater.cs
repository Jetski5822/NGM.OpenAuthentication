using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using JetBrains.Annotations;
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

        public DefaultPermissionSettingsUpdater(IScopeProviderPermissionService scopeProviderPermissionService) {
            _scopeProviderPermissionService = scopeProviderPermissionService;

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


        }
    }
}