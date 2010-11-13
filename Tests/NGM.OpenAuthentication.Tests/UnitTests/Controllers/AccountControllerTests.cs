using System;
using System.Net;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using Moq;
using NGM.OpenAuth.Tests.Fakes;
using NGM.OpenAuthentication.Controllers;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using NUnit.Framework;
using Orchard.Security;

namespace NGM.OpenAuthentication.Tests.UnitTests.Controllers {
    [TestFixture]
    public class AccountControllerTests {
        private const string OpenAuthUrlForGoogle = "https://www.google.com/accounts/o8/id";

        [Test]
        public void should_return_logon_view_when_no_response_returned_from_relyparty() {
            var mockRelyingService = new Mock<IOpenIdRelyingPartyService>();
            mockRelyingService.Setup(ctx => ctx.HasResponse).Returns(false);

            var accountController = new AccountController(mockRelyingService.Object, null, null);
            accountController.ControllerContext = MockControllerContext(accountController);
            var viewResult = (ViewResult)accountController.LogOn(string.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("LogOn"));
            
            mockRelyingService.VerifyAll();
        }

        //[Test]
        //public void should_authenticate_user_with_credential_on_callback() {
        //    var mockRelyingService = new Mock<IOpenIdRelyingPartyService>();
        //    mockRelyingService.Setup(ctx => ctx.HasResponse).Returns(true);

        //    var mockAuthenticationResponse = new Mock<IAuthenticationResponse>();
        //    mockAuthenticationResponse.Setup(ctx => ctx.Status).Returns(AuthenticationStatus.Authenticated);
        //    Identifier identifier = Identifier.Parse("http://foo.google.com");

        //    mockAuthenticationResponse.Setup(ctx => ctx.ClaimedIdentifier).Returns(identifier);

        //    mockRelyingService.Setup(ctx => ctx.Response).Returns(mockAuthenticationResponse.Object);

        //    var mockAuthenticationService = new Mock<IAuthenticationService>();
        //    mockAuthenticationService.Setup(o => o.SignIn(It.IsAny<IUser>(), false));

        //    var mockOpenAuthenticationService = new Mock<IOpenAuthenticationService>();
        //    mockOpenAuthenticationService.Setup(ctx => ctx.CreateUser(OpenAuthUrlForGoogle));

        //    var accountController = new AccountController(mockRelyingService.Object, mockAuthenticationService.Object, mockOpenAuthenticationService.Object);
        //    var redirectResult = (RedirectResult) accountController.LogOn(string.Empty);

        //    Assert.That(redirectResult.Url, Is.EqualTo("~/"));

        //    mockRelyingService.VerifyAll();
        //    mockAuthenticationResponse.VerifyAll();
        //    mockAuthenticationService.VerifyAll();
        //}

        [Test]
        public void should_return_error_message_when_authentication_was_canceled() {
            var mockRelyingService = new Mock<IOpenIdRelyingPartyService>();
            mockRelyingService.Setup(ctx => ctx.HasResponse).Returns(true);

            var mockAuthenticationResponse = new Mock<IAuthenticationResponse>();
            mockAuthenticationResponse.Setup(ctx => ctx.Status).Returns(AuthenticationStatus.Canceled);

            mockRelyingService.Setup(ctx => ctx.Response).Returns(mockAuthenticationResponse.Object);

            var accountController = new AccountController(mockRelyingService.Object, null, null);
            var viewResult = (ViewResult)accountController.LogOn(string.Empty);

            Assert.That(viewResult.ViewData.ModelState.IsValid, Is.False);
            Assert.That(viewResult.ViewData.ModelState.ContainsKey("InvalidProvider"), Is.True);

            mockRelyingService.VerifyAll();
            mockAuthenticationResponse.VerifyAll();
        }

        [Test]
        public void should_return_error_message_when_authentication_failed() {
            var mockRelyingService = new Mock<IOpenIdRelyingPartyService>();
            mockRelyingService.Setup(ctx => ctx.HasResponse).Returns(true);

            var mockAuthenticationResponse = new Mock<IAuthenticationResponse>();
            mockAuthenticationResponse.Setup(ctx => ctx.Status).Returns(AuthenticationStatus.Failed);
            var exception = new Exception("Error Message");
            mockAuthenticationResponse.Setup(ctx => ctx.Exception).Returns(exception);

            mockRelyingService.Setup(ctx => ctx.Response).Returns(mockAuthenticationResponse.Object);

            var accountController = new AccountController(mockRelyingService.Object, null, null);
            var viewResult = (ViewResult)accountController.LogOn(string.Empty);

            Assert.That(viewResult.ViewData.ModelState.IsValid, Is.False);
            Assert.That(viewResult.ViewData.ModelState.ContainsKey("UnknownError"), Is.True);

            ModelState modelState;
            viewResult.ViewData.ModelState.TryGetValue("UnknownError", out modelState);
            Assert.That(modelState.Errors.FirstOrDefault().ErrorMessage, Is.EqualTo(exception.Message));

            mockRelyingService.VerifyAll();
            mockAuthenticationResponse.VerifyAll();
        }

