using Orchard;
using Orchard.DisplayManagement.Implementation;

namespace NGM.OpenAuthentication {
    public interface IAuthenticationShapeHelper : IDependency {
        bool IsLogOn(ShapeCreatedContext context);
        bool IsCreate(ShapeCreatedContext context);
    }
}