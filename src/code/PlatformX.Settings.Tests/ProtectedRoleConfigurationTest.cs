using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PlatformX.Secrets.Shared.Behaviours;
using PlatformX.Settings.Shared.Behaviours;
using PlatformX.Settings.Shared.Config;
using PlatformX.Settings.Types;

namespace PlatformX.Settings.NTesting
{
    public class ProtectedRoleConfigurationTest
    {
   
        [SetUp]
        public void Init()
        {
        }

        public ProtectedRoleConfiguration InitProtectedRoleConfiguration()
        {
            var secretLoader = new Mock<ISecretLoader>();
            var protectedRoleConfiguration = new ProtectedRoleConfiguration(secretLoader.Object);

            return protectedRoleConfiguration;
        }

        [Test]
        public void TestRetrieval()
        {
            var fulfilmentRoleType = "C";
            var regionKey = "au";
            var locationKey = "est";

            var protectedRoleConfiguration = InitProtectedRoleConfiguration();

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