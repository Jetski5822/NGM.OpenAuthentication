using NGM.OpenAuthentication.Extensions;
using Orchard;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IUsernameService : IDependency {
        string Calculate(string currentValue);
    }

    public class UsernameService : IUsernameService {
        private readonly IMembershipService _membershipService;

        public UsernameService(IMembershipService membershipService) {
            _membershipService = membershipService;
        }

        public string Calculate(string currentValue) {
            /* I Dont want to user an email address as a Username...*/
            string userName = currentValue.IsEmailAddress() ? currentValue.Substring(0, currentValue.IndexOf('@')) : currentValue;

            int uniqueValue = 0;

            string newUniqueUserName = userName;

            while (true) {
                if (_membershipService.GetUser(newUniqueUserName) == null)
                    break;

                newUniqueUserName = string.Format("{0}{1}", userName, uniqueValue);

                uniqueValue++;
            }

            return newUniqueUserName;
        }
    }
}