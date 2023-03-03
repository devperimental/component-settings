using Moq;
using NUnit.Framework;
using PlatformX.Secrets.Shared.Behaviours;
using PlatformX.Settings.Types;
using System.Diagnostics.CodeAnalysis;

namespace PlatformX.Settings.NTesting
{
    [ExcludeFromCodeCoverage]
    public class ProtectedRoleConfigurationTest
    {
   
        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void TestRetrieval()
        {
            var fulfilmentRoleType = "C";
            var regionKey = "au";
            var locationKey = "est";

            var secretLoader = new Mock<ISecretLoader>();
            secretLoader.Setup(c => c.LoadClientSecret(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns("CLIENT-SECRET-VALUE");

            var protectedRoleConfiguration = new ProtectedRoleConfiguration(secretLoader.Object);

            var serviceKeyName = "IDENTITY-SERVICE-KEY";
            var requestServiceKey = protectedRoleConfiguration.GetSecretString(serviceKeyName, fulfilmentRoleType, regionKey, locationKey);

            Assert.IsNotNull(requestServiceKey);

            var serviceSecretKeyName = "IDENTITY-SERVICE-SECRET";
            var requestServiceSecret = protectedRoleConfiguration.GetSecretString(serviceSecretKeyName, fulfilmentRoleType, regionKey, locationKey);

            Assert.IsNotNull(requestServiceSecret);

            var azfKeyName = "IdentityService-AZF-KEY";
            var azfKey = protectedRoleConfiguration.GetSecretString(azfKeyName, fulfilmentRoleType, regionKey, locationKey);

            Assert.IsNotNull(azfKey);
        }
    }
}