using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.Messages;
using NGM.OpenAuthentication.Services;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.OpenId.Services {
    [OrchardFeature("OpenId")]
    public static class Claims {
        public static IOpenIdMessageExtension CreateClaimsRequest(
            IScopeProviderPermissionService scopeProviderPermissionService)
        {
            var claimsRequest = new ClaimsRequest();

            var openIdAccessControl = new OpenIdAccessControlProvider();

            if (scopeProviderPermissionService.IsPermissionEnabled("Birthdate", openIdAccessControl))
                claimsRequest.BirthDate = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("Country", openIdAccessControl))
                claimsRequest.Country = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("Email", openIdAccessControl))
                claimsRequest.Email = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("FullName", openIdAccessControl))
                claimsRequest.FullName = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("Gender", openIdAccessControl))
                claimsRequest.Gender = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("Language", openIdAccessControl))
                claimsRequest.Language = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("Nickname", openIdAccessControl))
                claimsRequest.Nickname = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("PostalCode", openIdAccessControl))
                claimsRequest.PostalCode = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("TimeZone", openIdAccessControl))
                claimsRequest.TimeZone = DemandLevel.Require;

            return claimsRequest;
        }

        public static FetchRequest CreateFetchRequest(IScopeProviderPermissionService scopeProviderPermissionService)
        {
            var fetchRequest = new FetchRequest();

            var openIdAccessControl = new OpenIdAccessControlProvider();

            if (scopeProviderPermissionService.IsPermissionEnabled("Email", openIdAccessControl))
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.Email);

            if (scopeProviderPermissionService.IsPermissionEnabled("FullName", openIdAccessControl)) {
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.First);
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Last);
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.FullName);
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Alias);
            }

            return fetchRequest;
        }
    }
}