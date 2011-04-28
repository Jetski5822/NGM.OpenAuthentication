using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.Messages;
using NGM.OpenAuthentication.Services;

namespace NGM.OpenAuthentication.Core.OpenId {
    public static class Claims {
        public static IOpenIdMessageExtension CreateClaimsRequest(
            IScopeProviderPermissionService scopeProviderPermissionService)
        {
            var claimsRequest = new ClaimsRequest();

            if (scopeProviderPermissionService.IsPermissionEnabled("Birthdate", Provider.OpenId))
                claimsRequest.BirthDate = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("Country", Provider.OpenId))
                claimsRequest.Country = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("Email", Provider.OpenId))
                claimsRequest.Email = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("FullName", Provider.OpenId))
                claimsRequest.FullName = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("Gender", Provider.OpenId))
                claimsRequest.Gender = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("Language", Provider.OpenId))
                claimsRequest.Language = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("Nickname", Provider.OpenId))
                claimsRequest.Nickname = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("PostalCode", Provider.OpenId))
                claimsRequest.PostalCode = DemandLevel.Require;

            if (scopeProviderPermissionService.IsPermissionEnabled("TimeZone", Provider.OpenId))
                claimsRequest.TimeZone = DemandLevel.Require;

            return claimsRequest;
        }

        public static FetchRequest CreateFetchRequest(IScopeProviderPermissionService scopeProviderPermissionService)
        {
            var fetchRequest = new FetchRequest();

            if (scopeProviderPermissionService.IsPermissionEnabled("Email", Provider.OpenId))
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.Email);

            if (scopeProviderPermissionService.IsPermissionEnabled("FullName", Provider.OpenId)) {
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.First);
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Last);
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.FullName);
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Alias);
            }

            return fetchRequest;
        }
    }
}