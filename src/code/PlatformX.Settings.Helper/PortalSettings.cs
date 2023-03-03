using PlatformX.Settings.Shared.Behaviours;
using PlatformX.Settings.Shared.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PlatformX.Settings.Helper
{
    public class PortalSettings : IPortalSettings
    {
        private Dictionary<string, string> _configurationItems;
        private readonly IProtectedConfiguration _protectedConfiguration;
        private readonly PortalConfiguration _portalConfiguration;
        
        private static readonly object LoadLock = new object();

        public PortalSettings(PortalConfiguration portalConfiguration, IProtectedConfiguration protectedConfiguration)
        {
            _configurationItems = new Dictionary<string, string>();
            _portalConfiguration = portalConfiguration;
            _protectedConfiguration = protectedConfiguration;
        }

        public bool GetBool(string key)
        {
            var formattedKey = GetPortalKey(key);
            GetSetting(formattedKey, bool.Parse, out var ret);
            return ret;
        }

        public int GetInt(string key)
        {
            var formattedKey = GetPortalKey(key);
            GetSetting(formattedKey, int.Parse, out var ret);
            return ret;
        }

        public string GetString(string key)
        {
            var formattedKey = GetPortalKey(key);  
            GetSetting(formattedKey, itm => itm, out var ret);
            return ret;
        }

        private string GetPortalKey(string key)
        {
            return $"{_portalConfiguration.PortalName}-{key}".ToLower();
        }

        private string GetServiceKey(string key, string serviceName)
        {
            return $"{_portalConfiguration.PortalName}-{serviceName}-{key}".ToLower();
        }

        private string GetProviderKey(string key, string serviceName, string providerName)
        {
            return $"{_portalConfiguration.PortalName}-{serviceName}-{providerName}-{key}".ToLower();
        }

        private void GetSetting<T>(string formattedKey, Func<string, T> convertor, out T result)
        {
            try
            {
                var settingsVal = _configurationItems[formattedKey];
                result = convertor(settingsVal);
                return;
            }
            catch
            {
                // ignored
            }

            result = default;
        }


        public void LoadConfigurationItems(Dictionary<string, string> configurationItems)
        {
            lock (LoadLock)
            {
                _configurationItems = configurationItems;
            }
        }

        public string GetSecretString(string key)
        {
            return _protectedConfiguration.GetSecretString(key);
        }

        private static void TryParse<T>(string source, out T value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertFrom(typeof(string)))
            {
                value = (T)converter.ConvertFromString(source);
                return;
            }

            value = default;
        }

        public T GetPortalSetting<T>(string key)
        {
            var formattedKey = GetPortalKey(key);
            var itemValue = _configurationItems[formattedKey];
            TryParse(itemValue, out T value);
            return value;
        }

        public T GetServiceSetting<T>(string key, string serviceName)
        {
            var formattedKey = GetServiceKey(key, serviceName);
            var itemValue = _configurationItems[formattedKey];
            TryParse(itemValue, out T value);
            return value;
        }

        public T GetProviderSetting<T>(string key, string serviceName, string providerName)
        {
            var formattedKey = GetProviderKey(key, serviceName, providerName);
            var itemValue = _configurationItems[formattedKey];
            TryParse(itemValue, out T value);
            return value;
        }
    }
}