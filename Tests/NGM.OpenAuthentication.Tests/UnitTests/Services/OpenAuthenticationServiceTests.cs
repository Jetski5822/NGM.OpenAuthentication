using Moq;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NUnit.Framework;
using Orchard.Data;

namespace NGM.OpenAuthentication.Tests.UnitTests.Services {
    [TestFixture]
    public class OpenAuthenticationServiceTests {
        [Test]
        public void should_return_null_when_no_user_exists_for_identifier() {
            var mockRepository = new Mock<IRepository<OpenAuthenticationPartRecord>>();

            var openAuthenticationService = new OpenAuthenticationService(null, mockRepository.Object, null, null);
            var user = openAuthenticationService.GetUser("test");

            Assert.That(user, Is.Null);
        }

        [Test]
        [Ignore ("Cant seem to mock the stuff i want to call. I will do this test with a functional test")]
        public void should_return_user_when_identifier_exists_against_user() {
            //var identifier = "foo";
            //var openAuthenticationPartRecord = new OpenAuthenticationPartRecord {Id = 123, Identifier = identifier};

            //var mockRepository = new Mock<IRepository<OpenAuthenticationPartRecord>>();
            //mockRepository.Setup(ctx => ctx.Get(It.IsAny<Expression<Func<OpenAuthenticationPartRecord, bool>>>())).Returns(openAuthenticationPartRecord);

            //var mockUser = new Mock<IUser>();

            //var mockContentItem = new Mock<ContentItem>();
            //mockContentItem.Setup(ctx => ctx.Get(It.IsAny<Type>())).Returns(mockUser.Object);

            //var mockContentManager = new Mock<IContentManager>();
            //mockContentManager.Setup(ctx => ctx.Get(openAuthenticationPartRecord.Id)).Returns(mockContentItem.Object);

            //var openAuthenticationService = new OpenAuthenticationService(mockContentManager.Object, mockRepository.Object);
            //var user = openAuthenticationService.GetUser(identifier);

            //Assert.That(user, Is.Not.Null);
        }

    }
}
