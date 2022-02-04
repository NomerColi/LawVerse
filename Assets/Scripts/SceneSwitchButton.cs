#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public class SceneSwitchButton {

    static SceneSwitchButton () {
        ToolbarExtender.LeftToolbarGUI.Add (OnToolBarGUI);
    }

    static void OnToolBarGUI () {
        if(Application.isPlaying)
            return;
        GUILayout.FlexibleSpace ();
        foreach (var sceneName in new string[] { "Main", "World" }) {
            if (GUILayout.Button (new GUIContent (sceneName, "Load " + sceneName), ToolbarStyles.commandButtonStyle)) {
                SceneSwitchButton.OpenScene (sceneName);
            }
        }
    }

    static class ToolbarStyles {
        public static readonly GUIStyle commandButtonStyle;
        public static readonly GUIStyle commandGameButtonStyle;

        static ToolbarStyles () {
            commandButtonStyle = new GUIStyle ("Command") {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold,
                fixedWidth = 60
            };
            commandGameButtonStyle = new GUIStyle ("Command") {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold,
                fixedWidth = 80
            };
        }

    }

    public static void OpenScene(string sceneName)
    {
        var path = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(sceneName + " t:scene").First());
        EditorSceneManager.OpenScene(path);
    }
    

}

#endif