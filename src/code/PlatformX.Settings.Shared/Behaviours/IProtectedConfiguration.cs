namespace PlatformX.Settings.Shared.Behaviours
{
    public interface IProtectedConfiguration
    {
        string GetSecretString(string secretName);
    }
}
