using Microsoft.Extensions.Logging;
using PlatformX.Secrets.Behaviours;
using System;
using System.Collections.Generic;

namespace PlatformX.Secrets.Mocks
{
    public class MockSecretLoader<T> : ISecretLoader
    {
        private ILogger<T> _traceLogger;
        private Dictionary<string, string> _defaultCollection;

        public MockSecretLoader(ILogger<T> traceLogger)
        {
            _traceLogger = traceLogger;
            _defaultCollection = new Dictionary<string, string> {
                { "PLATFORM-SERVICE-KEY","C4EDEF4F-F4DE-4C49-80D6-8B94C16C6390" },
                { "PLATFORM-SERVICE-SECRET","DEFAULT-SERVICE-SECRET"},
                { "LOGGING-SERVICE-KEY","C4EDEF4F-F4DE-4C49-80D6-8B94C16C6390" },
                { "LOGGING-SERVICE-SECRET","DEFAULT-SERVICE-SECRET"},
                { "MESSAGING-SERVICE-KEY","C4EDEF4F-F4DE-4C49-80D6-8B94C16C6390" },
                { "MESSAGING-SERVICE-SECRET","DEFAULT-SERVICE-SECRET"},
                { "AUDIT-SERVICE-KEY","C4EDEF4F-F4DE-4C49-80D6-8B94C16C6390" },
                { "AUDIT-SERVICE-SECRET","DEFAULT-SERVICE-SECRET"},
                { "IDENTITY-SERVICE-KEY","C4EDEF4F-F4DE-4C49-80D6-8B94C16C6390" },
                { "IDENTITY-SERVICE-SECRET","DEFAULT-SERVICE-SECRET"}
            };
        }

        public string DeleteSecret(string key, string value)
        {
            throw new NotImplementedException();
        }

        public void SaveClientSecret(string key, string value, string regionKey, string locationKey)
        {
            throw new NotImplementedException();
        }

        public void DeleteSecret(string key)
        {
            throw new NotImplementedException();
        }

        public void DeleteClientSecret(string key, string regionKey, string locationKey)
        {
            throw new NotImplementedException();
        }

        public string LoadSecret(string key)
        {
            if (_defaultCollection.ContainsKey(key))
                return _defaultCollection[key];
            else
                return key;
        }

        public string LoadClientSecret(string key, string regionKey, string locationKey)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> LoadSecrets(List<string> keyList)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var key in keyList)
            {
                dictionary.Add(key, Guid.NewGuid().ToString());
            }

            return dictionary;
        }

        public void PopulateSecrets(Dictionary<string, string> secrets)
        {
            foreach (var item in secrets)
            {
                if (!_defaultCollection.ContainsKey(item.Key))
                {
                    _defaultCollection.Add(item.Key, item.Value);
                }
                else
                {
                    _defaultCollection[item.Key] = item.Value;
                }
            }
        }

        public string SaveSecret(string key, string value)
        {
            throw new NotImplementedException();
        }

        void ISecretLoader.SaveSecret(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}