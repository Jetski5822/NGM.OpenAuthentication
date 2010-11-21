using System.Net;
using DotNetOpenAuth.Messaging;

namespace NGM.OpenAuthentication.Tests.Fakes {
    public class FakeOutgoingWebResponse : OutgoingWebResponse {
        public FakeOutgoingWebResponse(HttpWebResponse httpWebResponse) :base (httpWebResponse, 0) {
            
        }
    }
}