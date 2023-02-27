using System.Collections.Generic;

namespace PlatformX.Settings.Shared.Behaviours
{
    public interface IPortalSettings
    {
        bool GetBool(string key);
        string GetString(string key);
        int GetInt(string key);
        T GetPortalSetting<T>(string key);
        T GetServiceSetting<T>(string key, string serviceName);
        T GetProviderSetting<T>(string key, string serviceName, string providerName);
        string GetSecretString(string key);
        void LoadConfigurationItems(Dictionary<string, string> configurationItems);
    }
}
