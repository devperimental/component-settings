using PlatformX.Secrets.Shared.Behaviours;
using PlatformX.Settings.Shared.Behaviours;
using System.Collections.Generic;

namespace PlatformX.Settings.Types
{
    public class ProtectedConfiguration : IProtectedConfiguration
    {
        private readonly ISecretLoader _secretLoader;
        private readonly Dictionary<string, string> _protectedItems;
        private static readonly object LoadLock = new object();

        public ProtectedConfiguration (ISecretLoader secretLoader)
        {
            _secretLoader = secretLoader;
            _protectedItems = new Dictionary<string, string>();
        }

        public string GetSecretString(string secretName)
        {
            if (_protectedItems != null && _protectedItems.ContainsKey(secretName))
            {
                return _protectedItems[secretName];
            }
            else
            {
                return LoadFromProtectedStore(secretName);
            }
        }

        private string LoadFromProtectedStore(string secretName)
        {
            string secretValue;
            lock (LoadLock)
            {
                if (_protectedItems.ContainsKey(secretName))
                {
                    return _protectedItems[secretName];
                }

                secretValue = _secretLoader.LoadSecret(secretName);

                if (!_protectedItems.ContainsKey(secretName))
                {
                    _protectedItems.Add(secretName, secretValue);
                }
                else
                {
                    _protectedItems[secretName] = secretValue;
                }
            }

            return secretValue;
        }
    }
}
