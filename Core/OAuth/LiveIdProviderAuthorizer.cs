using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NGM.OpenAuthentication.Extensions;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using Orchard;

namespace NGM.OpenAuthentication.Core.OAuth {
    public class LiveIdProviderAuthorizer : IOAuthProviderAuthorizer {
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthorizer _authorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        public LiveIdProviderAuthorizer(IOrchardServices orchardServices,
            IAuthorizer authorizer,
            IOpenAuthenticationService openAuthenticationService) {
            _orchardServices = orchardServices;
            _authorizer = authorizer;
            _openAuthenticationService = openAuthenticationService;
        }

        public AuthorizeState Authorize(string returnUrl) {
            var request = _orchardServices.WorkContext.HttpContext.Request;
            var response = _orchardServices.WorkContext.HttpContext.Response;

            OAuthAuthenticationParameters parameters = new OAuthAuthenticationParameters(Provider);
            if (HasVerificationToken()) {
                try {
                    // Construct a request for an access token.
                    WebRequest tokenRequest = WebRequest.Create("https://consent.live.com/AccessToken.aspx");
                    tokenRequest.ContentType = "application/x-www-form-urlencoded";
                    tokenRequest.Method = "POST";

                    using (StreamWriter writer = new StreamWriter(tokenRequest.GetRequestStream())) {
                        writer.Write(string.Format(
                            "wrap_client_id={0}&wrap_client_secret={1}&wrap_callback={2}&wrap_verification_code={3}",
                            HttpUtility.UrlEncode(this.ClientKeyIdentifier),
                            HttpUtility.UrlEncode(this.ClientSecret),
                            HttpUtility.UrlEncode(LiveIdParametersHelpers.LiveIdCallback()),
                            HttpUtility.UrlEncode(VerificationToken())));
                    }
                    // Send the request and get the response.
                    WebResponse tokenResponse = tokenRequest.GetResponse();

                    // Read the first line of the response body.
                    string tokenResponseText = new StreamReader(tokenResponse.GetResponseStream()).ReadLine();

                    // Parse the response body as being in the format of 'x-www-form-urlencoded'.
                    NameValueCollection tokenResponseData = HttpUtility.ParseQueryString(tokenResponseText);

                    // Store data in cookies where the JS API will pick them up.
                    response.Cookies["wl_clientId"].Value = this.ClientKeyIdentifier;
                    response.Cookies["wl_clientState"].Value = request.QueryString["wrap_client_state"];
                    response.Cookies["wl_scope"].Value = request.QueryString["exp"];
                    response.Cookies["wl_accessToken"].Value = tokenResponseData["wrap_access_token"];
                    response.Cookies["wl_accessTokenExpireTime"].Value = tokenResponseData["wrap_access_token_expires_in"];
                    response.Cookies["wl_cid"].Value = tokenResponseData["cid"] ?? tokenResponseData["uid"];
                    response.Cookies["wl_complete"].Value = "done";

                    parameters = new OAuthAuthenticationParameters(this.Provider) {
                        ExternalIdentifier = tokenResponseData["cid"] ?? tokenResponseData["uid"],
                        OAuthToken = tokenResponseData["wrap_access_token"],
                        OAuthAccessToken = tokenResponseData["skey"]
                    };
                }
                catch (WebException webException) {
                    string responseBody = null;
                    if (webException.Response != null) {
                        using (var sr = new StreamReader(webException.Response.GetResponseStream(), Encoding.UTF8)) {
                            responseBody = sr.ReadToEnd();
                        }
                    }
                    throw new Exception(String.Format(
                        "Failure occurred contacting consent service: Response=\r\n\r\n----\r\n{0}\r\n----\r\n\r\n", responseBody), webException);
                }
                catch (Exception innerException) {
                    throw new Exception("Failed to get access token. Ensure that the verifier token provided is valid.", innerException);
                }
            }

            var status = _authorizer.Authorize(parameters);

            var tempReturnUrl = _orchardServices.WorkContext.HttpContext.Request.QueryString["?ReturnUrl"];
            if (!string.IsNullOrEmpty(tempReturnUrl) && string.IsNullOrEmpty(returnUrl)) {
                returnUrl = tempReturnUrl;
            }

            if (status != OpenAuthenticationStatus.Authenticated)
                status = OpenAuthenticationStatus.AssociateOnLogon;

            return new AuthorizeState(returnUrl, status) {
                Error = _authorizer.Error,
                RegisterModel = new RegisterModel(parameters)
            };
        }

        private bool HasVerificationToken() {
             return VerificationToken() != null;
        }

        private string VerificationToken() {
            return _orchardServices.WorkContext.HttpContext.Request.QueryString["wrap_verification_code"];
        }

        public string ClientKeyIdentifier {
            get { return _openAuthenticationService.GetSettings().Record.LiveIdClientIdentifier; }
        }

        public string ClientSecret {
            get { return _openAuthenticationService.GetSettings().Record.LiveIdClientSecret; }
        }

        public bool IsConsumerConfigured {
            get {
                return !string.IsNullOrEmpty(ClientKeyIdentifier) && !string.IsNullOrEmpty(ClientSecret);
            }
        }

        public OAuthProvider Provider {
            get { return OAuthProvider.LiveId; }
        }
    }
}