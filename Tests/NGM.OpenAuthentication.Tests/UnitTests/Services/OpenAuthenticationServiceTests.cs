using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NUnit.Framework;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Security;

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

        //[Test]
        //public void should_return_all_identifiers_for_specified_user() {
        //    var mockRepository = new Mock<IRepository<OpenAuthenticationPartRecord>>();
        //    var record1 = new OpenAuthenticationPartRecord { Id = 1, Identifier = "Foo"};
        //    var record2 = new OpenAuthenticationPartRecord { Id = 1, Identifier = "bar" };
        //    mockRepository.Setup(o => o.Fetch(It.IsAny<Expression<Func<OpenAuthenticationPartRecord, bool>>>())).Returns(new [] {record1, record2});
        //    var openAuthenticationService = new OpenAuthenticationService(null, mockRepository.Object, null, null);

        //    var mockUser = new Mock<IUser>();
        //    mockUser.SetupGet(o => o.Id).Returns(1);

        //    var identities = openAuthenticationService.GetIdentifiersFor(mockUser.Object).List();

        //    Assert.That(identities.Count(), Is.EqualTo(2));
        //}

        [Test]
        public void should_return_all_identifiers_for_specified_user() {
            var mockContentManager = new Mock<IContentManager>();
            var mockContentQuerySpecialized = new Mock<IContentQuery<OpenAuthenticationPart, OpenAuthenticationPartRecord>>();

            mockContentQuerySpecialized.Setup(o => o.Where(It.IsAny<Expression<Func<OpenAuthenticationPartRecord, bool>>>())).Returns(mockContentQuerySpecialized.Object);

            var mockContentQuery = new Mock<IContentQuery<ContentItem>>();
            var mockContentQueryOpenAuthenticationPart = new Mock<IContentQuery<OpenAuthenticationPart>>();
            mockContentQueryOpenAuthenticationPart.Setup(o => o.Join<OpenAuthenticationPartRecord>()).Returns(mockContentQuerySpecialized.Object);
            mockContentQuery.Setup(o => o.ForPart<OpenAuthenticationPart>()).Returns(mockContentQueryOpenAuthenticationPart.Object);
            mockContentManager.Setup(o => o.Query()).Returns(mockContentQuery.Object);

            var openAuthenticationService = new OpenAuthenticationService(mockContentManager.Object, null, null, null);
            var contentQuery = openAuthenticationService.GetIdentifiersFor(null);
            Assert.That(contentQuery, Is.EqualTo(mockContentQuerySpecialized.Object));
            Assert.That(contentQuery.List(), Is.EquivalentTo(mockContentQuerySpecialized.Object.List()));
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
