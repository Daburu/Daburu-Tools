using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DaburuTools
{
	namespace Input
	{
		[CustomEditor(typeof(GyroControl))]
		[CanEditMultipleObjects]
		public class GyroControlEditor : Editor
		{
			private GameObject thisGO { get { return ((GyroControl)serializedObject.targetObject).gameObject; } }

			private SerializedProperty mSP_mouseSensitivityX;
			private SerializedProperty mSP_mouseSensitivityY;
			private SerializedProperty mSP_lockNHideCursor;
			private SerializedProperty mSP_rotationPivot;
			private SerializedProperty mSP_isPivotRotating;
			private SerializedProperty mSP_enumSnapTo;

			private GUIContent mouseSensitivityXContent;
			private GUIContent mouseSensitivityYContent;
			private GUIContent lockNHideCursorContent;
			private GUIContent rotationPivotContent;
			private GUIContent isPivotRotatingContent;
			private GUIContent enumSnapToContent;

			private void OnEnable()
			{
				mSP_mouseSensitivityX		= serializedObject.FindProperty("mouseSensitivityX");
				mSP_mouseSensitivityY		= serializedObject.FindProperty("mouseSensitivityY");
				mSP_lockNHideCursor			= serializedObject.FindProperty("mbLockNHideCursor");
				mSP_rotationPivot			= serializedObject.FindProperty("m_RotationPivot");
				mSP_isPivotRotating			= serializedObject.FindProperty("bIsPivotRotating");
				mSP_enumSnapTo				= serializedObject.FindProperty("enum_snapTo");

				mouseSensitivityXContent		= new GUIContent("Mouse Sensitivity X", "Sensitivity for mouse X-axis movement");
				mouseSensitivityYContent		= new GUIContent("Mouse Sensitivity Y", "Sensitivity for mouse Y-axis movement");
				lockNHideCursorContent 			= new GUIContent("Lock N Hide Cursor", "If true, locks and hide the cursor");
				rotationPivotContent			= new GUIContent("Rotation Pivot", "The pivot of gyroscope rotation. If nothing is assigned , the pivot will be the center of the current transform");
				isPivotRotatingContent			= new GUIContent("Is Pivot Rotating", "Determines if the pivot should rotate too. This will not affect if the pivot is the current object");
				enumSnapToContent				= new GUIContent("Snap To Mode", "Determine what SnapToPoint() do. Worldaxis = snaps current rotation to world axis, InitialRotation = snaps current rotation to the initial rotation of the object");
			}

			public override void OnInspectorGUI()
			{
				serializedObject.Update();

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Editor Mouse Emulation", EditorStyles.boldLabel);
				EditorGUILayout.PropertyField(mSP_mouseSensitivityX, mouseSensitivityXContent);
				EditorGUILayout.PropertyField(mSP_mouseSensitivityY, mouseSensitivityYContent);
				EditorGUILayout.PropertyField(mSP_lockNHideCursor, lockNHideCursorContent);

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Pivot Properties", EditorStyles.boldLabel);
				EditorGUILayout.PropertyField(mSP_rotationPivot, rotationPivotContent);
				Transform rotationPivot = mSP_rotationPivot.objectReferenceValue as System.Object as Transform;
				if (rotationPivot == null)
				{
					EditorGUILayout.HelpBox("If PropertyField is not set, it defaults to the current transform", MessageType.Info, true);
				}
				else
				{
					if (rotationPivot != thisGO.transform)
					{
						EditorGUILayout.PropertyField(mSP_isPivotRotating, isPivotRotatingContent);
					}
				}

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Reset Properties", EditorStyles.boldLabel);
				EditorGUILayout.PropertyField(mSP_enumSnapTo, enumSnapToContent);

				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}
