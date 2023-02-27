using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
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
        
        public KeyVaultSecretLoader(SecretLoaderConfiguration secretLoaderConfiguration, ILogger<T> traceLogger, IEndpointHelper endpointHelper)
        {
            _secretLoaderConfiguration = secretLoaderConfiguration ?? throw new ArgumentNullException(nameof(secretLoaderConfiguration));
            _traceLogger = traceLogger ?? throw new ArgumentNullException(nameof(traceLogger));
            _endpointHelper = endpointHelper ?? throw new ArgumentNullException(nameof(endpointHelper));
        }

        #region Load
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
                var vault = GetSecretClient(keyVaultUrl, _secretLoaderConfiguration.TenantId);
                var secret = vault.GetSecret(keyName);
                returnValue = secret.Value.Value;
            }
            catch (Exception ex)
            {
                _traceLogger.LogWarning($"Error retrieving key:{keyName}");
                _traceLogger.LogError(ex.Message);
            }

            return returnValue;
        }

        #endregion

        public Dictionary<string, string> LoadSecrets(List<string> keyList)
        {
            var list = new Dictionary<string, string>();

            if (keyList.Count == 0)
            {
                throw new ApplicationException("Please ensure protected keys are passed through from calling method");
            }

            if (keyList.Count > 0)
            {
                var secretClient = GetSecretClient(_endpointHelper.GetKeyVaultUrl(), _secretLoaderConfiguration.TenantId);

                foreach (var item in keyList)
                {
                    var keyName = FormatKeyName(item);

                    if (_secretLoaderConfiguration.Environment.ToLower() == "local")
                    {
                        _traceLogger.LogInformation($"keyName:{keyName}");
                    }

                    //try
                    //{
                        var secret = secretClient.GetSecret(keyName);
                        list.Add(item, secret.Value.Value);
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

        #region Save
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
                var vault = GetSecretClient(vaultUrl, _secretLoaderConfiguration.TenantId);
                vault.SetSecret(keyName, value);
            }
            catch (Exception ex)
            {
                _traceLogger.LogWarning($"Error saving key: {keyName}");
                _traceLogger.LogError(ex.Message);
            }
        }

        #endregion

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
                var vault = GetSecretClient(vaultUrl, _secretLoaderConfiguration.TenantId);
                var deleteOp = vault.StartDeleteSecret(keyName);
                var purgeOp = vault.PurgeDeletedSecret(keyName);
            }
            catch (Exception ex)
            {
                _traceLogger.LogWarning($"Error delete key: {keyName}");
                _traceLogger.LogError(ex.Message);
            }
        }

        private SecretClient GetSecretClient(string vaultUrl, string tenantId)
        {
            if (string.IsNullOrEmpty(vaultUrl))
            {
                throw new ArgumentNullException(nameof(vaultUrl), $"{nameof(vaultUrl)} is null in GetSecretClient");
            }

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId), $"{nameof(tenantId)} is null in GetSecretClient");
            }

            if(_secretLoaderConfiguration.Environment == "local")
            {
                return new SecretClient(new Uri(vaultUrl), new VisualStudioCredential(new VisualStudioCredentialOptions { TenantId = tenantId }));
            }
            else
            {
                return new SecretClient(new Uri(vaultUrl), new DefaultAzureCredential(new DefaultAzureCredentialOptions { SharedTokenCacheTenantId = tenantId }));
            }
        }

        private string FormatKeyName(string keyToFind)
        {
            return $"{_secretLoaderConfiguration.Prefix}-{keyToFind}-{_secretLoaderConfiguration.Environment}";
        }
    }
}
