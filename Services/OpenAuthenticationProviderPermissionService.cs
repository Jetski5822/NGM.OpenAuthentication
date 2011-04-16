using System;
using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Services {
    public class OpenAuthenticationProviderPermissionService : IOpenAuthenticationProviderPermissionService {
        public bool IsPermissionEnabled(string namedPermission, Provider provider) {
            return false;
        }
    }
}