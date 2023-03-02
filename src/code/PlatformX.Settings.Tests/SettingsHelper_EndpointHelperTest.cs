using Moq;
using NUnit.Framework;
using PlatformX.Secrets.Shared.Behaviours;
using PlatformX.Settings.Helper;
using PlatformX.Settings.Shared.Config;
using PlatformX.Settings.Shared.EnumTypes;
using PlatformX.Settings.Types;

namespace PlatformX.Settings.NTesting
{
    public class SettingsHelper_EndpointHelperTest
    {
        private EndpointHelperConfiguration _endpointHelperConfiguration;
        private EndpointHelper _endpointHelper;

        [SetUp]
        public void Setup()
        {
            _endpointHelperConfiguration = new EndpointHelperConfiguration
            {
                Prefix = "dz",
                Environment = "dev",
                RoleKey = "mgmt",
                Location = "syd",
                Region = "au"
            };

            _endpointHelper = new EndpointHelper(_endpointHelperConfiguration);
        }

        [Test]
        public void GetStorageAccount()
        {
            var storageAccount = _endpointHelper.GetStorageAccount();
            Assert.IsNotNull(storageAccount);
        }

        [Test]
        public void GetServiceBusNamespace()
        {
            var serviceBusNamespace = _endpointHelper.GetServiceBusNamespace();
            Assert.IsNotNull(serviceBusNamespace);
        }

        [Test]
        public void GetKeyVaultUrl()
        {
            var serviceUri = _endpointHelper.GetKeyVaultUrl();
            Assert.IsNotNull(serviceUri);
        }

        [Test]
        [TestCase("au", "est")]
        public void GetClientStorageAccount(string regionKey, string locationKey)
        {
            var serviceUri = _endpointHelper.GetClientStorageAccount(regionKey, locationKey);
            Assert.IsNotNull(serviceUri);
        }

        [Test]
        [TestCase("au", "est")]
        public void GetClientServiceBusNamespace(string regionKey, string locationKey)
        {
            var serviceUri = _endpointHelper.GetClientServiceBusNamespace(regionKey, locationKey);
            Assert.IsNotNull(serviceUri);
        }

        [Test]
        [TestCase("au", "est")]
        public void GetClientKeyVaultUrl(string regionKey, string locationKey)
        {
            var serviceUri = _endpointHelper.GetClientKeyVaultUrl(regionKey, locationKey);
            Assert.IsNotNull(serviceUri);
        }

        [Test]
        [TestCase("au", "est")]
        public void GetManagementStorageAccount(string regionKey, string locationKey)
        {
            var serviceUri = _endpointHelper.GetManagementStorageAccount(regionKey, locationKey);
            Assert.IsNotNull(serviceUri);
        }

        [Test]
        [TestCase("au", "est")]
        public void GetManagementServiceBusNamespace(string regionKey, string locationKey)
        {
            var serviceUri = _endpointHelper.GetManagementServiceBusNamespace(regionKey, locationKey);
            Assert.IsNotNull(serviceUri);
        }

        [Test]
        [TestCase("au", "est")]
        public void GetManagementKeyVaultUrl(string regionKey, string locationKey)
        {
            var serviceUri = _endpointHelper.GetManagementKeyVaultUrl(regionKey, locationKey);
            Assert.IsNotNull(serviceUri);
        }

    }
}