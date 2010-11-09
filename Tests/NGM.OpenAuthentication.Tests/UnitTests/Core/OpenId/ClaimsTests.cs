using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using Moq;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Models;
using NUnit.Framework;

namespace NGM.OpenAuthentication.Tests.UnitTests.Core.OpenId {
    [TestFixture]
    public class ClaimsTests {
        [Test]
        public void when_required_values_are_set_in_configuration_then_they_should_return_required_in_request() {
            var mockSettings = new Mock<OpenAuthenticationSettingsRecord>();
            mockSettings.SetupGet(o => o.Birthdate).Returns(true);
            mockSettings.SetupGet(o => o.Country).Returns(true);
            mockSettings.SetupGet(o => o.Email).Returns(true);
            mockSettings.SetupGet(o => o.FullName).Returns(true);
            mockSettings.SetupGet(o => o.Gender).Returns(true);
            mockSettings.SetupGet(o => o.Language).Returns(true);
            mockSettings.SetupGet(o => o.Nickname).Returns(true);
            mockSettings.SetupGet(o => o.PostalCode).Returns(true);
            mockSettings.SetupGet(o => o.TimeZone).Returns(true);

            var claimsRequest = Claims.CreateRequest(mockSettings.Object);

            Assert.That(claimsRequest.BirthDate, Is.EqualTo(DemandLevel.Require));
            Assert.That(claimsRequest.Country, Is.EqualTo(DemandLevel.Require));
            Assert.That(claimsRequest.Email, Is.EqualTo(DemandLevel.Require));
            Assert.That(claimsRequest.FullName, Is.EqualTo(DemandLevel.Require));
            Assert.That(claimsRequest.Gender, Is.EqualTo(DemandLevel.Require));
            Assert.That(claimsRequest.Language, Is.EqualTo(DemandLevel.Require));
            Assert.That(claimsRequest.Nickname, Is.EqualTo(DemandLevel.Require));
            Assert.That(claimsRequest.PostalCode, Is.EqualTo(DemandLevel.Require));
            Assert.That(claimsRequest.TimeZone, Is.EqualTo(DemandLevel.Require));
        }

        [Test]
        public void should_return_false_for_requested_attribute_if_not_set_in_confiruration() {
            var mockSettings = new Mock<OpenAuthenticationSettingsRecord>();
            mockSettings.SetupGet(o => o.Birthdate).Returns(true);
            mockSettings.SetupGet(o => o.Country).Returns(true);
            mockSettings.SetupGet(o => o.Email).Returns(false);
            mockSettings.SetupGet(o => o.FullName).Returns(true);
            mockSettings.SetupGet(o => o.Gender).Returns(false);
            mockSettings.SetupGet(o => o.Language).Returns(true);
            mockSettings.SetupGet(o => o.Nickname).Returns(true);
            mockSettings.SetupGet(o => o.PostalCode).Returns(false);
            mockSettings.SetupGet(o => o.TimeZone).Returns(true);

            var claimsRequest = Claims.CreateRequest(mockSettings.Object);

            Assert.That(claimsRequest.BirthDate, Is.EqualTo(DemandLevel.Require));
            Assert.That(claimsRequest.Country, Is.EqualTo(DemandLevel.Require));
            Assert.That(claimsRequest.Email, Is.EqualTo(DemandLevel.NoRequest));
            Assert.That(claimsRequest.FullName, Is.EqualTo(DemandLevel.Require));
            Assert.That(claimsRequest.Gender, Is.EqualTo(DemandLevel.NoRequest));
            Assert.That(claimsRequest.Language, Is.EqualTo(DemandLevel.Require));
            Assert.That(claimsRequest.Nickname, Is.EqualTo(DemandLevel.Require));
            Assert.That(claimsRequest.PostalCode, Is.EqualTo(DemandLevel.NoRequest));
            Assert.That(claimsRequest.TimeZone, Is.EqualTo(DemandLevel.Require));
        }
    }
}
