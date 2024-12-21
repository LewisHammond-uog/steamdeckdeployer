using System;
using UnityEditor;
using UnityEditor.SettingsManagement;
using UnityEngine;

namespace SteamDeckDeployer.Settings
{
	class DeckSettings : EditorWindow
	{
		// [UserSetting] with no arguments simply registers the key with UserSettingsProvider so that it can be included
		// in debug views and reset with the options gizmo. Usually this is used in conjunction with [UserSettingsBlock].
		[UserSetting]
		static Setting<string> s_Foo = new Setting<string>("steamdeck.gamename", string.Empty, SettingsScope.Project);

		// A UserSettingBlock is a callback invoked from the UserSettingsProvider. It allows you to draw more complicated
		// UI elements without the need to create a new SettingsProvider. Parameters are "category" and "search keywords."
		// For maximum compatibility, use `SettingsGUILayout` searchable and settings fields to get features like search
		// and per-setting reset with a context click.
		[UserSettingBlock("Custom GUI Settings")]
		static void ConditionalValueGUI(string searchContext)
		{
			EditorGUI.BeginChangeCheck();


			EditorGUI.BeginChangeCheck();

			s_Foo.value = SettingsGUILayout.SearchableTextField("NAME", s_Foo.value, searchContext);
			
			// Because FooClass is a reference type, we need to apply the changes to the backing repository (SetValue
			// would also work here).
			if (EditorGUI.EndChangeCheck())
				s_Foo.ApplyModifiedProperties();

			if (EditorGUI.EndChangeCheck())
				DeckSettingsManager.Save();
		}
	}
}
