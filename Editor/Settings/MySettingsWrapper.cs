using UnityEditor;
using UnityEditor.SettingsManagement;

namespace SteamDeckDeployer.Settings
{
    // Usually you will only have a single Settings instance, so it is convenient to define a UserSetting<T> implementation
    // that points to your instance. In this way you avoid having to pass the Settings parameter in setting field definitions.
    class Setting<T> : UserSetting<T>
    {
        public Setting(string key, T value, SettingsScope scope = SettingsScope.Project)
            : base(DeckSettingsManager.instance, key, value, scope)
        {}

        Setting(UnityEditor.SettingsManagement.Settings settings, string key, T value, SettingsScope scope = SettingsScope.Project)
            : base(settings, key, value, scope) { }
    }
}
