//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Web;
//using DotNetOpenAuth.OAuth2;
//using NGM.OpenAuthentication.Core.OAuth.DotNetOpenAuth.ApplicationBlock.Facebook;
//using NGM.OpenAuthentication.Models;
//using Orchard;
//using Orchard.ContentManagement;

//namespace NGM.OpenAuthentication.Core.OAuth {
//    public class FacebookAuthorizer : IOAuthAuthorizer {
//        private readonly IOrchardServices _orchardServices;

//        private static readonly FacebookClient client = new FacebookClient();

//        public FacebookAuthorizer(IOrchardServices orchardServices) {
//            _orchardServices = orchardServices;

//            client.ClientIdentifier = ClientKeyIdentifier;
//            client.ClientSecret = ClientSecret;
//        }

//        public string ClientKeyIdentifier {
//            get { return _orchardServices.WorkContext.CurrentSite.As<OpenAuthenticationSettingsPart>().Record.FacebookClientIdentifier; }
//        }

//        public string ClientSecret {
//            get { return _orchardServices.WorkContext.CurrentSite.As<OpenAuthenticationSettingsPart>().Record.FacebookClientSecret; }
//        }

//        public bool IsConsumerConfigured {
//            get {
//                return !string.IsNullOrEmpty(ClientKeyIdentifier) && !string.IsNullOrEmpty(ClientSecret);
//            }
//        }

//        public RegisterModel Authorize() {
//            IAuthorizationState authorization = client.ProcessUserAuthorization();
//            if (authorization == null) {
//                // Kick off authorization request
//                client.RequestUserAuthorization();
//            } else {
//                var request = WebRequest.Create("https://graph.facebook.com/me?access_token=" + Uri.EscapeDataString(authorization.AccessToken));
//                using (var response = request.GetResponse()) {
//                    using (var responseStream = response.GetResponseStream()) {
//                        var graph = FacebookGraph.Deserialize(responseStream);
//                        return new RegisterModel {
//                            UserName = HttpUtility.HtmlEncode(graph.FirstName) + HttpUtility.HtmlEncode(graph.LastName), 
//                            ExternalIdentifier = HttpUtility.HtmlEncode(graph.Id), 
//                            FriendlyIdentifier = HttpUtility.HtmlEncode(graph.Id)
//                        };
//                    }
//                }
//            }
//            return null;
//        }

//        public string Provider {
//            get { return "Facebook"; }
//        }
//    }
//}