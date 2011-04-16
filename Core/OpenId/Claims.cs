using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.Messages;
using NGM.OpenAuthentication.Services;

namespace NGM.OpenAuthentication.Core.OpenId {
    public static class Claims {
        public static IOpenIdMessageExtension CreateClaimsRequest(
            IOpenAuthenticationProviderPermissionService openAuthenticationProviderPermissionService) {

            var claimsRequest = new ClaimsRequest();

            if (openAuthenticationProviderPermissionService.IsPermissionEnabled("Birthdate", Provider.OpenId))
                claimsRequest.BirthDate = DemandLevel.Require;

            if (openAuthenticationProviderPermissionService.IsPermissionEnabled("Country", Provider.OpenId))
                claimsRequest.Country = DemandLevel.Require;

//            if (openAuthenticationProviderPermissionService.IsPermissionEnabled("Email", Provider.OpenId))
                claimsRequest.Email = DemandLevel.Require;

//            if (openAuthenticationProviderPermissionService.IsPermissionEnabled("FullName", Provider.OpenId))
                claimsRequest.FullName = DemandLevel.Require;

            if (openAuthenticationProviderPermissionService.IsPermissionEnabled("Gender", Provider.OpenId))
                claimsRequest.Gender = DemandLevel.Require;

            if (openAuthenticationProviderPermissionService.IsPermissionEnabled("Language", Provider.OpenId))
                claimsRequest.Language = DemandLevel.Require;

            if (openAuthenticationProviderPermissionService.IsPermissionEnabled("Nickname", Provider.OpenId))
                claimsRequest.Nickname = DemandLevel.Require;

            if (openAuthenticationProviderPermissionService.IsPermissionEnabled("PostalCode", Provider.OpenId))
                claimsRequest.PostalCode = DemandLevel.Require;

            if (openAuthenticationProviderPermissionService.IsPermissionEnabled("TimeZone", Provider.OpenId))
                claimsRequest.TimeZone = DemandLevel.Require;

            return claimsRequest;
        }

        public static FetchRequest CreateFetchRequest(IOpenAuthenticationProviderPermissionService openAuthenticationProviderPermissionService) {
            var fetchRequest = new FetchRequest();

            //if (openAuthenticationProviderPermissionService.IsPermissionEnabled("Email", Provider.OpenId))
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.Email);

            //if (openAuthenticationProviderPermissionService.IsPermissionEnabled("FullName", Provider.OpenId)) {
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.First);
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Last);
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.FullName);
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Alias);
            //}

            return fetchRequest;
        }
    }
}