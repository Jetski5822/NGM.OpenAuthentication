using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NGM.OpenAuthentication.Core.OAuth {
    public interface IOAuthSettings {
        string ClientKeyIdentifier { get; }
        string ClientSecret { get; }
    }
}