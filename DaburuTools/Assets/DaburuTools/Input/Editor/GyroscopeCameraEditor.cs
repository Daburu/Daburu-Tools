using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DaburuTools
{
	namespace Input
	{
		[CustomEditor(typeof(GyroscopeCamera))]
		[CanEditMultipleObjects]
		public class GyroscopeCameraEditor : Editor
		{
			private GameObject thisGO { get { return ((GyroscopeCamera)serializedObject.targetObject).gameObject; } }

			private SerializedProperty mSP_cameraPerspective;
			private SerializedProperty mSP_thirdPersonPivotOffset;
			private SerializedProperty mSP_mouseSensitivityX;
			private SerializedProperty mSP_mouseSensitivityY;
			private SerializedProperty mSP_lockNHideCursor;

			private GUIContent cameraPerspectiveContent;
			private GUIContent thirdPersonPivotOffsetContent;
			private GUIContent mouseSensitivityXContent;
			private GUIContent mouseSensitivityYContent;
			private GUIContent lockNHideCursorContent;

			private bool mbNeedChangeCameraPerspective = false;
			private bool mbNeedChangeThirdPersonPivotOffset = false;

			private void OnEnable()
			{
				mSP_cameraPerspective		= serializedObject.FindProperty("mCameraPerspective");
				mSP_thirdPersonPivotOffset	= serializedObject.FindProperty("mfThirdPersonPivotOffset");
				mSP_mouseSensitivityX		= serializedObject.FindProperty("mouseSensitivityX");
				mSP_mouseSensitivityY		= serializedObject.FindProperty("mouseSensitivityY");
				mSP_lockNHideCursor			= serializedObject.FindProperty("mbLockNHideCursor");

				cameraPerspectiveContent		= new GUIContent("Camera Perspective", "The perspective of the camera, first or third");
				thirdPersonPivotOffsetContent	= new GUIContent("Pivot Offset", "The z distance of the third person camera from the camPivot");
				mouseSensitivityXContent		= new GUIContent("Mouse Sensitivity X", "Sensitivity for mouse X-axis movement");
				mouseSensitivityYContent		= new GUIContent("Mouse Sensitivity Y", "Sensitivity for mouse Y-axis movement");
				lockNHideCursorContent 			= new GUIContent("Lock N Hide Cursor", "If true, locks and hide the cursor");
			}

			public override void OnInspectorGUI()
			{
				serializedObject.Update();

				EditorGUILayout.PropertyField(serializedObject.FindProperty("camPivot"));

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(mSP_cameraPerspective, cameraPerspectiveContent);
				if (EditorGUI.EndChangeCheck())
					mbNeedChangeCameraPerspective = true;
				if (mSP_cameraPerspective.enumValueIndex == 1)
				{
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(mSP_thirdPersonPivotOffset, thirdPersonPivotOffsetContent);
					if (EditorGUI.EndChangeCheck())
						mbNeedChangeThirdPersonPivotOffset = true;
				}

				EditorGUILayout.PropertyField(mSP_mouseSensitivityX, mouseSensitivityXContent);
				EditorGUILayout.PropertyField(mSP_mouseSensitivityY, mouseSensitivityYContent);
				EditorGUILayout.PropertyField(mSP_lockNHideCursor, lockNHideCursorContent);

				serializedObject.ApplyModifiedProperties();


				if (mbNeedChangeCameraPerspective)
					UpdatePerspective(mSP_cameraPerspective.enumValueIndex);

				if (mbNeedChangeThirdPersonPivotOffset)
					UpdateThirdPersonPivotOffset();
			}

			private void UpdatePerspective(int _perspective)
			{
				Transform parentTransform;
				GyroscopeCamera gyroCamScript = thisGO.GetComponent<GyroscopeCamera>();

				switch (_perspective)
				{
				case 0:	// First person
					parentTransform = thisGO.transform.parent;
					if (parentTransform == null)
						break;
					else if (parentTransform.gameObject != gyroCamScript.camPivot)
						break;

					// Remove the gameObject.
					Undo.SetTransformParent(thisGO.transform, parentTransform.parent, "Set CamPivot as Parent");
					Undo.DestroyObjectImmediate(parentTransform.gameObject);

					EditorGUIUtility.PingObject(thisGO);
					break;
				case 1:	// Third person
					parentTransform = thisGO.transform.parent;
					if (parentTransform != null && parentTransform.gameObject == gyroCamScript.camPivot)
						break;

					// Create the new gameObject
					GameObject newCamPivot = new GameObject("CamPivot");
					Undo.RegisterCreatedObjectUndo(newCamPivot, "Create CamPivot");

					newCamPivot.transform.position = thisGO.transform.position;
					newCamPivot.transform.position += thisGO.transform.forward * mSP_thirdPersonPivotOffset.floatValue;

					Undo.SetTransformParent(newCamPivot.transform, thisGO.transform.parent, "Match newCamPivot parent");
					Undo.SetTransformParent(thisGO.transform, newCamPivot.transform, "Set newCamPivot as parent");

					gyroCamScript.camPivot = newCamPivot;
					EditorGUIUtility.PingObject(thisGO);
					break;
				}

				mbNeedChangeCameraPerspective = false;
			}

			private void UpdateThirdPersonPivotOffset()
			{
				if (mSP_cameraPerspective.enumValueIndex != 1)
				{
					Debug.LogError("Attempted to Update Pivot Offset. Failed as camera is not in third person mode");
					return;
				}

				Undo.RecordObject(thisGO.transform, "Update Pivot Offset");
				thisGO.transform.localPosition = -Vector3.forward * mSP_thirdPersonPivotOffset.floatValue;

				mbNeedChangeThirdPersonPivotOffset = false;
			}
		}
	}
}
