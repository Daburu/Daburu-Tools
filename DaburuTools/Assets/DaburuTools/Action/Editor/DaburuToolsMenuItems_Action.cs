using UnityEngine;
using UnityEditor;
using DaburuTools.Action;

namespace DaburuTools
{
	namespace Actions
	{
		public class DaburuToolsMenuItems_Action
		{
			#region ActionHandler
			[MenuItem("Assets/Create/DaburuTools/Action Handler", false, 0)]
			[MenuItem("DaburuTools/Action/Create ActionHandler", false, 1)]
			private static void DT_Action_CreateActionHandler()
			{
				ActionHandler[] actionHandlers = GameObject.FindObjectsOfType<ActionHandler>();
				if (actionHandlers.Length == 0)
				{
					Object ActionHandlerPrefab = AssetDatabase.LoadAssetAtPath("Assets/DaburuTools/Action/DaburuTools_ActionHandler.prefab", typeof(GameObject));
					if (((GameObject)ActionHandlerPrefab).GetComponent<ActionHandler>() != null)
					{
						GameObject actionHandler = PrefabUtility.InstantiatePrefab(ActionHandlerPrefab) as GameObject;
						Undo.RegisterCreatedObjectUndo(actionHandler, "Create ActionHandler");
						// Remove the "(Clone)" of the name.
						actionHandler.name = actionHandler.name.Remove(actionHandler.name.Length - 7);
						Debug.Log("ActionHanlder created successfully.");
					}
					else
					{
						Debug.LogWarning("Operation Failed. Did you shift anything in the DaburuTools folder?");
					}
				}
				else
				{
					Debug.LogWarning("There is already an ActionHandler instance.\nPlease check your hierarchy to ensure that there is only ONE instance of the ActionHandler prefab.");
				}
			}
				
			[MenuItem("DaburuTools/Action/Delete All ActionHandler", false, 2)]
			private static void DT_Action_DeleteAllActionHandler()
			{
				ActionHandler[] actionHandlers = GameObject.FindObjectsOfType<ActionHandler>();

				if (actionHandlers.Length == 0)
				{
					Debug.LogWarning("No instances of ActionHandler were found.");
					return;
				}

				int instancesLeft = actionHandlers.Length;
				int count = 0;
				for (int i = instancesLeft - 1; i > -1; i--)
				{
					Undo.DestroyObjectImmediate(actionHandlers[i].gameObject);
					count++;
				}
				Debug.Log("Deleted all ActionHandler instances successfully.\nTotal of " + count + " instances found and removed.");
			}
			#endregion



			#region ActionTemplate
			[MenuItem("Assets/Create/DaburuTools/Action Script", false, 1)]
			[MenuItem("DaburuTools/Action/Create New Action Script From Template", false, 21)]
			private static void DT_Action_CreateNewActionScriptFromTemplate()
			{
				DaburuTools_Action_ActionScriptTemplateWindow.Init();
			}
			#endregion
		}
	}
}
