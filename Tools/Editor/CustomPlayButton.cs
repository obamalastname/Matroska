using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Matroska 
{
    [InitializeOnLoad]
    public class StartBoostrapOnSceneView
    {
        public const string BOOTSTRAP_SCENE = "bootstrap";
        public const string BOOTSTRAP_PATH = "Assets/Matroska/Scene/bootstrap.unity";
        public static readonly string PreviousScenePathKey = "Matroska.Tools.CustomPlayButton.PreviousSceneKey";
        
        static StartBoostrapOnSceneView()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            if (EditorApplication.isPlaying)
                return;
            
            Handles.BeginGUI();

            Rect rectStartBoostrap = new Rect(50, 10, 100, 20);

            if (GUI.Button(rectStartBoostrap, "Start Bootstrap"))
            {
	            Scene currentScene = SceneManager.GetActiveScene();
                
                EditorPrefs.SetString(PreviousScenePathKey, currentScene.path);

                if (currentScene.name != BOOTSTRAP_SCENE)
                {
                    EditorSceneManager.OpenScene(BOOTSTRAP_PATH);
                }
                
                EditorApplication.EnterPlaymode();
            }

            Rect rectOpenBoostrap = rectStartBoostrap;
            rectOpenBoostrap.x = rectStartBoostrap.x + rectStartBoostrap.width + 10f;
            
            if (GUI.Button(rectOpenBoostrap, "Open Bootstrap"))
            {
	            Scene currentScene = SceneManager.GetActiveScene();
                
	            EditorPrefs.SetString(PreviousScenePathKey, currentScene.path);
	            
	            EditorSceneManager.OpenScene(BOOTSTRAP_PATH);
            }
            
            Rect rectOpenLast = rectOpenBoostrap;
            rectOpenLast.x = rectOpenBoostrap.x + rectOpenBoostrap.width + 10f;
            rectOpenLast.width = 70f;
            
            if (GUI.Button(rectOpenLast, "Open Last"))
            {
	            string previousScenePath = EditorPrefs.GetString(PreviousScenePathKey, "");

	            if (!string.IsNullOrEmpty(previousScenePath))
	            {
		            EditorSceneManager.OpenScene(previousScenePath);
		            EditorPrefs.DeleteKey(PreviousScenePathKey);
	            }
            }

            Handles.EndGUI();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                EditorApplication.delayCall += () =>
                {
                    string previousScenePath = EditorPrefs.GetString(PreviousScenePathKey, "");

                    if (!string.IsNullOrEmpty(previousScenePath))
                    {
                        EditorSceneManager.OpenScene(previousScenePath);
                        EditorPrefs.DeleteKey(PreviousScenePathKey);
                    }
                };
            }
        }
    }
}

