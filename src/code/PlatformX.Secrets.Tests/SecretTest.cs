using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PlatformX.Secrets.Mocks;
using PlatformX.Settings.Helper;
using PlatformX.Secrets.Shared.Config;
using PlatformX.Settings.Shared.Config;
using PlatformX.Settings.Shared.Behaviours;
using PlatformX.Secrets.Azure;

namespace PlatformX.Secrets.NTesting
{
    [TestFixture]
    public class SecretTests
    {
        private SecretLoaderConfiguration _secretLoaderConfiguration;
        private EndpointHelperConfiguration _endpointHelperConfiguration;
        private IEndpointHelper _endpointHelper;

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
        public void MockSecretLoaderTest()
        {
            var traceLogger = new Mock<ILogger<SecretTests>>();
            
            var secretLoader = new MockSecretLoader<SecretTests>(traceLogger.Object);

            if (secretLoader.GetType() == typeof(MockSecretLoader<SecretTests>))
            {
                Assert.Pass();
            }

        }

        [Test]
        public void KeyVaultSecretLoadTest()
        {
            var traceLogger = new Mock<ILogger<SecretTests>>();
            var secretLoader = new KeyVaultSecretLoader<SecretTests>(_secretLoaderConfiguration, traceLogger.Object, _endpointHelper);

            if (secretLoader.GetType() != typeof(KeyVaultSecretLoader<SecretTests>))
            {
                Assert.Fail();
            }

            var secretValue = secretLoader.LoadSecret("PLATFORM-SERVICE-KEY");

            Assert.IsTrue(!string.IsNullOrEmpty(secretValue));
        }

        [Test]
        public void KeyVaultSecretLoadSetTest()
        {
            var traceLogger = new Mock<ILogger<SecretTests>>();
            var secretLoader = new KeyVaultSecretLoader<SecretTests>(_secretLoaderConfiguration, traceLogger.Object, _endpointHelper);

            if (secretLoader.GetType() != typeof(KeyVaultSecretLoader<SecretTests>))
            {
                Assert.Fail();
            }

            var db = "PlatformDB";
            var protectedKeyList = new List<string>();
            protectedKeyList.AddRange(new List<string> {
                    $"{db}TenantId",
                    $"{db}ApplicationId",
                    $"{db}ApplicationName",
                    $"{db}ApplicationSecret",
                    $"{db}PrimaryEndpoint",
                    $"{db}ReadOnlyEndpoint"
                });

            var secretValue = secretLoader.LoadSecrets(protectedKeyList);

            Assert.IsTrue(secretValue.Count > 0);
        }


        [Test]
        public void KeyVaultSecretSaveTest()
        {
            var traceLogger = new Mock<ILogger<SecretTests>>();
            var secretLoader = new KeyVaultSecretLoader<SecretTests>(_secretLoaderConfiguration, traceLogger.Object, _endpointHelper);

            if (secretLoader.GetType() != typeof(KeyVaultSecretLoader<SecretTests>))
            {
                Assert.Fail();
            }

            var itemToSave = Guid.NewGuid().ToString();

            secretLoader.SaveSecret("UT-TEST-KEY", itemToSave);

            var secretValue = secretLoader.LoadSecret("UT-TEST-KEY");

            Assert.IsTrue(secretValue == itemToSave);
        }

        [Test]
        public void KeyVaultSecretDeleteTest()
        {
            var traceLogger = new Mock<ILogger<SecretTests>>();
            var secretLoader = new KeyVaultSecretLoader<SecretTests>(_secretLoaderConfiguration, traceLogger.Object, _endpointHelper);

            if (secretLoader.GetType() != typeof(KeyVaultSecretLoader<SecretTests>))
            {
                Assert.Fail();
            }
            var itemToSave = Guid.NewGuid().ToString();

            secretLoader.SaveSecret("UT-TEST-KEY-DELETE-P", itemToSave);

            var secretValue = secretLoader.LoadSecret("UT-TEST-KEY-DELETE-P");

            Assert.IsTrue(secretValue == itemToSave);

            secretLoader.DeleteSecret("UT-TEST-KEY-DELETE-P");

            secretValue = secretLoader.LoadSecret("UT-TEST-KEY-DELETE-P");

            Assert.IsTrue(secretValue == "");
        }

        [Test]
        public void KeyVaultClientSecretSaveLoadDeleteTest()
        {
            var traceLogger = new Mock<ILogger<SecretTests>>();
            var secretLoader = new KeyVaultSecretLoader<SecretTests>(_secretLoaderConfiguration, traceLogger.Object, _endpointHelper);

            if (secretLoader.GetType() != typeof(KeyVaultSecretLoader<SecretTests>))
            {
                Assert.Fail();
            }

            var regionKey = "au";
            var locationKey = "est";
            var keyValue = Guid.NewGuid().ToString();

            var tempKey = "UT-TEST-KEY-CLNT" + Guid.NewGuid().ToString();
            secretLoader.SaveClientSecret(tempKey, keyValue, regionKey, locationKey);
            
            var secretValue = secretLoader.LoadClientSecret(tempKey, regionKey, locationKey);

            Assert.IsTrue(secretValue == keyValue);

            secretLoader.DeleteClientSecret(tempKey, regionKey, locationKey);

            Assert.IsTrue(true);
        }

        [Test]
        public void GetSecretsList()
        {
            var traceLogger = new Mock<ILogger<SecretTests>>();
            var secretLoader = new KeyVaultSecretLoader<SecretTests>(_secretLoaderConfiguration, traceLogger.Object, _endpointHelper);

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