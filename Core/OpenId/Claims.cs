using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Core.OpenId {
    public static class Claims {
        public static ClaimsRequest CreateClaimsRequest(OpenAuthenticationSettingsPart openAuthenticationSettings) {
            var claimsRequest = new ClaimsRequest();

            var openAuthenticationSettingsRecord = openAuthenticationSettings.Record;

            if (openAuthenticationSettingsRecord == null)
                return claimsRequest;

            if (openAuthenticationSettingsRecord.Birthdate == true)
                claimsRequest.BirthDate = DemandLevel.Require;

            if (openAuthenticationSettingsRecord.Country == true)
                claimsRequest.Country = DemandLevel.Require;

            if (openAuthenticationSettingsRecord.Email == true)
                claimsRequest.Email = DemandLevel.Require;

            if (openAuthenticationSettingsRecord.FullName == true)
                claimsRequest.FullName = DemandLevel.Require;

            if (openAuthenticationSettingsRecord.Gender == true)
                claimsRequest.Gender = DemandLevel.Require;

            if (openAuthenticationSettingsRecord.Language == true)
                claimsRequest.Language = DemandLevel.Require;

            if (openAuthenticationSettingsRecord.Nickname == true)
                claimsRequest.Nickname = DemandLevel.Require;

            if (openAuthenticationSettingsRecord.PostalCode == true)
                claimsRequest.PostalCode = DemandLevel.Require;

            if (openAuthenticationSettingsRecord.TimeZone == true)
                claimsRequest.TimeZone = DemandLevel.Require;


            return claimsRequest;
        }

        public static FetchRequest CreateFetchRequest(OpenAuthenticationSettingsPart openAuthenticationSettings) {
            var fetchRequest = new FetchRequest();

            var openAuthenticationSettingsRecord = openAuthenticationSettings.Record;

            if (openAuthenticationSettingsRecord == null)
                return fetchRequest;

            if (openAuthenticationSettingsRecord.Email == true)
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.Email);

            if (openAuthenticationSettingsRecord.FullName == true) {
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.First);
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Last);
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.FullName);
                fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Alias);
            }

            return fetchRequest;
        }
    }
}