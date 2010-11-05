using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core.OpenId.Mappers;
using NGM.OpenAuthentication.Events;
using NGM.OpenAuthentication.Services;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core.OpenId {
    public class AccountAssociatior : IAccountAssociatior<IAuthenticationResponse> {
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IExtendedPropertiesEventHandler _extendedPropertiesEventHandler;

        public AccountAssociatior(IOpenAuthenticationService openAuthenticationService, IExtendedPropertiesEventHandler extendedPropertiesEventHandler) {
            _openAuthenticationService = openAuthenticationService;
            _extendedPropertiesEventHandler = extendedPropertiesEventHandler;
        }

        public void Associate(IAuthenticationResponse credentials, IUser user) {
            var mapper = new ClaimsResponseToExtendedUserPropertiesContextMapper();
            var context = mapper.Map(credentials.GetExtension<ClaimsResponse>());

            _openAuthenticationService.AssociateOpenIdWithUser(user, new OpenIdIdentifier(credentials.ClaimedIdentifier));

            _extendedPropertiesEventHandler.Save(context);
        }
    }

    public interface IAccountAssociatior<T> {
        void Associate(T credentials, IUser user);
    }
}