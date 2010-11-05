using NGM.OpenAuthentication.Models;
using Orchard.Events;

namespace NGM.OpenAuthentication.Events {
    public interface IExtendedPropertiesEventHandler : IEventHandler {
        void Save(ExtendedProperties extendedProperties);
    }
}