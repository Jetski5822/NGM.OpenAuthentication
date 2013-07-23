using System.Web.Security;
using Orchard;

namespace NGM.OpenAuthentication.Services {
    public interface IPasswordGeneratorService : IDependency {
        string Generate();
    }

    public class PasswordGeneratorService : IPasswordGeneratorService {
        public string Generate() {
            return Membership.GeneratePassword(10, 5);
        }
    }
}