using NGM.OpenAuthentication.Models;
using Orchard.Events;

namespace NGM.OpenAuthentication.Events {
    public interface IExtendedUserPropertiesEventHandler : IEventHandler {
        void Save(ExtendedUserPropertiesContext extendedUserPropertiesContext);
    }
}