        [Test]
        public void should_assign_identifier_to_logged_in_account() {

            var mockRelyingService = new Mock<IOpenIdRelyingPartyService>();
            mockRelyingService.Setup(ctx => ctx.HasResponse).Returns(true);

            var mockAuthenticationResponse = new Mock<IAuthenticationResponse>();
            mockAuthenticationResponse.Setup(ctx => ctx.Status).Returns(AuthenticationStatus.Authenticated);
            Identifier identifier = Identifier.Parse("http://foo.google.com");

            mockAuthenticationResponse.Setup(ctx => ctx.ClaimedIdentifier).Returns(identifier);

            mockRelyingService.Setup(ctx => ctx.Response).Returns(mockAuthenticationResponse.Object);

            var mockUser = new Mock<IUser>();

            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService.Setup(o => o.GetAuthenticatedUser()).Returns(mockUser.Object);

            var mockOpenAuthenticationService = new Mock<IOpenAuthenticationService>();
            mockOpenAuthenticationService.Setup(ctx => ctx.AssociateOpenIdWithUser(mockUser.Object, identifier.ToString()));

            var accountController = new AccountController(mockRelyingService.Object, mockAuthenticationService.Object, mockOpenAuthenticationService.Object);
            var actionResult = accountController.LogOn(string.Empty);

            mockAuthenticationResponse.VerifyAll();
            mockAuthenticationService.VerifyAll();
            mockRelyingService.VerifyAll();
            mockOpenAuthenticationService.VerifyAll();
        }

        [Test]
        public void should_not_assign_identifier_to_an_account_when_identifier_exists_on_another_account_upon_return_from_openid_service() {
            var mockRelyingService = new Mock<IOpenIdRelyingPartyService>();
            mockRelyingService.Setup(ctx => ctx.HasResponse).Returns(true);

            var mockAuthenticationResponse = new Mock<IAuthenticationResponse>();
            mockAuthenticationResponse.Setup(ctx => ctx.Status).Returns(AuthenticationStatus.Authenticated);
            Identifier identifier = Identifier.Parse("http://foo.google.com");

            mockRelyingService.Setup(ctx => ctx.Response).Returns(mockAuthenticationResponse.Object);

            var mockAuthenticationService = new Mock<IAuthenticationService>();

            var mockOpenAuthenticationService = new Mock<IOpenAuthenticationService>();
            mockOpenAuthenticationService.Setup(ctx => ctx.IsAccountExists(It.IsAny<string>())).Returns(true);

            var accountController = new AccountController(mockRelyingService.Object, mockAuthenticationService.Object, mockOpenAuthenticationService.Object);
            var viewResult = (ViewResult)accountController.LogOn(string.Empty);

            Assert.That(viewResult.ViewData.ModelState.IsValid, Is.False);
            Assert.That(viewResult.ViewData.ModelState.ContainsKey("IdentifierAssigned"), Is.True);

            mockAuthenticationResponse.VerifyAll();
            mockRelyingService.VerifyAll();
            mockOpenAuthenticationService.VerifyAll();
        }

        [Test]
        public void should_redirect_to_register_route_if_user_does_not_exist() {
            var mockRelyingService = new Mock<IOpenIdRelyingPartyService>();
            mockRelyingService.Setup(ctx => ctx.HasResponse).Returns(true);

            var mockAuthenticationResponse = new Mock<IAuthenticationResponse>();
            mockAuthenticationResponse.Setup(ctx => ctx.Status).Returns(AuthenticationStatus.Authenticated);
            Identifier identifier = Identifier.Parse("http://foo.google.com");

            mockAuthenticationResponse.Setup(ctx => ctx.ClaimedIdentifier).Returns(identifier);

            mockRelyingService.Setup(ctx => ctx.Response).Returns(mockAuthenticationResponse.Object);

            var mockAuthenticationService = new Mock<IAuthenticationService>();

            var mockOpenAuthenticationService = new Mock<IOpenAuthenticationService>();

            var accountController = new AccountController(mockRelyingService.Object, mockAuthenticationService.Object, mockOpenAuthenticationService.Object);
            var redirectToRouteResult = (RedirectToRouteResult)accountController.LogOn(string.Empty);

            Assert.That(redirectToRouteResult.RouteValues["area"], Is.EqualTo("NGM.OpenAuthentication"));
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo("Register"));
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.EqualTo("Account"));

            Assert.That(accountController.TempData.ContainsKey("RegisterModel"), Is.True);

            mockAuthenticationService.Verify(ctx => ctx.SignIn(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never());
            mockOpenAuthenticationService.Verify(ctx => ctx.AssociateOpenIdWithUser(It.IsAny<IUser>(), It.IsAny<string>()), Times.Never());

            mockAuthenticationResponse.VerifyAll();
            mockAuthenticationService.VerifyAll();
            mockRelyingService.VerifyAll();
            mockOpenAuthenticationService.VerifyAll();
        }

