using Orchard;

namespace NGM.OpenAuthentication.Core {
    public interface IAccessControlProvider : IDependency {
        string Name { get; }
        string Hash { get; }
    }
}