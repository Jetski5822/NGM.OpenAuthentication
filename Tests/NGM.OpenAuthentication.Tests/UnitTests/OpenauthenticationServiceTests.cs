using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGM.OpenAuthentication.Services;
using NUnit.Framework;

namespace NGM.OpenAuthentication.Tests.UnitTests {
    [TestFixture]
    public class OpenAuthenticationServiceTests {
        [Test]
        public void should_return_null_when_no_user_exists_for_identifier() {
            var openAuthenticationService = new OpenAuthenticationService();
            var user = openAuthenticationService.GetUser("test");

            Assert.That(user, Is.Null);
        }
    }
}
