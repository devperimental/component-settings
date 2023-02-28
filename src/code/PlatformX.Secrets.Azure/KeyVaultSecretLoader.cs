using Microsoft.Extensions.Logging;
using PlatformX.Secrets.Shared.Behaviours;
using PlatformX.Secrets.Shared.Config;
using PlatformX.Settings.Shared.Behaviours;
using System;
using System.Collections.Generic;

namespace PlatformX.Secrets.Azure
{
    public class KeyVaultSecretLoader<T> : ISecretLoader
    {
        private readonly SecretLoaderConfiguration _secretLoaderConfiguration;
        private readonly ILogger<T> _traceLogger;
        private readonly IEndpointHelper _endpointHelper;
        private readonly ISecretClient _secretClient;
        
        public KeyVaultSecretLoader(SecretLoaderConfiguration secretLoaderConfiguration, 
            ILogger<T> traceLogger, 
            IEndpointHelper endpointHelper,
            ISecretClient secretClient)
        {
            _secretLoaderConfiguration = secretLoaderConfiguration ?? throw new ArgumentNullException(nameof(secretLoaderConfiguration));
            _traceLogger = traceLogger ?? throw new ArgumentNullException(nameof(traceLogger));
            _endpointHelper = endpointHelper ?? throw new ArgumentNullException(nameof(endpointHelper));
            _secretClient = secretClient;
        }

        public string LoadClientSecret(string secretName, string regionKey, string locationKey)
        {
            var keyVaultUrl = _endpointHelper.GetClientKeyVaultUrl(regionKey, locationKey);
            return LoadSecretInternal(secretName, keyVaultUrl);
        }

        public string LoadSecret(string secretName)
        {
            var keyVaultUrl = _endpointHelper.GetKeyVaultUrl();
            return LoadSecretInternal(secretName, keyVaultUrl);
        }

        private string LoadSecretInternal(string secretName, string keyVaultUrl)
        {
            var returnValue = string.Empty;

            var keyName = FormatKeyName(secretName);

            if (_secretLoaderConfiguration.Environment.ToLower() == "local")
            {
                _traceLogger.LogInformation($"starting call to retrieve keyName:{keyName}");
            }

            try
            {
                returnValue = _secretClient.GetSecret(keyName);
            }
            catch (Exception ex)
            {
                _traceLogger.LogError(ex, "Error retrieving {keyName}", keyName);
            }

            return returnValue;
        }

        public Dictionary<string, string> LoadSecrets(List<string> keyList)
        {
            var list = new Dictionary<string, string>();

            if (keyList.Count == 0)
            {
                throw new ApplicationException("Please ensure protected keys are passed through from calling method");
            }

            if (keyList.Count > 0)
            {
                foreach (var item in keyList)
                {
                    var keyName = FormatKeyName(item);

                    if (_secretLoaderConfiguration.Environment.ToLower() == "local")
                    {
                        _traceLogger.LogInformation($"keyName:{keyName}");
                    }

                    //try
                    //{
                        var secret = _secretClient.GetSecret(keyName);
                        list.Add(item, secret);
                    //}
                    //catch (Exception ex)
                    //{
                    //    _traceLogger.LogWarning($"Error retrieving key:{keyName}");
                    //    _traceLogger.LogError(ex.Message);
                   // }
                }
            }

            return list;
        }

        public void PopulateSecrets(Dictionary<string, string> secrets)
        {
            throw new NotImplementedException();
        }

        public void SaveSecret(string key, string value)
        {
            var vaultUrl = _endpointHelper.GetKeyVaultUrl();
            SaveClientSecretInternal(key, value, vaultUrl);
        }

        public void SaveClientSecret(string key, string value, string regionKey, string locationKey)
        {
            var vaultUrl = _endpointHelper.GetClientKeyVaultUrl(regionKey, locationKey);
            SaveClientSecretInternal(key, value, vaultUrl);
        }

        private void SaveClientSecretInternal(string key, string value, string vaultUrl)
        {
            var keyName = FormatKeyName(key);

            if (_secretLoaderConfiguration.Environment.ToLower() != "prod")
            {
                _traceLogger.LogInformation($"attempting to save keyName:{keyName}");
            }

            try
            {
                _secretClient.SetSecret(keyName, value);
            }
            catch (Exception ex)
            {
                _traceLogger.LogWarning($"Error saving key: {keyName}");
                _traceLogger.LogError(ex.Message);
            }
        }

        public void DeleteSecret(string key)
        {
            string keyVaultUrl = _endpointHelper.GetKeyVaultUrl();
            DeleteSecretInternal(key, keyVaultUrl);
        }

        public void DeleteClientSecret(string key, string regionKey, string locationKey)
        {
            var keyVaultUrl = _endpointHelper.GetClientKeyVaultUrl(regionKey, locationKey);
            DeleteSecretInternal(key, keyVaultUrl);
        }

        private void DeleteSecretInternal(string key, string vaultUrl)
        {
            var keyName = FormatKeyName(key);

            if (_secretLoaderConfiguration.Environment.ToLower() != "prod")
            {
                _traceLogger.LogInformation($"attempting to delete keyName:{keyName}");
            }

            try
            {
                _secretClient.StartDeleteSecret(keyName);
                _secretClient.PurgeDeletedSecret(keyName);
            }
            catch (Exception ex)
            {
                _traceLogger.LogWarning($"Error delete key: {keyName}");
                _traceLogger.LogError(ex.Message);
            }
        }

        private string FormatKeyName(string keyToFind)
        {
            return $"{_secretLoaderConfiguration.Prefix}-{keyToFind}-{_secretLoaderConfiguration.Environment}";
        }
    }
}
