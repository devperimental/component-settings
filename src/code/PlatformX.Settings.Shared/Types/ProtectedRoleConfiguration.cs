using PlatformX.Secrets.Shared.Behaviours;
using PlatformX.Settings.Shared.Behaviours;

namespace PlatformX.Settings.Types
{
    public class ProtectedRoleConfiguration : IProtectedRoleConfiguration
    {
        private readonly ISecretLoader _secretLoader;
    
        public ProtectedRoleConfiguration(ISecretLoader secretLoader)
        {
            _secretLoader = secretLoader;
        }

        public string GetSecretString(string secretName, string roleKey, string regionKey, string locationKey)
        {
            return LoadFromProtectedStore(secretName, roleKey, regionKey, locationKey);
        }

        private string LoadFromProtectedStore(string secretName, string roleKey, string regionKey, string locationKey)
        {
            return _secretLoader.LoadClientSecret(secretName, regionKey, locationKey);
        }
    }
}