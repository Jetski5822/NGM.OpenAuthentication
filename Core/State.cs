using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NGM.OpenAuthentication.Core {
    public class State {
        public static OpenAuthenticationParameters Parameters {
            get { return HttpContext.Current.Session["parameters"] as OpenAuthenticationParameters; }
            set {
                if (value == null) {
                    HttpContext.Current.Session.Remove("parameters");
                } else {
                    HttpContext.Current.Session["parameters"] = value;
                }
            }
        }

        public static void Clear() {
            Parameters = null;
        }
    }
}