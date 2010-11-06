//using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
//using NGM.OpenAuthentication.Models;
//using Orchard;

//namespace NGM.OpenAuthentication.Core.OpenId.Mappers {
//    public class ClaimsResponseToExtendedUserPropertiesContextMapper : IMapper<ClaimsResponse, ExtendedProperties> {
//        public ExtendedProperties Map(ClaimsResponse source) {
//            var context = new ExtendedProperties();
//            if (source.Gender.HasValue)
//                context.Gender = source.Gender.Value.ToString();

//            return context;
//        }
//    }
//}