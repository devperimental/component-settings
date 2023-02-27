namespace PlatformX.Settings.Shared.Behaviours
{
    public interface IProtectedRoleConfiguration
    {
        string GetSecretString(string secretName, string roleKey, string regionKey, string locationKey);
    }
}
