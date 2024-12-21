using System.Linq;
using UnityEditor.Build.Profile;

namespace SteamDeckDeployer
{
	using UnityEditor;
	using UnityEditor.Build.Reporting;
	using UnityEngine;

	public static class CustomBuildHandler
	{
		[InitializeOnLoadMethod]
		private static void RegisterCustomBuildHandler()
		{
			BuildPlayerWindow.RegisterBuildPlayerHandler(BuildAndRunWithCustomOptions);
		}

		private static void BuildAndRunWithCustomOptions(BuildPlayerOptions defaultOptions)
		{
			// Use the specified options but modify the output path
			BuildPlayerOptions customOptions = defaultOptions;

			if (BuildProfile.GetActiveBuildProfile().scriptingDefines
			    .Contains(PostBuildRunScript.KDeploySteamDeckDefine))
			{
				DoSteamDeckOptions(ref defaultOptions, ref customOptions);
			}


			// Perform the build using the modified options
			BuildReport report = BuildPipeline.BuildPlayer(customOptions);

			// Handle build results
			if (report.summary.result == BuildResult.Succeeded)
			{
				Debug.Log("Build succeeded!");
			}
			else
			{
				Debug.LogError("Build failed!");
			}
		}

		private static void DoSteamDeckOptions(ref BuildPlayerOptions defaultOptions, ref BuildPlayerOptions customOptions)
		{
			// Extract the directory and filename from the original path
			string originalDirectory = System.IO.Path.GetDirectoryName(defaultOptions.locationPathName);
			string fileName = "SteamDeckBuild.exe";

			// Create the new path by adding "SteamDeck" before the .exe file
			string newDirectory = System.IO.Path.Combine(originalDirectory, "SteamDeck");
			customOptions.locationPathName = System.IO.Path.Combine(newDirectory, fileName);

			Debug.Log($"Doing SteamDeck build to {customOptions.locationPathName}");
			
			// Ensure the directory exists
			if (!System.IO.Directory.Exists(customOptions.locationPathName))
			{
				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(customOptions.locationPathName));
			}

			if (customOptions.options.HasFlag(BuildOptions.AutoRunPlayer))
			{
				Debug.LogWarning("Steamdeck deploy doesn't support auto run");
				customOptions.options &= ~BuildOptions.AutoRunPlayer;
			}
		}

		private static void RunBuiltPlayer(string pathToPlayer)
		{
			Debug.Log($"Running built player: {pathToPlayer}");
			System.Diagnostics.Process.Start(pathToPlayer); // Launch the built application
		}
	}

}