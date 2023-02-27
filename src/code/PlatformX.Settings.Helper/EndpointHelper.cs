using PlatformX.Settings.Shared.Behaviours;
using PlatformX.Settings.Shared.Config;
using PlatformX.Settings.Shared.EnumTypes;

namespace PlatformX.Settings.Helper
{
    public class EndpointHelper : IEndpointHelper
    {
        private readonly EndpointHelperConfiguration _endpointHelperConfiguration;

        public EndpointHelper(EndpointHelperConfiguration endpointHelperConfiguration)
        {
            _endpointHelperConfiguration = endpointHelperConfiguration;
        }

        public string GetStorageAccount()
        {
            return
                $"{_endpointHelperConfiguration.Prefix}appstor{_endpointHelperConfiguration.Environment}{_endpointHelperConfiguration.RoleKey}{_endpointHelperConfiguration.Region}{_endpointHelperConfiguration.Location}";
        }

        public string GetServiceBusNamespace()
        {
            return
                $"{_endpointHelperConfiguration.Prefix}-sb-{_endpointHelperConfiguration.Environment}-{_endpointHelperConfiguration.RoleKey}-{_endpointHelperConfiguration.Region}-{_endpointHelperConfiguration.Location}";
        }

        public string GetKeyVaultUrl()
        {
            return
                $"https://{_endpointHelperConfiguration.Prefix}-kv-{_endpointHelperConfiguration.Environment}-{_endpointHelperConfiguration.RoleKey}-{_endpointHelperConfiguration.Region}-{_endpointHelperConfiguration.Location}.vault.azure.net";
        }

        public string GetClientStorageAccount(string regionKey, string locationKey)
        {
            return
                $"{_endpointHelperConfiguration.Prefix}appstor{_endpointHelperConfiguration.Environment}{StackType.Client}{regionKey}{locationKey}";
        }

        public string GetClientServiceBusNamespace(string regionKey, string locationKey)
        {
            return
                $"{_endpointHelperConfiguration.Prefix}-sb-{_endpointHelperConfiguration.Environment}-{StackType.Client}-{regionKey}-{locationKey}";
        }

        public string GetClientKeyVaultUrl(string regionKey, string locationKey)
        {
            return
                $"https://{_endpointHelperConfiguration.Prefix}-kv-{_endpointHelperConfiguration.Environment}-{StackType.Client}-{regionKey}-{locationKey}.vault.azure.net";
        }

        public string GetManagementStorageAccount(string regionKey, string locationKey)
        {
            return
                $"{_endpointHelperConfiguration.Prefix}appstor{_endpointHelperConfiguration.Environment}{StackType.Management}{regionKey}{locationKey}";
        }

        public string GetManagementServiceBusNamespace(string regionKey, string locationKey)
        {
            return
                $"{_endpointHelperConfiguration.Prefix}-sb-{_endpointHelperConfiguration.Environment}-{StackType.Management}-{regionKey}-{locationKey}";
        }

        public string GetManagementKeyVaultUrl(string regionKey, string locationKey)
        {
            return
                $"https://{_endpointHelperConfiguration.Prefix}-kv-{_endpointHelperConfiguration.Environment}-{StackType.Management}-{regionKey}-{locationKey}.vault.azure.net";
        }
    }
}