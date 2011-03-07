using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using NGM.OpenAuthentication.Extensions;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.Security;
using WindowsLive;

namespace NGM.OpenAuthentication.Core.OAuth {
    public class LiveIdProviderAuthorizer : IOAuthProviderAuthorizer {
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthorizer _authorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IAuthenticationService _authenticationService;

        public const string LoginCookie = "webauthtoken";

        static DateTime ExpireCookie = DateTime.Now.AddYears(-10);
        static DateTime PersistCookie = DateTime.Now.AddYears(10);

        private WindowsLiveLogin _login;

        public LiveIdProviderAuthorizer(IOrchardServices orchardServices,
            IAuthorizer authorizer,
            IOpenAuthenticationService openAuthenticationService,
            IAuthenticationService authenticationService) {
            _orchardServices = orchardServices;
            _authorizer = authorizer;
            _openAuthenticationService = openAuthenticationService;
            _authenticationService = authenticationService;

            _login = new WindowsLiveLogin(ClientKeyIdentifier, ClientSecret);
        }

        public AuthorizeState Authorize(string returnUrl) {
            var request = _orchardServices.WorkContext.HttpContext.Request;
            var response = _orchardServices.WorkContext.HttpContext.Response;
            var action = request["action"];

            if (action == "login") {
                CompleteAuthorization(returnUrl, request, response);
            }
            if (action == "logout") {
                ProcessLogOut(returnUrl, request, response);
            }
            if (action == "clearcookie") {
                ProcessClearCookie(returnUrl, response);
            }

            return new AuthorizeState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating) {
                Error = new KeyValuePair<string, string>("error", "Unknown Action")
            };
        }

        private void ProcessClearCookie(string returnUrl, HttpResponseBase response) {
            // Unsure I need this
            HttpCookie loginCookie = new HttpCookie(LoginCookie);
            loginCookie.Expires = ExpireCookie;
            response.Cookies.Add(loginCookie);

            string type;
            byte[] content;
            _login.GetClearCookieResponse(out type, out content);
            response.ContentType = type;
            response.OutputStream.Write(content, 0, content.Length);

            response.End();
        }

        private void ProcessLogOut(string returnUrl, HttpRequestBase request, HttpResponseBase response) {
            _authenticationService.SignOut();
            // Expire cookie.
            HttpCookie loginCookie = new HttpCookie(LoginCookie);
            loginCookie.Expires = ExpireCookie;
            response.Cookies.Add(loginCookie);
            var requestContext = new UrlHelper(request.RequestContext);
            response.Redirect(requestContext.LogOff(returnUrl));
            response.End();
        }

        private void CompleteAuthorization(string returnUrl, HttpRequestBase request, HttpResponseBase response) {
            WindowsLiveLogin.User user = _login.ProcessLogin(request.Form);

            if (user != null) {
                var parameters = new OAuthAuthenticationParameters(this.Provider) {
                    ExternalIdentifier = user.Token,
                    ExternalDisplayIdentifier = user.Id,
                    OAuthToken = user.Token
                };

                _authorizer.Authorize(parameters);

                HttpCookie loginCookie = new HttpCookie(LoginCookie);
                loginCookie.Value = user.Token;
                loginCookie.Values.Add("UserId", user.Id);
                response.Cookies.Add(loginCookie);
                var requestContext = new UrlHelper(request.RequestContext);
                response.Redirect(requestContext.LogOn(returnUrl));
                response.End();
            }
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