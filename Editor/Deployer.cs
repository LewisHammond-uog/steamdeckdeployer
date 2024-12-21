// Wes Jurica - 2022
// This is free and unencumbered software released into the public domain.

// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.

// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain.We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors.We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

// For more information, please refer to<https://unlicense.org>

using System.Collections;
using System.Linq;
using SteamDeckDeployer.Settings;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.Build.Profile;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Networking;

// Check "Auto upload" in the Title Upload tab of the Steam Devkit Management Tool. 
// Enter "Name" from Steam Devkit Management Tool here in SteamDeckBuildJson class.
// Then add this script to and "Editor" folder in your project.
// After your project is built, the tool will atempt to install the game on your Steam Deck.

public static class PostBuildRunScript
{
    public const string KDeploySteamDeckDefine = "DEPLOY_TO_STEAMDECK";
    
    static PostUploadToSteamDeckCoroutine postCoroutine;

    [PostProcessBuild(1000)]
    public static void UploadToSteamDeck(BuildTarget target, string pathToBuiltProject)
    {
        if (BuildProfile.GetActiveBuildProfile().scriptingDefines.Contains(KDeploySteamDeckDefine))
        {
            if (target == BuildTarget.StandaloneWindows64)
            {
                Debug.Log("Deploying to SteamDeck");
                postCoroutine = new PostUploadToSteamDeckCoroutine();
                postCoroutine.Start();
            }
        }
    }

    [System.Serializable]
    public class SteamDeckBuildJson
    {
        public string type = "build";
        public string status = "success";
        public string name = "NOTSET"; // Enter "Name" from Steam Devkit Management Tool here
    }

    public class PostUploadToSteamDeckCoroutine
    {
        public void Start()
        {
            EditorCoroutineUtility.StartCoroutine(PostUploadToSteamDeck(), this);
        }
    }

    static IEnumerator PostUploadToSteamDeck()
    {
        SteamDeckBuildJson steamDeckBuildJson = new SteamDeckBuildJson();
        steamDeckBuildJson.name = DeckSettingsManager.Get<string>("steamdeck.gamename");

        string json = JsonUtility.ToJson(steamDeckBuildJson);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        using (UnityWebRequest webRequest = UnityWebRequest.PostWwwForm("http://127.0.0.1:32010/post_event", ""))
        {
            using (UploadHandlerRaw uH = new UploadHandlerRaw(bytes))
            {
                webRequest.uploadHandler = uH;
                webRequest.SetRequestHeader("Content-Type", "application/json");
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    if (webRequest.error.Equals("Cannot connect to destination host"))
                    {
                        Debug.LogError("Steam Deck upload error: Cannot connect to Steam Deck. Is SteamOS Devkit Client running?");
                    }
                    else if (webRequest.error.Equals("HTTP/1.1 500 Internal Server Error"))
                    {
                        Debug.LogError("Steam Deck upload error: Cannot connect to Steam Deck. Is the Steam Deck powered on and in Game Mode?");
                    }
                    else
                    {
                        Debug.LogError("Steam Deck upload error: " + webRequest.error);
                    }
                }
                else
                {
                    Debug.Log("Uploaded to Steam Deck");
                }
            }
        }
    }
}