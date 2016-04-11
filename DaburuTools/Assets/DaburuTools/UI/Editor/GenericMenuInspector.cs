using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DaburuTools
{
	namespace UI
	{
		[CustomEditor(typeof(GenericMenu))]
		public class GenericMenuInspector : Editor
		{
			public override void OnInspectorGUI()
			{
				serializedObject.Update();

				DrawMenuButtonProperties();

				serializedObject.ApplyModifiedProperties();
			}



			private void DrawMenuButtonProperties()
			{
				SerializedProperty buttonNameArr = serializedObject.FindProperty("mArrStrButtonNames");
				SerializedProperty buttonEventArr = serializedObject.FindProperty("mArrButtonEvents");

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Menu Button Properties:", EditorStyles.boldLabel);

				for (int index = 0; index < buttonNameArr.arraySize; index++)
				{
					// Div-like wrapper for each button data.
					GUIStyle style = new GUIStyle();
					style.margin = style.padding = new RectOffset(0, 0, 7, 7);
					Rect buttonDataRect = EditorGUILayout.BeginVertical(style);
					EditorGUI.DrawRect(buttonDataRect, Color.grey);
					buttonDataRect.Set(buttonDataRect.x + 1, buttonDataRect.y + 1, buttonDataRect.width - 2, buttonDataRect.height - 2);
					EditorGUI.DrawRect(buttonDataRect, Color.white);

					DrawButtonDataAtIndex(buttonNameArr, buttonEventArr, index);

					EditorGUILayout.EndVertical();
				}

				GUIContent addButtonContent = new GUIContent("Add Menu Button", "Adds one button to the menu.");
				if (GUILayout.Button(addButtonContent))
				{
					buttonNameArr.arraySize++;
					buttonEventArr.arraySize++;

					Debug.Log("Number of Buttons Changed - Button Added");
				}
			}

			private void DrawButtonDataAtIndex(SerializedProperty _names, SerializedProperty _events, int _index)
			{
				bool bNeedDeleteIndex = false;

				EditorGUILayout.BeginHorizontal();
				GUIContent buttonNameElementContent = new GUIContent("Button " + _index, "The display text of the button");

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(_names.GetArrayElementAtIndex(_index), buttonNameElementContent);
				if (EditorGUI.EndChangeCheck())
				{
					Debug.Log("Button " + _index + " has its name changed");
				}

				GUIContent moveButtonDownContent = new GUIContent("\u21b4", "Move this button down.");
				if (GUILayout.Button(moveButtonDownContent, GUILayout.Width(20f), GUILayout.Height(14f)))
				{
					if ((_index + 1) < _names.arraySize)
					{
						_names.MoveArrayElement(_index, _index + 1);
						_events.MoveArrayElement(_index, _index + 1);

						Debug.Log("Button " + _index + " has moved to " + (_index + 1));
					}
					else
					{
						Debug.LogWarning("Button is at bottom. Cannot move further down");
					}
				}

				GUIContent deleteButtonContent = new GUIContent("-", "Delete this button.");
				if (GUILayout.Button(deleteButtonContent, GUILayout.Width(20f), GUILayout.Height(14f)))
				{
					bNeedDeleteIndex = true;
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();

				// Display OnClick events.
				EditorGUI.BeginChangeCheck();
				GUIContent buttonEventElementContent = new GUIContent("OnClick", "Functions to be executed when the menu button is clicked");
				EditorGUILayout.PropertyField(_events.GetArrayElementAtIndex(_index), buttonEventElementContent);
				if (EditorGUI.EndChangeCheck())
				{
					Debug.Log("Button " + _index + " On Click methods changed");
				}

				if (bNeedDeleteIndex)
				{
					// The double delete is because of the way unity handles deletion in this case.
					// If it references something, it will clear the reference, but not remove the element from the list.
					int nameOldSize = _names.arraySize;
					_names.DeleteArrayElementAtIndex(_index);
					if (_names.arraySize == nameOldSize)
						_names.DeleteArrayElementAtIndex(_index);

					int eventOldSize = _events.arraySize;
					_events.DeleteArrayElementAtIndex(_index);
					if (_events.arraySize == eventOldSize)
						_events.DeleteArrayElementAtIndex(_index);

					Debug.Log("Number of Buttons Changed - Button Removed");
				}
			}
		}
	}
}
