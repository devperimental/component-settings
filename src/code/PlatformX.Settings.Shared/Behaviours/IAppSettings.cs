namespace PlatformX.Settings.Shared.Behaviours
{
   
    public interface IOrganisationSettings
    {
        bool GetBool(string key, string organisationName);
        string GetString(string key, string organisationName);
        int GetInt(string key, string organisationName);
        T GetOrganisationSetting<T>(string key, string organisationName);
        T GetApplicationSetting<T>(string key, string organisationName, string applicationName);
    }
}
