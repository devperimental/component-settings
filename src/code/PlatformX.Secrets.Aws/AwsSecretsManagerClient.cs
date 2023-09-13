using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Logging;
using PlatformX.Secrets.Shared.Behaviours;
using System;
using System.Threading.Tasks;

namespace PlatformX.Secrets.Aws
{
    public class AwsSecretsManagerClient : ISecretClient
    {
        private readonly ISecretsManagerCache _secretsManagerCache;
        private readonly IAmazonSecretsManager _amazonSecretsManager;
        private readonly ILogger<AwsSecretsManagerClient> _logger;

        public AwsSecretsManagerClient(ISecretsManagerCache secretsManagerCache, IAmazonSecretsManager amazonSecretsManager, ILogger<AwsSecretsManagerClient> logger)
        {
            _secretsManagerCache = secretsManagerCache;
            _amazonSecretsManager = amazonSecretsManager;
            _logger = logger;
        }

        public string GetSecret(string keyName)
        {
            var secretValue = GetSecretAsync(keyName).Result;
            return secretValue;
        }

        public async Task<string> GetSecretAsync(string keyName)
        {
            return await _secretsManagerCache.GetSecretString(keyName);
        }

        public void PurgeDeletedSecret(string keyName)
        {
            throw new NotImplementedException();
        }

        public bool SetSecret(string keyName, string value)
        {
            var response = SetSecretAsync(keyName, value).Result;
            return response;
        }

        public async Task<bool> SetSecretAsync(string keyName, string value)
        {
            var response = await _amazonSecretsManager.CreateSecretAsync(new CreateSecretRequest { Name = keyName, SecretString = value });
            return response.HttpStatusCode == System.Net.HttpStatusCode.Created || response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public void StartDeleteSecret(string keyName)
        {
            throw new NotImplementedException();
        }
    }
}
