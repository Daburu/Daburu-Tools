using UnityEngine;
using UnityEditor;
using System.IO;

namespace DaburuTools
{
	namespace Actions
	{
		public class DaburuTools_Action_ActionScriptTemplateWindow : EditorWindow
		{
			private string mStrActionName = "";
			private static bool mbFailLoadTemplate = false;
			private static bool mbFailNewAsset = false;

			public static void Init()
			{
				// Setup Variables
				mbFailLoadTemplate = false;
				mbFailNewAsset = false;

				// Get existing window or if none, make a new one.
				DaburuTools_Action_ActionScriptTemplateWindow window =
					(DaburuTools_Action_ActionScriptTemplateWindow)EditorWindow.GetWindowWithRect(
						typeof(DaburuTools_Action_ActionScriptTemplateWindow),
						new Rect(0, 0, 250, 130),
						true,
						"New Action Script");
				window.Show();
			}

			void OnGUI()
			{
				// Set up styles.
				GUIStyle previewStyle = new GUIStyle("label");
				previewStyle.normal.textColor = new Color(0.25f, 0.25f, 0.25f);

				GUILayout.Label ("Name Your New Action", EditorStyles.boldLabel);
				GUILayout.BeginHorizontal();
				GUILayout.Label("Action Name: ", GUILayout.Width(75));
				mStrActionName = GUILayout.TextField(mStrActionName);
				GUILayout.EndHorizontal();
				GUILayout.Label("Preview name: " + mStrActionName + "Action.cs", previewStyle);
				if (GUILayout.Button("Create Action Script"))
				{
					// Create script!
					if (CreateNewActionScript(mStrActionName + "Action") == true)
					{
						Close();
					}
				}

				// Cascading Info logs of priority.
				if (mbFailLoadTemplate)
				{
					EditorGUILayout.HelpBox(
						"Failed to load ActionScriptTemplate. Plase make sure the DaburuTools folder is in /Assets/",
						MessageType.Error, true);
				}
				else if (mbFailNewAsset)
				{
					EditorGUILayout.HelpBox(
						"Script of same name exists in the same Assets folder. Please name this Action something else.",
						MessageType.Error, true);
				}
				else
				{
					EditorGUILayout.HelpBox(
						"Type in your desired Action name in the textfield above and click the \"Create Action Script\" button.",
						MessageType.Info, true);
				}
			}

			private bool CreateNewActionScript(string _name)
			{
				TextAsset templateTextFile = AssetDatabase.LoadAssetAtPath("Assets/DaburuTools/Action/Editor/ActionScriptTemplate.txt",
					typeof(TextAsset)) as TextAsset;
				if (templateTextFile == null)
				{
					Debug.LogError("Failed to load ActionScriptTemplate. Plase make sure the DaburuTools folder is in /Assets/");
					mbFailLoadTemplate = true;
					return false;
				}
				mbFailLoadTemplate = false;
					
				string contents = "";
				contents = templateTextFile.text;
				contents = contents.Replace("#SCRIPTNAME#", _name);
				string newAssetPath = DTEditorUtility.GetSelectedPathOrFallback();
				newAssetPath += "/" + _name + ".cs";

				if(File.Exists(newAssetPath) == false)	// Do not overwrite
				{
					using (StreamWriter outfile = 
						new StreamWriter(newAssetPath))
					{
						outfile.Write(contents);
					}//File written

					AssetDatabase.Refresh();

					// Highlight the asset.
					Selection.activeObject = (Object)AssetDatabase.LoadAssetAtPath(newAssetPath, typeof(MonoScript));
					EditorGUIUtility.PingObject(Selection.activeObject);
					return true;
				}

				Debug.LogError("Script of same name exists in the same Assets folder. Please name this Action something else.");
				mbFailNewAsset = true;
				return false;
			}
		}
	}
}
