using System;
using UnityEditor;
using UnityEditor.SettingsManagement;
using UnityEngine;

namespace SteamDeckDeployer.Settings
{
	/// <summary>
	/// To create an entry in the Preferences window, define a new SettingsProvider inheriting <see cref="UserSettingsProvider"/>.
	/// You can also choose to implement your own SettingsProvider and ignore this implementation. The benefit of using
	/// <see cref="UserSettingsProvider"/> is that all <see cref="UserSetting{T}"/> fields in the assembly are automatically
	/// populated within the preferences, with support for search and resetting default values.
	/// </summary>
	static class DeckSettingsProvider
	{
		const string k_PreferencesPath = "Preferences/Steam Deck Deployer";
		
		[SettingsProvider]
		static SettingsProvider CreateSettingsProvider()
		{
			var provider = new UserSettingsProvider(k_PreferencesPath,
				DeckSettingsManager.instance,
				new [] { typeof(DeckSettingsProvider).Assembly });

			return provider;
		}
	}
}
