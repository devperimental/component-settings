using Amazon.SecretsManager.Extensions.Caching;
using Microsoft.Extensions.Logging;
using PlatformX.Secrets.Shared.Behaviours;
using System;

namespace PlatformX.Secrets.Aws
{
    public class AwsSecretsManagerClient : ISecretClient
    {
        private readonly ISecretsManagerCache _secretsManagerCache;
        private readonly ILogger<AwsSecretsManagerClient> _logger;

        public AwsSecretsManagerClient(ISecretsManagerCache secretsManagerCache, ILogger<AwsSecretsManagerClient> logger)
        {
            _secretsManagerCache = secretsManagerCache;
            _logger = logger;
        }

        public string GetSecret(string keyName)
        {
            var secretValue = string.Empty;

            try
            {
                secretValue = _secretsManagerCache.GetSecretString(keyName).Result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occured retrieving secret {secretName}", keyName);
            }

            return secretValue;
        }

        public void PurgeDeletedSecret(string keyName)
        {
            throw new NotImplementedException();
        }

        public void SetSecret(string keyName, string value)
        {
            throw new NotImplementedException();
        }

        public void StartDeleteSecret(string keyName)
        {
            throw new NotImplementedException();
        }
    }
}
