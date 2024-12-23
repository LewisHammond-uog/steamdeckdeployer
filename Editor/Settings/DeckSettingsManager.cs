﻿using UnityEditor;
using UnityEditor.SettingsManagement;

namespace SteamDeckDeployer.Settings
{
    /// <summary>
    /// This class will act as a manager for the <see cref="Settings"/> singleton.
    /// </summary>
    static class DeckSettingsManager
    {
        // Replace this with your own package name. Project settings will be stored in a JSON file in a directory matching
        // this name.
        internal const string k_PackageName = "com.lewishammond.steamdeckdeployer";

        static UnityEditor.SettingsManagement.Settings s_Instance;

        internal static UnityEditor.SettingsManagement.Settings instance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = new UnityEditor.SettingsManagement.Settings(k_PackageName);

                return s_Instance;
            }
        }

        // The rest of this file is just forwarding the various setting methods to the instance.

        public static void Save()
        {
            instance.Save();
        }

        public static T Get<T>(string key, SettingsScope scope = SettingsScope.Project, T fallback = default(T))
        {
            return instance.Get<T>(key, scope, fallback);
        }

        public static void Set<T>(string key, T value, SettingsScope scope = SettingsScope.Project)
        {
            instance.Set<T>(key, value, scope);
        }

        public static bool ContainsKey<T>(string key, SettingsScope scope = SettingsScope.Project)
        {
            return instance.ContainsKey<T>(key, scope);
        }
    }
}
