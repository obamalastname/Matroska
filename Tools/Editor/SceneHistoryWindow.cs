using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Matroska.Tools
{
	public class SceneHistoryWindow : EditorWindow
	{
		// List to hold the paths of recently opened scenes
		private List<string> recentScenes = new List<string>();
		private Vector2 scrollPosition;

		// Add menu named "Scene History" to the Window menu
		[MenuItem("Window/Scene History")]
		static void Init()
		{
			// Get existing open window or if none, make a new one:
			SceneHistoryWindow window = (SceneHistoryWindow)EditorWindow.GetWindow(typeof(SceneHistoryWindow));
			window.Show();
		}

		void OnEnable()
		{
			// Subscribe to the sceneOpened event
			EditorSceneManager.sceneOpened += SceneOpenedCallback;
		}

		void OnDisable()
		{
			// Unsubscribe when the window is closed
			EditorSceneManager.sceneOpened -= SceneOpenedCallback;
		}

		private void SceneOpenedCallback(Scene scene, OpenSceneMode mode)
		{
			// Remove the scene if it already exists in the list
			if (recentScenes.Contains(scene.path))
			{
				recentScenes.Remove(scene.path);
			}

			// Insert the scene at the beginning of the list
			recentScenes.Insert(0, scene.path);

			// Optionally, you might want to limit the number of scenes in the history.
			// if (recentScenes.Count > maxHistory) recentScenes.RemoveAt(recentScenes.Count - 1);

			// Refresh the window
			this.Repaint();
		}

		void OnGUI()
		{
			GUILayout.Label("Recent Scenes", EditorStyles.boldLabel);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);

			string sceneToOpen = null; // Temporary variable to store the path of the scene to open

			// Display each scene as a button
			foreach (string scenePath in recentScenes)
			{
				string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
				if (GUILayout.Button(sceneName))
				{
					// Instead of opening the scene here, store the path to open after the loop
					sceneToOpen = scenePath;
				}
			}

			GUILayout.EndScrollView();

			// If a button was clicked, open the scene and reorder the list
			if (sceneToOpen != null)
			{
				// Open the scene
				EditorSceneManager.OpenScene(sceneToOpen);

				// Reorder the list
				if (recentScenes.Contains(sceneToOpen))
				{
					recentScenes.Remove(sceneToOpen);
				}
				recentScenes.Insert(0, sceneToOpen);

				// Refresh the window
				this.Repaint();
			}
		}
	}
}