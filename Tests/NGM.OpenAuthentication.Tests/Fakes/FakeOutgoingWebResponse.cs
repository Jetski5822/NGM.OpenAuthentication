using System.Net;
using DotNetOpenAuth.Messaging;

namespace NGM.OpenAuth.Tests.Fakes {
    public class FakeOutgoingWebResponse : OutgoingWebResponse {
        public FakeOutgoingWebResponse(HttpWebResponse httpWebResponse) :base (httpWebResponse, 0) {
            
        }
    }
}