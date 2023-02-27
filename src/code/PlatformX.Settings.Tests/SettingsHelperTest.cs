using Moq;
using NUnit.Framework;
using PlatformX.Secrets.Shared.Behaviours;
using PlatformX.Settings.Helper;
using PlatformX.Settings.Shared.Config;
using PlatformX.Settings.Types;
using System.Collections.Generic;

namespace PlatformX.Settings.NTesting
{
    public class SettingsHelperTest
    {
        private string _portalName;
        private PortalConfiguration _portalConfiguration;

        [SetUp]
        public void Setup()
        {
            _portalName = "ARCHITECTED";
            _portalConfiguration = new PortalConfiguration
            {
                PortalName = _portalName
            };
        }

        [Test]
        public void AppSettingsTest()
        {
            var boolVal = "true";
            var intVal = 300;
            var stringVal = "a string";

            var serviceName = "SN";
            var providerName = "PN";

            var keyList = new Dictionary<string, string>
            {
                {$"{_portalName}-CheckTimestamp".ToLower(), boolVal },
                {$"{_portalName}-CallExpirySeconds".ToLower(), intVal.ToString() },
                {$"{_portalName}-BigString".ToLower(), stringVal },
                {$"{_portalName}-{serviceName}-CheckTimestamp".ToLower(), boolVal },
                {$"{_portalName}-{serviceName}-CallExpirySeconds".ToLower(), intVal.ToString() },
                {$"{_portalName}-{serviceName}-BigString".ToLower(), stringVal },
                {$"{_portalName}-{serviceName}-{providerName}-CheckTimestamp".ToLower(), boolVal },
                {$"{_portalName}-{serviceName}-{providerName}-CallExpirySeconds".ToLower(), intVal.ToString() },
                {$"{_portalName}-{serviceName}-{providerName}-BigString".ToLower(), stringVal }
            };

            var secretLoader = new Mock<ISecretLoader>();
            secretLoader.Setup(loader => loader.LoadSecret("ApplicationServiceKey")).Returns("A");
            secretLoader.Setup(loader => loader.LoadSecret("ApplicationServiceSecretKey")).Returns("B");

            var protectedConfiguration = new ProtectedConfiguration(secretLoader.Object);
            
            var portalSettings = new PortalSettings(_portalConfiguration, protectedConfiguration);
            portalSettings.LoadConfigurationItems(keyList);

            var a = portalSettings.GetBool("CheckTimestamp");
            var b = portalSettings.GetInt("CallExpirySeconds");

            var c = portalSettings.GetSecretString("ApplicationServiceKey");
            var d = portalSettings.GetSecretString("ApplicationServiceSecretKey");

            Assert.IsTrue(a);
            Assert.IsTrue(b == 300);
            Assert.IsTrue(c == "A");
            Assert.IsTrue(d == "B");

            var e1 = portalSettings.GetPortalSetting<bool>("CheckTimestamp");
            var e2 = portalSettings.GetPortalSetting<int>("CallExpirySeconds");
            var e3 = portalSettings.GetPortalSetting<string>("BigString");

            Assert.IsTrue(e1);
            Assert.IsTrue(e2 == intVal);
            Assert.IsTrue(e3 == stringVal);

            var f1 = portalSettings.GetServiceSetting<bool>("CheckTimestamp", serviceName);
            var f2 = portalSettings.GetServiceSetting<int>("CallExpirySeconds", serviceName);
            var f3 = portalSettings.GetServiceSetting<string>("BigString", serviceName);

            Assert.IsTrue(f1);
            Assert.IsTrue(f2 == intVal);
            Assert.IsTrue(f3 == stringVal);

            var g1 = portalSettings.GetProviderSetting<bool>("CheckTimestamp", serviceName, providerName);
            var g2 = portalSettings.GetProviderSetting<int>("CallExpirySeconds", serviceName, providerName);
            var g3 = portalSettings.GetProviderSetting<string>("BigString", serviceName, providerName);

            Assert.IsTrue(g1);
            Assert.IsTrue(g2 == intVal);
            Assert.IsTrue(g3 == stringVal);
        }
    }
}