        [Test]
        public void should_redirect_to_logon_view_if_no_viewmodel_present_on_register_page() {
            var accountController = new AccountController(null,null,null);
            var redirectToRouteResult = (RedirectToRouteResult)accountController.Register(null);

            Assert.That(redirectToRouteResult.RouteValues["area"], Is.EqualTo("NGM.OpenAuthentication"));
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo("LogOn"));
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.EqualTo("Account"));
        }

        [Test]
        public void should_redirect_to_logon_view_if_viewmodel_has_null_model_present_on_register_page() {
            var accountController = new AccountController(null, null, null);

            var viewModel = new RegisterViewModel();
            var redirectToRouteResult = (RedirectToRouteResult)accountController.Register(viewModel);

            Assert.That(redirectToRouteResult.RouteValues["area"], Is.EqualTo("NGM.OpenAuthentication"));
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo("LogOn"));
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.EqualTo("Account"));
        }

        [Test]
        public void should_use_passedin_model_from_logon_if_avalible() {
            var accountController = new AccountController(null, null, null);
            var model = new RegisterModel("Test");
            accountController.TempData.Add("RegisterModel", model);

            var viewResult = (ViewResult)accountController.Register(null);
            Assert.That(viewResult.ViewName, Is.EqualTo("Register"));
            Assert.That(viewResult.ViewData.Model, Is.TypeOf(typeof(RegisterViewModel)));
            var viewModel = viewResult.ViewData.Model as RegisterViewModel;
            Assert.That(viewModel.Model, Is.EqualTo(model));
        }

        [Test]
        public void should_not_recreate_registration_view_model_if_view_model_exists() {
            var accountController = new AccountController(null, null, null);
            var viewModel = new RegisterViewModel(new RegisterModel("test"));
            var viewResult = (ViewResult)accountController.Register(viewModel);

            Assert.That(viewResult.ViewData.Model, Is.EqualTo(viewModel));
        }

        


        /* POST */

        [Test]
        public void should_redirect_to_external_openid_logon_for_valid_openid_identifier() {
            var viewModel = new LogOnViewModel { OpenIdIdentifier = OpenAuthUrlForGoogle };

            var mockRelyingService = new Mock<IOpenIdRelyingPartyService>();

            var mockAuthenticationRequest = new Mock<IAuthenticationRequest>();

            var request = WebRequest.Create(OpenAuthUrlForGoogle);

            var fakeOutgoingWebResponse = new FakeOutgoingWebResponse(request.GetResponse() as HttpWebResponse);
            mockAuthenticationRequest.Setup(ctx => ctx.RedirectingResponse).Returns(fakeOutgoingWebResponse);

            var mockOpenAuthenticationService = new Mock<IOpenAuthenticationService>();

            mockRelyingService.Setup(ctx => ctx.CreateRequest(It.IsAny<OpenIdIdentifier>())).Returns(mockAuthenticationRequest.Object);

            var accountController = new AccountController(mockRelyingService.Object, null, mockOpenAuthenticationService.Object);
            var actionResult = accountController._LogOn(viewModel);

            Assert.That(actionResult, Is.Not.Null);

            mockAuthenticationRequest.VerifyAll();
            mockRelyingService.VerifyAll();
        }

        //[Test]
        //public void should_create_user_when_saving_valid_details() {
        //    var model = new RegisterModel(OpenAuthUrlForGoogle);
        //    var viewModel = new RegisterViewModel(model);
            
        //    var accountController = new AccountController(null, null, null);
        //    var actionResult = accountController._Register(viewModel);
        //}

        [Test]
        public void should_not_assign_identifier_to_an_account_when_identifier_exists_on_another_account() {
            Identifier identifier = Identifier.Parse("http://foo.google.com");

            var mockOpenAuthenticationService = new Mock<IOpenAuthenticationService>();
            mockOpenAuthenticationService.Setup(ctx => ctx.IsAccountExists(identifier.ToString())).Returns(true);

            var accountController = new AccountController(null, null, mockOpenAuthenticationService.Object);
            var viewResult = (ViewResult)accountController._LogOn(new LogOnViewModel { OpenIdIdentifier = identifier.ToString() });

            Assert.That(viewResult.ViewData.ModelState.IsValid, Is.False);
            Assert.That(viewResult.ViewData.ModelState.ContainsKey("IdentifierAssigned"), Is.True);

            mockOpenAuthenticationService.VerifyAll();
        }



        public ControllerContext MockControllerContext(ControllerBase controllerBase) {
            var mockHttpContext = new Mock<HttpContextBase>();
            var mockIPrinciple = new Mock<IPrincipal>();
            mockIPrinciple.SetupAllProperties();
            var mockIIdentity = new Mock<IIdentity>();
            mockIIdentity.SetupAllProperties();
            mockIPrinciple.Setup(ctx => ctx.Identity).Returns(mockIIdentity.Object);
            mockHttpContext.Setup(ctx => ctx.User).Returns(mockIPrinciple.Object);
            return new ControllerContext(
                mockHttpContext.Object,
                new RouteData(
                    new Route("foobar", new MvcRouteHandler()),
                    new MvcRouteHandler()),
                controllerBase);
        }
    }
}
