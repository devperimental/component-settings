using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PlatformX.Settings.Helper;
using PlatformX.Secrets.Shared.Config;
using PlatformX.Settings.Shared.Config;
using PlatformX.Settings.Shared.Behaviours;
using PlatformX.Secrets.Azure;
using PlatformX.Secrets.Shared.Behaviours;

namespace PlatformX.Secrets.NTesting
{
    [TestFixture]
    public class SecretTests
    {
        private SecretLoaderConfiguration _secretLoaderConfiguration;
        private EndpointHelperConfiguration _endpointHelperConfiguration;
        private IEndpointHelper _endpointHelper;
        
        private List<string> _keyListFull = new List<string> {
                    $"PlatformTenantId",
                    $"PlatformApplicationId",
                    $"PlatformApplicationName",
                    $"PlatformApplicationSecret",
                    $"PlatformPrimaryEndpoint",
                    $"PlatformReadOnlyEndpoint"
                };

        private List<string> _keyListPartial = new List<string> {
                    $"PlatformTenantId",
                    $"PlatformApplicationId"
                };

        [SetUp]
        public void Init()
        {
            _secretLoaderConfiguration ??=
                TestHelper.GetConfiguration<SecretLoaderConfiguration>(TestContext.CurrentContext.TestDirectory, "SecretLoaderConfiguration");
            
            _endpointHelperConfiguration ??=
                TestHelper.GetConfiguration<EndpointHelperConfiguration>(TestContext.CurrentContext.TestDirectory, "EndpointConfiguration");

            _endpointHelper = new EndpointHelper(_endpointHelperConfiguration);
        }

        [Test]
        public void KeyVaultSecretLoadTest()
        {
            var traceLogger = new Mock<ILogger<SecretTests>>();
            var secretClient = new Mock<ISecretClient>();

            secretClient.Setup(c => c.GetSecret(It.IsAny<string>())).Returns("PLATFORM-SERVICE-KEY-VALUE");

            var secretLoader = new KeyVaultSecretLoader<SecretTests>(_secretLoaderConfiguration, traceLogger.Object, _endpointHelper, secretClient.Object);

            var secretValue = secretLoader.LoadSecret("PLATFORM-SERVICE-KEY");

            Assert.IsNotEmpty(secretValue);
        }

        [Test]
        public void KeyVaultSecretLoadSetTest()
        {
            var traceLogger = new Mock<ILogger<SecretTests>>();
            var secretClient = new Mock<ISecretClient>();
            var secretLoader = new KeyVaultSecretLoader<SecretTests>(_secretLoaderConfiguration, traceLogger.Object, _endpointHelper, secretClient.Object);

            var protectedKeyList = new List<string>();
            protectedKeyList.AddRange(_keyListPartial);

            var secretValue = secretLoader.LoadSecrets(protectedKeyList);

            Assert.IsTrue(secretValue.Count > 0);
        }


        [Test]
        public void KeyVaultSecretSaveTest()
        {
            var traceLogger = new Mock<ILogger<SecretTests>>();
            var secretClient = new Mock<ISecretClient>();
            var itemToSave = Guid.NewGuid().ToString();
            
            secretClient.Setup(c => c.GetSecret(It.IsAny<string>())).Returns(itemToSave);
            var secretLoader = new KeyVaultSecretLoader<SecretTests>(_secretLoaderConfiguration, traceLogger.Object, _endpointHelper, secretClient.Object);

            secretLoader.SaveSecret("UT-TEST-KEY", itemToSave);

            var secretValue = secretLoader.LoadSecret("UT-TEST-KEY");

            Assert.IsTrue(secretValue == itemToSave);
        }

        [Test]
        public void KeyVaultSecretDeleteTest()
        {
            var traceLogger = new Mock<ILogger<SecretTests>>();
            var secretClient = new Mock<ISecretClient>();
            
            secretClient.Setup(c => c.GetSecret(It.IsAny<string>())).Returns(string.Empty);
            var secretLoader = new KeyVaultSecretLoader<SecretTests>(_secretLoaderConfiguration, traceLogger.Object, _endpointHelper, secretClient.Object);

            secretLoader.DeleteSecret("UT-TEST-KEY-DELETE-P");

            var secretValue = secretLoader.LoadSecret("UT-TEST-KEY-DELETE-P");

            Assert.IsEmpty(secretValue);
        }

        [Test]
        public void KeyVaultClientSecretLoad()
        {
            var traceLogger = new Mock<ILogger<SecretTests>>();
            var secretClient = new Mock<ISecretClient>();
            
            var keyValue = Guid.NewGuid().ToString();

            secretClient.Setup(c => c.GetSecret(It.IsAny<string>())).Returns(keyValue);

            var secretLoader = new KeyVaultSecretLoader<SecretTests>(_secretLoaderConfiguration, traceLogger.Object, _endpointHelper, secretClient.Object);

            var regionKey = "au";
            var locationKey = "est";
            
            var tempKey = "UT-TEST-KEY-CLNT" + Guid.NewGuid().ToString();
            secretLoader.SaveClientSecret(tempKey, keyValue, regionKey, locationKey);
            
            var secretValue = secretLoader.LoadClientSecret(tempKey, regionKey, locationKey);

            Assert.IsTrue(secretValue == keyValue);

        }

        [Test]
        public void GetSecretsList()
        {
            var traceLogger = new Mock<ILogger<SecretTests>>();
            var secretClient = new Mock<ISecretClient>();
            var secretLoader = new KeyVaultSecretLoader<SecretTests>(_secretLoaderConfiguration, traceLogger.Object, _endpointHelper, secretClient.Object);

            var dbList = "TestDB";
            var protectedKeyList = new List<string>();

            foreach (var db in dbList.Split(","))
            {
                protectedKeyList.AddRange(new List<string> {
                    $"{db}TenantId",
                    $"{db}ApplicationId",
                    $"{db}ApplicationName",
                    $"{db}ApplicationSecret",
                    $"{db}PrimaryEndpoint",
                    $"{db}ReadOnlyEndpoint"
                });
            }

            var keys = secretLoader.LoadSecrets(protectedKeyList);

            Assert.IsTrue(keys.Count > 0);
        }
    }
}