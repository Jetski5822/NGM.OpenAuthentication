using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NGM.OpenAuthentication.Controllers;
using NUnit.Framework;

namespace NGM.OpenAuthentication.Tests.UnitTests.Controllers {
    [TestFixture]
    public class CardSpaceAccountControllerTests {
        [Test, Ignore("Not implemented yet")]
        public void ShouldLoadToken() {
            CardSpaceAccountController controller = new CardSpaceAccountController();
            FormCollection collection = new FormCollection();
            string xmltoken = "<saml:Assertion MajorVersion=\"1\" MinorVersion=\"1\" AssertionID=\"SamlSecurityToken-6964e4f6-7a43-427a-b696-3b14b069c9bc\" Issuer=\"http://schemas.xmlsoap.org/ws/2005/05/identity/issuer/self\" IssueInstant=\"2011-02-16T09:26:41.378Z\" xmlns:saml=\"urn:oasis:names:tc:SAML:1.0:assertion\"><saml:Conditions NotBefore=\"2011-02-16T09:26:41.378Z\" NotOnOrAfter=\"2011-02-16T10:26:41.378Z\"><saml:AudienceRestrictionCondition><saml:Audience>http://localhost:30320/OrchardLocal/Users/Account/LogOn?ReturnUrl=%2FOrchardLocal%2F</saml:Audience></saml:AudienceRestrictionCondition></saml:Conditions><saml:AttributeStatement><saml:Subject><saml:SubjectConfirmation><saml:ConfirmationMethod>urn:oasis:names:tc:SAML:1.0:cm:bearer</saml:ConfirmationMethod></saml:SubjectConfirmation></saml:Subject><saml:Attribute AttributeName=\"givenname\" AttributeNamespace=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims\"><saml:AttributeValue>Nicholas</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=\"surname\" AttributeNamespace=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims\"><saml:AttributeValue>Mayne</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=\"emailaddress\" AttributeNamespace=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims\"><saml:AttributeValue>Jetski5822@hotmail.com</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=\"privatepersonalidentifier\" AttributeNamespace=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims\"><saml:AttributeValue>NQOBPdrZKGw8L1iOQtrY2tHimwASpBxcBnJEa0pYmfY=</saml:AttributeValue></saml:Attribute></saml:AttributeStatement><Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\"><SignedInfo><CanonicalizationMethod Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"></CanonicalizationMethod><SignatureMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#rsa-sha1\"></SignatureMethod><Reference URI=\"#SamlSecurityToken-6964e4f6-7a43-427a-b696-3b14b069c9bc\"><Transforms><Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\"></Transform><Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"></Transform></Transforms><DigestMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#sha1\"></DigestMethod><DigestValue>5lbzCaD3cjZIXyGR/FrIQcWC61c=</DigestValue></Reference></SignedInfo><SignatureValue>ROjwpbcIdU6iOfLfclmQoxWuKgJ484Qp/d1/N6fErim4UdPxvDiX6MTMzQf2SNNXQFtlwccqynRQJzJhaTv8vfGW1/qTHXkVDkCDyhiTjQyIP8Uq6t30CLQt+Qh017UOusagrcltFSgw2aM+EvJXEFUCLv4tE33CVpbEMFdJ2M+xPF7yjXfLtTRCU5amlsMjVbd3yUBJXFiyB/vmwJkhsEPE3awb1kVowrUMWszSaWLWTtblCGFzyWTVYHReekzLxPmTOHGW7+bKNzG7e/yztRrpWmQw7PlKmD3q77DJjZghl94uKvuVFwcJ+M5+xaurCOrw38eX/38O52i6+EZHmQ==</SignatureValue><KeyInfo><KeyValue><RSAKeyValue><Modulus>0sroDOFzEjmB4rso3FBF+TxA/7Duy5+/KZNqBiism1Cq6VVUrTBnDgRMyr7SIS5+WRwvRosiMWwT08U6DvabWdgiYCl1qCfIkW62KO8Ck6IV3HaJQYO6ga9pA7/xhznnO6oa7WWI0io7oFPw+qwZGQ65x8f9kttpvoyTryDQvsoKDP51wmzxNX1/KHftYzdP9fh7APg9n5NDpFs5CV+2w6XnrOYFNPgWRfSwyyb/SKZZNwuMU9sYVVZBBCH0721QcJ/8prlZxizBcLRyVwI6UNohup5yNIIaYMFK4lBN54FN8qAot3SH78pJqqrYnjhjee+/s/6HYLXtTtAyE0zBUw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue></KeyValue></KeyInfo></Signature></saml:Assertion>";
            collection.Add("xmlToken", xmltoken);
            controller.ControllerContext = MockControllerContext(controller, collection);
            controller.ValueProvider = collection.ToValueProvider();
            controller.LogOn("blah");
        }

        public ControllerContext MockControllerContext(ControllerBase controllerBase, NameValueCollection collection) {
            var mockHttpContext = new Mock<HttpContextBase>();
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(o => o.Params).Returns(collection);
            mockHttpContext.Setup(o => o.Request).Returns(mockRequest.Object);
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